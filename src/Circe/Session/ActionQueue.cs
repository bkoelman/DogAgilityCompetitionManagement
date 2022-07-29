using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

namespace DogAgilityCompetition.Circe.Session;

/// <summary>
/// Represents a producer/consumer queue that asynchronously executes a single <see cref="Action" /> at a time.
/// </summary>
/// <remarks>
/// <para>
/// Enqueued actions can be monitored through their returned <see cref="Task" />s, which enable waiting upon completion, inspecting if the task was
/// canceled or whether an exception was thrown, and running a continuation upon completion.
/// </para>
/// <para>
/// Consumption of actions in the queue can be paused and resumed. On disposal, any remaining actions are canceled.
/// </para>
/// </remarks>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public sealed class ActionQueue : IDisposable
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

#pragma warning disable CA2213 // Disposable fields should be disposed
    // Justification: This member is disposed by ConsumerLoop() instead of Dispose().
    private readonly BlockingCollection<WorkItem> workQueue = new();
#pragma warning restore CA2213 // Disposable fields should be disposed

    private readonly object stateLock = new();

    private bool isConsumerRunning; // Protected by stateLock
    private bool disposeRequested; // Protected by stateLock

    public ActionQueue()
    {
        Log.Debug("Creating task for consumer loop.");
        Task.Factory.StartNew(ConsumerLoop, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    /// <summary>
    /// Starts the consumer.
    /// </summary>
    public void Start()
    {
        using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

        lock (stateLock)
        {
            lockTracker.Acquired();

            if (!disposeRequested && !isConsumerRunning)
            {
                isConsumerRunning = true;

                Log.Debug("Signaling state change after start.");
                Monitor.Pulse(stateLock);
            }
            else
            {
                Log.Debug("Already started or disposed.");
            }
        }
    }

    /// <summary>
    /// Pauses the consumer. Blocks until the current action has completed.
    /// </summary>
    public void Pause()
    {
        using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

        lock (stateLock)
        {
            lockTracker.Acquired();

            if (!disposeRequested && isConsumerRunning)
            {
                isConsumerRunning = false;

                Log.Debug("Signaling state change after pause.");
                Monitor.Pulse(stateLock);
            }
            else
            {
                Log.Debug("Already paused or disposed.");
            }
        }
    }

    /// <summary>
    /// Blocks until an already running action has completed, then cancels all remaining actions in the queue.
    /// </summary>
    public void Dispose()
    {
        using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

        lock (stateLock)
        {
            lockTracker.Acquired();

            if (!disposeRequested)
            {
                disposeRequested = true;

                Log.Debug("Dispose: Signaling state change after dispose.");
                Monitor.Pulse(stateLock);

                workQueue.CompleteAdding();
            }
            else
            {
                Log.Debug("Already disposed.");
            }
        }
    }

    /// <summary>
    /// Adds an action to the queue that can be canceled before or during execution.
    /// </summary>
    /// <param name="action">
    /// The action to enqueue.
    /// </param>
    /// <param name="cancelToken">
    /// The token to signal cancellation.
    /// </param>
    /// <returns>
    /// The <see cref="Task" /> that represents the enqueued action.
    /// </returns>
    public Task Enqueue(Action action, CancellationToken cancelToken)
    {
        Guard.NotNull(action, nameof(action));

        Log.Debug("Adding queue entry.");
        var taskSource = new TaskCompletionSource<object?>();
        workQueue.Add(new WorkItem(taskSource, action, cancelToken), cancelToken);

        Log.Debug($"Created task {taskSource.Task.Id}.");

        // Uncomment the next block to simulate task failure.
        /*
        Task.Run(() =>
        {
            Thread.Sleep(1000);
            taskSource.SetException(new Exception("Task failed."));
        });
        */

        return taskSource.Task;
    }

    private void ConsumerLoop()
    {
        Log.Debug("Entering ConsumerLoop.");

        foreach (WorkItem workItem in workQueue.GetConsumingEnumerable())
        {
            try
            {
                if (workItem.CancelToken.IsCancellationRequested)
                {
                    // Action was canceled (and task was signaled) while waiting in the queue, so ignore it.
                    Log.Debug("Skipping pre-canceled queue entry.");
                    continue;
                }

                using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (!isConsumerRunning && !disposeRequested)
                    {
                        Log.Debug("Entering wait loop until running or disposing.");

                        while (!isConsumerRunning && !disposeRequested)
                        {
                            // Consumer is paused, so block this thread until state changes.
                            Monitor.Wait(stateLock);
                        }

                        Log.Debug("Exited wait loop.");
                    }

                    if (disposeRequested)
                    {
                        // Dispose has been called by producer, so cancel whatever is left in the queue.

                        Log.Debug("Disposal has been requested, canceling queue entry.");
                        workItem.TaskSource.TrySetCanceled();
                        continue;
                    }

                    int taskId = workItem.TaskSource.Task.Id;

                    // Perform action inside lock, so that state transitions will block 
                    // until the current action has completed.
                    try
                    {
                        workItem.CancelToken.ThrowIfCancellationRequested();

                        Log.Debug($"Executing action for task {taskId}.");
                        workItem.Action();
                        workItem.TaskSource.TrySetResult(null);
                        Log.Debug("Task completed.");
                    }
                    catch (OperationCanceledException)
                    {
                        Log.Debug($"Propagating cancellation request to task {taskId}.");
                        workItem.TaskSource.TrySetCanceled();
                    }
                    catch (Exception ex)
                    {
                        Log.Debug($"Setting task {taskId} to error state.");
                        workItem.TaskSource.TrySetException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error in ConsumerLoop.", ex);
            }
        }

        workQueue.Dispose();

        Log.Debug("Leaving ConsumerLoop.");
    }

    /// <summary>
    /// Represents an entry in the producer/consumer queue.
    /// </summary>
    private readonly struct WorkItem
    {
        public TaskCompletionSource<object?> TaskSource { get; }
        public Action Action { get; }
        public CancellationToken CancelToken { get; }

        public WorkItem(TaskCompletionSource<object?> taskSource, Action action, CancellationToken cancelToken)
            : this()
        {
            Guard.NotNull(taskSource, nameof(taskSource));
            Guard.NotNull(action, nameof(action));

            TaskSource = taskSource;
            Action = action;
            CancelToken = cancelToken;

            // This ensures that cancellation of an enqueued action immediately propagates
            // to the returned task. Later when the entry gets dequeued, it will be ignored.
            cancelToken.Register(CancelTask);
        }

        private void CancelTask()
        {
            Log.Debug($"Propagating cancellation request to task {TaskSource.Task.Id}.");
            TaskSource.TrySetCanceled();
        }
    }
}
