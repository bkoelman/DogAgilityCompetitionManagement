using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// Performs connecting to a remote mediator, keeping the session alive; automatically reconnects when the session is lost.
    /// </summary>
    internal sealed class SessionGuard : IDisposable
    {
        private const int TryNextComPortDelayInMilliseconds = 100;

        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private static readonly Version ProtocolVersionExpected =
            new(KeepAliveOperation.CurrentProtocolVersion.Major, KeepAliveOperation.CurrentProtocolVersion.Minor);

        private readonly ComPortRotator portRotator = new();
        private readonly ActionQueue outgoingOperationsQueue;
        private readonly ManualResetEventSlim disposeRequestedWaitHandle = new(false);
        private readonly ManualResetEventSlim reconnectLoopTerminatedWaitHandle = new(false);
        private readonly FreshObjectReference<CirceComConnection?> activeConnection = new(null);
        private readonly FreshDateTime lastRefreshTimeInUtc = new(DateTime.MinValue);
        private readonly FreshNullableBoolean seenProtocolVersionMismatch = new(null);

        private readonly object stateLock = new();

        private bool hasBeenStarted; // Protected by stateLock
        private bool hasBeenDisposed; // Protected by stateLock

        public event EventHandler? BeforePacketSent;
        public event EventHandler? AfterPacketReceived;
        public event EventHandler<ControllerConnectionStateEventArgs>? StateChanged;
        public event EventHandler<IncomingOperationEventArgs>? OperationReceived;

        public SessionGuard(ActionQueue outgoingOperationsQueue)
        {
            Guard.NotNull(outgoingOperationsQueue, nameof(outgoingOperationsQueue));

            this.outgoingOperationsQueue = outgoingOperationsQueue;
        }

        public void Start()
        {
            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (hasBeenStarted || hasBeenDisposed)
                    {
                        Log.Debug("Already started or disposed.");
                        return;
                    }

                    hasBeenStarted = true;
                }
            }

            Log.Debug("Creating task for reconnect loop.");

            Task.Factory.StartNew(ReconnectLoop, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(_ => reconnectLoopTerminatedWaitHandle.Set());
        }

        public void Dispose()
        {
            // We protect against disposing multiple times sequentially, not concurrently.

            bool disposeAlreadyDone;
            bool mustWaitForReconnectLoopToComplete;

            using (var lockTracker = new LockTracker(Log, "Dispose (pre)"))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    disposeAlreadyDone = hasBeenDisposed;
                    mustWaitForReconnectLoopToComplete = hasBeenStarted;
                }
            }

            if (!disposeAlreadyDone)
            {
                if (mustWaitForReconnectLoopToComplete)
                {
                    Log.Debug("Signaling reconnect loop to terminate.");
                    disposeRequestedWaitHandle.Set();

                    Log.Debug("Waiting until reconnect loop has terminated.");
                    reconnectLoopTerminatedWaitHandle.Wait();
                }

                Log.Debug("Cleaning up wait handles.");
                disposeRequestedWaitHandle.Dispose();
                reconnectLoopTerminatedWaitHandle.Dispose();

                using var lockTracker = new LockTracker(Log, "Dispose (post)");

                lock (stateLock)
                {
                    lockTracker.Acquired();

                    hasBeenDisposed = true;
                }
            }
            else
            {
                Log.Debug("Already disposed.");
            }
        }

        public Task SendAsync(Operation operation, CancellationToken cancellationToken)
        {
            Guard.NotNull(operation, nameof(operation));

            Log.Debug($"Adding outgoing operation to queue: {operation}");

            return outgoingOperationsQueue.Enqueue(() =>
            {
                // The reconnect thread will pause the queue before changing the active connection.
                // In turn, when the queue gets paused, it will block until the current action has
                // completed. So we can assume that the active connection never changes while this 
                // callback is running.

                CirceComConnection? connectionSnapshot = activeConnection.Value;

                if (connectionSnapshot == null)
                {
                    throw new NotConnectedToMediatorException();
                }

                if (!TryDirectSend(connectionSnapshot, false, operation, out Exception? error))
                {
                    throw new Exception("Failed to send outgoing operation.", error);
                }
            }, cancellationToken);
        }

        private void ReconnectLoop()
        {
            Log.Debug("Entering ReconnectLoop.");

            while (true)
            {
                try
                {
                    if (disposeRequestedWaitHandle.IsSet)
                    {
                        Log.Debug("ReconnectLoop: Disposal has been requested.");
                        CleanupSession();

                        Log.Debug("Leaving ReconnectLoop.");
                        return;
                    }

#if DEBUGGING_HACKS
                    bool needsReconnect = lastRefreshTimeInUtc.Value == DateTime.MinValue; // Connect first time only, never disconnect
#else
                    bool needsReconnect = lastRefreshTimeInUtc.Value.AddSeconds(2) < SystemContext.UtcNow();
#endif

                    if (needsReconnect)
                    {
                        Log.Debug("Reconnect required after expired lifetime.");

                        CleanupSession();

                        if (disposeRequestedWaitHandle.IsSet)
                        {
                            Log.Debug("ReconnectLoop: Disposal has been requested (after session cleanup).");

                            Log.Debug("Leaving ReconnectLoop.");
                            return;
                        }

                        CreateSession();
                    }

                    // If disposing, must wake immediately. Otherwise, we'll sleep some time 
                    // before re-checking the session.
                    //Log.Debug("Starting sleep on wait handle.");
                    disposeRequestedWaitHandle.Wait(250);
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected error in ReconnectLoop.", ex);
                }
            }
        }

        private void CleanupSession()
        {
            Log.Debug("Entering CleanupSession.");
            outgoingOperationsQueue.Pause();

            // It should not be needed to set ActiveConnection to null until we have reconnected,
            // but doing so will cause NullReferenceExceptions, which are easier to trace (just in case).
            CirceComConnection? previousConnection = activeConnection.Exchange(null);

            if (previousConnection != null)
            {
                Log.Debug("Starting cleanup of previous connection.");

                // Best effort: If possible, notify the mediator to stop sending.

                // The outgoing queue has been paused. So we are guaranteed to be the only 
                // thread that is writing to the port. So locking for exclusive write 
                // access is not needed here.

                if (!TryDirectSend(previousConnection, false, new LogoutOperation(), out Exception? logoutError))
                {
                    Log.Debug("Failed to send Logout operation.", logoutError);
                }

                // Because the lifetime of this instance is longer than the lifetime of a connection, it
                // is critical to detach event handlers so the garbage collector can run. Failing to detach
                // event handlers will cause a memory leak that increases at each reconnect.
                previousConnection.OperationReceived -= NewConnectionOnOperationReceived;
                previousConnection.PacketSending -= NewConnectionOnPacketSending;
                previousConnection.PacketReceived -= NewConnectionOnPacketReceived;

                previousConnection.Dispose();
                Log.Debug("Finished cleanup of previous connection.");

                OnStateChanged(ControllerConnectionState.Disconnected, previousConnection.PortName);
            }
        }

        private void CreateSession()
        {
            Log.Debug("Entering CreateSession.");

            // Block until we have successfully sent a Login operation.
            Exception? connectError;
            CirceComConnection newConnection;

            do
            {
                // Block until we have obtained a COM port.
                bool raisedWaitEvent = false;

                Log.Debug("Entering wait loop until at least one port found.");

#if DEBUGGING_HACKS
                string comPortName = "COM3"; // Force single COM port
#else
                string? comPortName = portRotator.GetNextPortName();
#endif

                while (comPortName == null)
                {
                    if (disposeRequestedWaitHandle.IsSet)
                    {
                        Log.Debug("CreateSession: Disposal has been requested while scanning for ports.");
                        OnStateChanged(ControllerConnectionState.Disconnected, null);
                        return;
                    }

                    if (!raisedWaitEvent)
                    {
                        OnStateChanged(ControllerConnectionState.WaitingForComPort, null);
                        raisedWaitEvent = true;
                    }

                    Thread.Sleep(TryNextComPortDelayInMilliseconds);
                    comPortName = portRotator.GetNextPortName();
                }

                if (disposeRequestedWaitHandle.IsSet)
                {
                    Log.Debug("CreateSession: Disposal has been requested after port detection.");
                    OnStateChanged(ControllerConnectionState.Disconnected, null);
                    return;
                }

                OnStateChanged(ControllerConnectionState.Connecting, comPortName);

                seenProtocolVersionMismatch.Value = null;

                newConnection = new CirceComConnection(comPortName);
                newConnection.PacketSending += NewConnectionOnPacketSending;
                newConnection.PacketReceived += NewConnectionOnPacketReceived;
                newConnection.OperationReceived += NewConnectionOnOperationReceived;

                // The caller has closed a previous connection and paused the outgoing queue. So we
                // are guaranteed to be the only thread that is writing to the port. So locking
                // for exclusive write access is not needed here.
                if (TryDirectSend(newConnection, true, new LoginOperation(), out connectError))
                {
                    // A KeepAlive response should be received shortly, so we'll set the lifetime to one 
                    // second in the past, which leaves a single second for the mediator to respond.
                    lastRefreshTimeInUtc.Value = SystemContext.UtcNow().AddSeconds(-1);
                }

                if (connectError != null)
                {
                    Log.Warn($"Failed to open port {comPortName} and send Login operation.", connectError);

                    // Because the lifetime of this instance is longer than the lifetime of a connection, it
                    // is critical to detach event handlers so the garbage collector can run. Failing to detach
                    // event handlers will cause a memory leak that increases at each reconnect.
                    newConnection.PacketSending -= NewConnectionOnPacketSending;
                    newConnection.PacketReceived -= NewConnectionOnPacketReceived;
                    newConnection.OperationReceived -= NewConnectionOnOperationReceived;
                    newConnection.Dispose();

                    OnStateChanged(ControllerConnectionState.Disconnected, comPortName);
                }
            }
            while (connectError != null);

            activeConnection.Value = newConnection;
        }

        private void NewConnectionOnPacketSending(object? sender, EventArgs e)
        {
            BeforePacketSent?.Invoke(sender, e);
        }

        private void NewConnectionOnPacketReceived(object? sender, EventArgs e)
        {
            AfterPacketReceived?.Invoke(sender, e);
        }

        private static bool TryDirectSend(CirceComConnection connection, bool openBeforeSend, Operation operation, out Exception? error)
        {
            error = null;

            try
            {
                if (openBeforeSend)
                {
                    connection.Open();
                }

                connection.Send(operation);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                // This may happen when you unplug the serial port cable while writing to the port.
                // The exception indicates that the port has been closed, so writing fails.
                error = ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                error = ex;
            }
            catch (SecurityException ex)
            {
                error = ex;
            }
            catch (IOException ex)
            {
                error = ex;
            }

            return false;
        }

        private void NewConnectionOnOperationReceived(object? sender, IncomingOperationEventArgs e)
        {
            Log.Debug("Entering NewConnectionOnOperationReceived.");

            // To be compliant with the CIRCE spec, we should discard any incoming operations other than KeepAlive
            // when in the process of establishing a session. But this may result in losing important information 
            // during a competition run, so we silently assume that no protocol version mismatch occurs and allow 
            // any incoming operation to be handled when we have not received a KeepAlive response to our 
            // Login operation yet.
            // On the other hand, once we have detected a protocol version mismatch, we discard any additional 
            // incoming operations on that same connection.

            // This method is not expected to execute concurrently (due to an exclusive lock in System.Net.SerialPort
            // around invocation of the DataReceived event), so we can safely apply sequential changes related to 
            // version mismatches, without taking an exclusive lock. Furthermore, it seems unlikely we will get here
            // after connection has been closed (not sure if that's possible). Even then, the worst that can happen
            // is a reconnect due to incorrect logic regarding version or keep-alive, which is much less problematic
            // than taking an exclusive lock, risking potential deadlocks.

            if (e.Operation is KeepAliveOperation keepAliveOperation)
            {
                // Major and Minor version must match exactly; Build is ignored here and Revision is not used by the CIRCE spec.

                // Justification for nullable suppression: Operation has been validated for required parameters when this code is reached.
                var incomingVersion = new Version(keepAliveOperation.ProtocolVersion!.Major, keepAliveOperation.ProtocolVersion.Minor);
                bool isVersionMismatch = incomingVersion != ProtocolVersionExpected;

                bool? wasVersionMismatch = seenProtocolVersionMismatch.CompareExchange(isVersionMismatch, null);

                if (wasVersionMismatch == null)
                {
                    // We are in the process of establishing a connection
                    if (isVersionMismatch)
                    {
                        // Detected version mismatch. Force reconnect by setting lifetime expired.
                        Log.Debug("Setting session lifetime to expired after detection of protocol version mismatch " +
                            $"(expected {ProtocolVersionExpected.Major}.{ProtocolVersionExpected.Minor}.*, " +
                            $"received {keepAliveOperation.ProtocolVersion.Major}.{keepAliveOperation.ProtocolVersion.Minor}.*).");

                        lastRefreshTimeInUtc.Value = DateTime.MinValue;
                        OnStateChanged(ControllerConnectionState.ProtocolVersionMismatch, e.Connection.PortName);
                    }
                    else if (keepAliveOperation.MediatorStatus == KnownMediatorStatusCode.MediatorUnconfigured)
                    {
                        // Detected mediator unconfigured. Force reconnect by setting lifetime expired.
                        Log.Debug("Setting session lifetime to expired after detection of unconfigured mediator.");

                        lastRefreshTimeInUtc.Value = DateTime.MinValue;
                        OnStateChanged(ControllerConnectionState.MediatorUnconfigured, e.Connection.PortName);
                    }
                    else
                    {
                        outgoingOperationsQueue.Start();
                        OnStateChanged(ControllerConnectionState.Connected, e.Connection.PortName);
                    }
                }
            }

            bool? seenProtocolVersionMismatchSnapshot = seenProtocolVersionMismatch.Value;

            if (seenProtocolVersionMismatchSnapshot == false)
            {
                Log.Debug("Extending session lifetime.");

                // Extend session lifetime due to any incoming operation.
                lastRefreshTimeInUtc.Value = SystemContext.UtcNow();
            }

            if (seenProtocolVersionMismatchSnapshot == null || seenProtocolVersionMismatchSnapshot == false)
            {
                try
                {
                    OperationReceived?.Invoke(this, e);
                }
                catch (Exception ex)
                {
                    Log.Error($"Unexpected error while processing incoming operation on {e.Connection}: {e.Operation}", ex);
                }
            }
            else
            {
                Log.Warn($"Discarding incoming operation after detection of version mismatch on {e.Connection}: {e.Operation}");
            }
        }

        private void OnStateChanged(ControllerConnectionState connectionState, string? comPort)
        {
            Log.Debug($"Raising state change event with state={connectionState}, port={comPort}.");
            StateChanged?.Invoke(this, new ControllerConnectionStateEventArgs(connectionState, comPort));
        }
    }
}
