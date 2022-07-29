using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.Circe.Controller;

/// <summary>
/// Provides a controller session by delegating access to a remotely connected CIRCE mediator.
/// </summary>
public sealed class CirceControllerSessionManager : IDisposable
{
    // When performance stagnates on high load, consider increasing Thread-pool size. 
    // Defaults for .exe are: Min=8, Max=1000

    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly ActionQueue outgoingOperationsQueue;
    private readonly SessionGuard sessionGuard;
    private readonly ControllerIncomingOperationDispatcher operationDispatcher;

    public DeviceTracker DeviceTracker { get; }

    public event EventHandler? PacketSending;
    public event EventHandler? PacketReceived;
    public event EventHandler<ControllerConnectionStateEventArgs>? ConnectionStateChanged;
    public event EventHandler<EventArgs<DeviceAction>>? DeviceActionReceived;

    public CirceControllerSessionManager()
    {
        operationDispatcher = new ControllerIncomingOperationDispatcher(this);
        DeviceTracker = new DeviceTracker();
        outgoingOperationsQueue = new ActionQueue();

        sessionGuard = new SessionGuard(outgoingOperationsQueue);
        sessionGuard.StateChanged += SessionGuardOnStateChanged;
        sessionGuard.OperationReceived += OperationReceived;
    }

    public void Start()
    {
        sessionGuard.BeforePacketSent += PacketSending;
        sessionGuard.AfterPacketReceived += PacketReceived;

        sessionGuard.Start();
    }

    public void Dispose()
    {
        // Order is important here: the session guard will pause the queue before cleaning up the connection.
        sessionGuard.Dispose();
        outgoingOperationsQueue.Dispose();

        DeviceTracker.Dispose();
    }

    public Task AlertAsync(WirelessNetworkAddress destinationAddress, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(destinationAddress, nameof(destinationAddress));

        var operation = new AlertOperation(destinationAddress);

        return sessionGuard.SendAsync(operation, cancellationToken);
    }

    public Task NetworkSetupAsync(WirelessNetworkAddress destinationAddress, bool joinNetwork, DeviceRoles roles, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(destinationAddress, nameof(destinationAddress));

        var operation = new NetworkSetupOperation(destinationAddress, joinNetwork, roles);

        return sessionGuard.SendAsync(operation, cancellationToken);
    }

    public Task SynchronizeClocksAsync(CancellationToken cancellationToken = default)
    {
        var operation = new SynchronizeClocksOperation();
        return sessionGuard.SendAsync(operation, cancellationToken);
    }

    public Task VisualizeAsync(IEnumerable<WirelessNetworkAddress> destinationAddresses, VisualizeFieldSet fieldSet,
        CancellationToken cancellationToken = default)
    {
        var operation = new VisualizeOperation(destinationAddresses)
        {
            CurrentCompetitorNumber = fieldSet.CurrentCompetitorNumber,
            NextCompetitorNumber = fieldSet.NextCompetitorNumber,
            StartTimer = fieldSet.StartPrimaryTimer,
            PrimaryTimerValue = fieldSet.PrimaryTimerValue,
            SecondaryTimerValue = fieldSet.SecondaryTimerValue,
            FaultCount = fieldSet.CurrentFaultCount,
            RefusalCount = fieldSet.CurrentRefusalCount,
            Eliminated = fieldSet.CurrentIsEliminated,
            PreviousPlacement = fieldSet.PreviousPlacement
        };

        return sessionGuard.SendAsync(operation, cancellationToken);
    }

    private void SessionGuardOnStateChanged(object? sender, ControllerConnectionStateEventArgs e)
    {
        ConnectionStateChanged?.Invoke(this, e);
    }

    private void HandleIncomingKeepAliveOperation(KeepAliveOperation operation)
    {
        // Justification for nullable suppression: Operation has been validated for required parameters when this code is reached.
        DeviceTracker.UpdateMediatorStatus(operation.MediatorStatus!.Value);
    }

    private void HandleIncomingNotifyStatusOperation(NotifyStatusOperation operation)
    {
        if (IsDeviceCompliant(operation))
        {
            DeviceStatus deviceStatus = DeviceStatus.FromOperation(operation);
            DeviceTracker.SetDeviceStatus(deviceStatus);
        }
    }

    private void HandleIncomingNotifyActionOperation(NotifyActionOperation operation)
    {
        // Justification for nullable suppression: Operation has been validated for required parameters when this code is reached.
        DeviceTracker.NotifyDeviceIsAlive(operation.OriginatingAddress!);

        DeviceActionReceived?.Invoke(this, new EventArgs<DeviceAction>(DeviceAction.FromOperation(operation)));
    }

    private void OperationReceived(object? sender, IncomingOperationEventArgs e)
    {
        e.Operation.Visit(operationDispatcher);
    }

    private static bool IsDeviceCompliant(NotifyStatusOperation notifyStatusOperation)
    {
        // Justification for nullable suppression: Operation has been validated for required parameters when this code is reached.
        if (!AreCapabilitiesValid(notifyStatusOperation.Capabilities!.Value))
        {
            Log.Warn($"Discarding device {notifyStatusOperation.OriginatingAddress} " +
                $"because combination of capabilities is invalid ({notifyStatusOperation.Capabilities}).");

            return false;
        }

        if (notifyStatusOperation.OriginatingAddress == WirelessNetworkAddress.Default)
        {
            Log.Warn($"Discarding unconfigured device {notifyStatusOperation.OriginatingAddress}.");
            return false;
        }

        return true;
    }

    private static bool AreCapabilitiesValid(DeviceCapabilities capabilities)
    {
        if (capabilities == DeviceCapabilities.None)
        {
            return false;
        }

        if ((capabilities & DeviceCapabilities.TimeSensor) != 0 && capabilities != DeviceCapabilities.TimeSensor)
        {
            return false;
        }

        if ((capabilities & DeviceCapabilities.Display) != 0 && capabilities != DeviceCapabilities.Display)
        {
            return false;
        }

        return true;
    }

    private sealed class ControllerIncomingOperationDispatcher : IOperationAcceptor
    {
        private static readonly ISystemLogger InnerLog = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

        private readonly CirceControllerSessionManager owner;
        private readonly object nonReentrantProcessIncomingOperationLock = new();

        public ControllerIncomingOperationDispatcher(CirceControllerSessionManager owner)
        {
            Guard.NotNull(owner, nameof(owner));
            this.owner = owner;
        }

        public void Accept(LoginOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(LogoutOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(AlertOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(NetworkSetupOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(DeviceSetupOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(SynchronizeClocksOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(VisualizeOperation operation)
        {
            WarnForUnexpectedOperation(operation);
        }

        public void Accept(KeepAliveOperation operation)
        {
            using var lockTracker = new LockTracker(InnerLog, MethodBase.GetCurrentMethod()!);

            lock (nonReentrantProcessIncomingOperationLock)
            {
                lockTracker.Acquired();

                owner.HandleIncomingKeepAliveOperation(operation);
            }
        }

        public void Accept(NotifyStatusOperation operation)
        {
            using var lockTracker = new LockTracker(InnerLog, MethodBase.GetCurrentMethod()!);

            lock (nonReentrantProcessIncomingOperationLock)
            {
                lockTracker.Acquired();

                owner.HandleIncomingNotifyStatusOperation(operation);
            }
        }

        public void Accept(NotifyActionOperation operation)
        {
            using var lockTracker = new LockTracker(InnerLog, MethodBase.GetCurrentMethod()!);

            lock (nonReentrantProcessIncomingOperationLock)
            {
                lockTracker.Acquired();

                owner.HandleIncomingNotifyActionOperation(operation);
            }
        }

        private static void WarnForUnexpectedOperation(Operation operation)
        {
            InnerLog.Warn($"Discarding unexpected incoming operation: {operation}");
        }
    }
}
