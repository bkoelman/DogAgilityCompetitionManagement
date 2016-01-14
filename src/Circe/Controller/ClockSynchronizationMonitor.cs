using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// Performs collective clock synchronization on a set of wireless devices. Signals when last succeeded clock
    /// synchronization happened too long ago.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification =
            "Reason for not disposing CancellationTokenSource, see http://blogs.msdn.com/b/pfxteam/archive/2012/03/25/10287435.aspx"
        )]
    public sealed class ClockSynchronizationMonitor
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly TimeSpan SyncReplyTimeout = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan RecommendSyncAfter = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan RequireSyncAfter = TimeSpan.FromMinutes(15);

        [NotNull]
        private readonly FreshReference<CirceControllerSessionManager> circeSessionManager =
            new FreshReference<CirceControllerSessionManager>(null);

        [NotNull]
        private readonly FreshReference<TaskCompletionSource<ClockSynchronizationResult>> syncTaskSource =
            new FreshReference<TaskCompletionSource<ClockSynchronizationResult>>(null);

        [NotNull]
        private readonly FreshReference<CancellationTokenSource> syncCancellationSource =
            new FreshReference<CancellationTokenSource>(null);

        [NotNull]
        private readonly object stateLock = new object();

        [NotNull]
        private readonly Dictionary<WirelessNetworkAddress, DeviceSyncStatus> deviceMap =
            new Dictionary<WirelessNetworkAddress, DeviceSyncStatus>(); // Protected by stateLock

        [CanBeNull]
        [ItemNotNull]
        private HashSet<WirelessNetworkAddress> networkBeingSynced; // Protected by stateLock

        [CanBeNull]
        private CancellationTokenSource raiseEventsCancellationTokenSource; // Protected by stateLock

        private bool IsSyncInProgress => networkBeingSynced != null;

        public event EventHandler SyncRecommended;
        public event EventHandler SyncRequired;
        public event EventHandler<ClockSynchronizationCompletedEventArgs> SyncCompleted;

        public void Initialize([NotNull] CirceControllerSessionManager sessionManager)
        {
            Guard.NotNull(sessionManager, nameof(sessionManager));

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (circeSessionManager.Value != null)
                    {
                        throw new InvalidOperationException("Already initialized.");
                    }

                    sessionManager.DeviceTracker.DeviceAdded += DeviceTrackerOnDeviceAddedOrChanged;
                    sessionManager.DeviceTracker.DeviceChanged += DeviceTrackerOnDeviceAddedOrChanged;

                    circeSessionManager.Value = sessionManager;
                }
            }
        }

        private void DeviceTrackerOnDeviceAddedOrChanged([CanBeNull] object sender,
            [NotNull] EventArgs<DeviceStatus> eventArgs)
        {
            Guard.NotNull(eventArgs, nameof(eventArgs));

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (!deviceMap.ContainsKey(eventArgs.Argument.DeviceAddress))
                    {
                        deviceMap[eventArgs.Argument.DeviceAddress] = new DeviceSyncStatus();
                    }
                    deviceMap[eventArgs.Argument.DeviceAddress].Update(eventArgs.Argument.ClockSynchronization);

                    if (deviceMap[eventArgs.Argument.DeviceAddress].IsSynchronized && IsSyncInProgress)
                    {
                        VerifySynchronizationComplete();
                    }
                }
            }
        }

        private void VerifySynchronizationComplete()
        {
            bool allSucceeded = networkBeingSynced.All(SeenAndSynchronized);

            if (allSucceeded)
            {
                syncTaskSource.Value?.TrySetResult(ClockSynchronizationResult.Succeeded);
            }
        }

        private bool SeenAndSynchronized([NotNull] WirelessNetworkAddress deviceAddress)
        {
            return deviceMap.ContainsKey(deviceAddress) && deviceMap[deviceAddress].IsSynchronized;
        }

        public void Suspend()
        {
            AssertInitialized();

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (raiseEventsCancellationTokenSource != null)
                    {
                        raiseEventsCancellationTokenSource.Cancel();
                        raiseEventsCancellationTokenSource = null;
                    }

                    syncCancellationSource.Value?.Cancel();
                }
            }
        }

        public void StartNetworkSynchronization(
            [NotNull] [ItemNotNull] IEnumerable<WirelessNetworkAddress> devicesInNetwork)
        {
            Guard.NotNull(devicesInNetwork, nameof(devicesInNetwork));
            CirceControllerSessionManager sessionManager = AssertInitialized();

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    AssertNotSyncInProgress();

                    Suspend();

                    networkBeingSynced = new HashSet<WirelessNetworkAddress>(devicesInNetwork);

                    if (!NetworkContainsSynchronizableDevices())
                    {
                        // Only devices without a clock. No need for sync or raise events for clock re-sync.
                        Task.Factory.StartNew(
                            () => HandleNetworkSynchronizationCompleted(ClockSynchronizationResult.Succeeded),
                            CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                        return;
                    }

                    ResetDevices();
                }
            }

            syncCancellationSource.Value = new CancellationTokenSource();
            Task invokeOperationTask = sessionManager.SynchronizeClocksAsync(syncCancellationSource.Value.Token);

            invokeOperationTask.ContinueWith(opTask =>
            {
                if (opTask.IsFaulted)
                {
                    Log.Warn("Failed sending SynchronizeClocks operation.", opTask.Exception);
                    HandleNetworkSynchronizationCompleted(ClockSynchronizationResult.Failed);
                }
                else if (opTask.IsCanceled)
                {
                    Log.Warn("Timeout on sending SynchronizeClocks operation.");
                    HandleNetworkSynchronizationCompleted(ClockSynchronizationResult.CanceledOrTimedOut);
                }
                else
                {
                    Task<ClockSynchronizationResult> waitForRepliesTask =
                        WaitForRepliesAsync(syncCancellationSource.Value.Token);

                    waitForRepliesTask.ContinueWith(repliesTask =>
                    {
                        if (repliesTask.IsFaulted)
                        {
                            Log.Warn("Failed waiting for Sync replies.", opTask.Exception);
                            HandleNetworkSynchronizationCompleted(ClockSynchronizationResult.Failed);
                        }
                        else if (repliesTask.IsCanceled)
                        {
                            string succeeded = deviceMap.Any()
                                ? string.Join(", ", deviceMap.Keys.Where(IsDeviceSynchronized))
                                : "(none)";
                            Log.Warn($"Timeout on waiting for Sync Succeeded replies. Devices succeeded: {succeeded}");
                            HandleNetworkSynchronizationCompleted(ClockSynchronizationResult.CanceledOrTimedOut);
                        }
                        else
                        {
                            HandleNetworkSynchronizationCompleted(repliesTask.Result);
                        }
                    }, TaskScheduler.Current);

                    AutoCancelTaskAfterTimeout(waitForRepliesTask, syncCancellationSource.Value);
                }
            }, TaskScheduler.Current);

            AutoCancelTaskAfterTimeout(invokeOperationTask, syncCancellationSource.Value);
        }

        private void HandleNetworkSynchronizationCompleted(ClockSynchronizationResult synchronizationResult)
        {
            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (synchronizationResult == ClockSynchronizationResult.Succeeded)
                    {
                        Resume();
                    }

                    networkBeingSynced = null;
                }
            }

            try
            {
                SyncCompleted?.Invoke(this, new ClockSynchronizationCompletedEventArgs(synchronizationResult));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to handle clock synchronization update for {synchronizationResult}.", ex);
            }
        }

        private bool NetworkContainsSynchronizableDevices()
        {
            return networkBeingSynced != null && networkBeingSynced.Any();
        }

        private void Resume()
        {
            if (NetworkContainsSynchronizableDevices())
            {
                raiseEventsCancellationTokenSource = new CancellationTokenSource();

                Task.Delay(RecommendSyncAfter, raiseEventsCancellationTokenSource.Token).ContinueWith(recommendTask =>
                {
                    if (!recommendTask.IsCanceled)
                    {
                        SyncRecommended?.Invoke(this, EventArgs.Empty);
                    }
                });

                Task.Delay(RequireSyncAfter, raiseEventsCancellationTokenSource.Token).ContinueWith(requireTask =>
                {
                    if (!requireTask.IsCanceled)
                    {
                        SyncRequired?.Invoke(this, EventArgs.Empty);
                    }
                });
            }
        }

        [AssertionMethod]
        [NotNull]
        private CirceControllerSessionManager AssertInitialized()
        {
            if (circeSessionManager.Value == null)
            {
                throw new InvalidOperationException("Call Initialize first.");
            }
            return circeSessionManager.Value;
        }

        [AssertionMethod]
        private void AssertNotSyncInProgress()
        {
            if (IsSyncInProgress)
            {
                throw new InvalidOperationException("Clock synchronization is already in progress.");
            }
        }

        private void ResetDevices()
        {
            foreach (DeviceSyncStatus syncStatus in deviceMap.Values)
            {
                syncStatus.Reset();
            }
        }

        private static void AutoCancelTaskAfterTimeout([NotNull] Task taskToWatch,
            [NotNull] CancellationTokenSource taskCancelTokenSource)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    bool completed = taskToWatch.Wait(SyncReplyTimeout);
                    if (!completed)
                    {
                        taskCancelTokenSource.Cancel();
                    }
                }
                catch (AggregateException)
                {
                    // Do not handle task errors here, caller should in its continuation.
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        [NotNull]
        private Task<ClockSynchronizationResult> WaitForRepliesAsync(CancellationToken cancellationToken)
        {
            syncTaskSource.Value = new TaskCompletionSource<ClockSynchronizationResult>();

            cancellationToken.Register(() => syncTaskSource.Value.TrySetCanceled());

            return syncTaskSource.Value.Task;
        }

        public bool IsDeviceSynchronized([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));
            AssertInitialized();

            bool result = false;

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (deviceMap.ContainsKey(deviceAddress))
                    {
                        result = deviceMap[deviceAddress].IsSynchronized;
                    }
                }
            }

            return result;
        }

        private sealed class DeviceSyncStatus
        {
            private bool deviceIsRequestingSync;

            // When a re-sync is performed, device must first send a status without SyncSucceeded flag, before we
            // accept a status update with the SyncSucceeded flag set. Otherwise we do not know whether the success
            // belongs to the current or previous synchronization.
            private bool seenOtherStatusAfterSyncSucceeded;

            [CanBeNull]
            private DateTime? syncSucceededAt;

            public bool IsSynchronized
                =>
                    !deviceIsRequestingSync && syncSucceededAt != null &&
                        syncSucceededAt.Value.Add(RequireSyncAfter) > SystemContext.UtcNow();

            public DeviceSyncStatus()
            {
                Reset();
            }

            public void Update([CanBeNull] ClockSynchronizationStatus? status)
            {
                deviceIsRequestingSync = false;

                switch (status)
                {
                    case ClockSynchronizationStatus.RequiresSync:
                        deviceIsRequestingSync = true;
                        break;
                    case ClockSynchronizationStatus.SyncSucceeded:
                        if (seenOtherStatusAfterSyncSucceeded && syncSucceededAt == null)
                        {
                            syncSucceededAt = SystemContext.UtcNow();
                        }
                        break;
                }

                if (!seenOtherStatusAfterSyncSucceeded && status != ClockSynchronizationStatus.SyncSucceeded)
                {
                    seenOtherStatusAfterSyncSucceeded = true;
                }
            }

            public void Reset()
            {
                deviceIsRequestingSync = false;
                syncSucceededAt = null;
            }
        }
    }
}