using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Mediator
{
    /// <summary>
    /// Provides a mediator session by delegating access to a remotely connected CIRCE controller.
    /// </summary>
    public sealed class CirceMediatorSessionManager : IDisposable
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly TimeSpan MaxIdleTime = TimeSpan.FromMilliseconds(700);

        [NotNull]
        private readonly FreshEnum<MediatorConnectionState> connectionState =
            new FreshEnum<MediatorConnectionState>(MediatorConnectionState.Disconnected);

        [NotNull]
        private readonly FreshReference<string> connectedComPort = new FreshReference<string>(null);

        [NotNull]
        private readonly MediatorIncomingOperationDispatcher operationDispatcher;

        [NotNull]
        [ItemNotNull]
        private readonly BlockingCollection<Operation> sendQueue = new BlockingCollection<Operation>();

        [NotNull]
        private readonly ManualResetEventSlim powerOffRequestedWaitHandle = new ManualResetEventSlim(false);

        [NotNull]
        private readonly ManualResetEventSlim powerOffCompletedWaitHandle = new ManualResetEventSlim(false);

        [NotNull]
        private readonly FreshNotNullableReference<Version> protocolVersion =
            new FreshNotNullableReference<Version>(KeepAliveOperation.CurrentProtocolVersion);

        [NotNull]
        private readonly FreshReference<string> portName = new FreshReference<string>(null);

        [CanBeNull]
        public string ComPortName
        {
            get
            {
                return portName.Value;
            }
            set
            {
                portName.Value = value;
            }
        }

        [NotNull]
        public Version ProtocolVersion
        {
            get
            {
                return protocolVersion.Value;
            }
            set
            {
                Guard.NotNull(value, nameof(value));
                protocolVersion.Value = value;
            }
        }

        [NotNull]
        private readonly FreshInt32 mediatorStatus = new FreshInt32(0);

        [NotNull]
        private readonly FreshBoolean awaitingAddressAssignment = new FreshBoolean(false);

        public int StatusCode
        {
            get
            {
                return mediatorStatus.Value;
            }
            set
            {
                if (value != mediatorStatus.Value)
                {
                    Guard.InRangeInclusive(value, nameof(value), 0, 999);
                    mediatorStatus.Value = value;

                    if (value == 1)
                    {
                        awaitingAddressAssignment.Value = true;
                    }

                    StatusCodeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [NotNull]
        private readonly FreshReference<IWirelessDevice> mediator = new FreshReference<IWirelessDevice>(null);

        [CanBeNull]
        public IWirelessDevice Mediator
        {
            get
            {
                return mediator.Value;
            }
            set
            {
                Guard.NotNull(value, nameof(value));
                mediator.Value = value;
            }
        }

        [NotNull]
        public ConcurrentDictionary<WirelessNetworkAddress, IWirelessDevice> Devices { get; } =
            new ConcurrentDictionary<WirelessNetworkAddress, IWirelessDevice>();

        [NotNull]
        private readonly DeviceStatusChangeLogger deviceChangeLogger = new DeviceStatusChangeLogger();

        public event EventHandler PacketSending;
        public event EventHandler PacketReceived;
        public event EventHandler<MediatorConnectionStateEventArgs> ConnectionStateChanged;
        public event EventHandler StatusCodeChanged;

        public CirceMediatorSessionManager()
        {
            operationDispatcher = new MediatorIncomingOperationDispatcher(this);
        }

        private void ConnectionOnOperationReceived([CanBeNull] object sender, [NotNull] IncomingOperationEventArgs e)
        {
            Log.Debug($"Incoming {e.Operation} on {e.Connection.PortName}.");
            e.Operation.Visit(operationDispatcher);
        }

        private void Login()
        {
            ChangeConnectionStateTo(MediatorConnectionState.LoginReceived, connectedComPort.Value);
        }

        private void Logout()
        {
            ChangeConnectionStateTo(MediatorConnectionState.WaitingForLogin, connectedComPort.Value);
        }

        private void Setup([NotNull] DeviceSetupOperation operation)
        {
            if (operation.CapabilitiesOrNone == DeviceCapabilities.None)
            {
                if (Mediator != null && Mediator.Address == operation.DestinationAddressOrDefault &&
                    Mediator.IsPoweredOn)
                {
                    SetupMediator(operation.AssignAddress);
                }
                else
                {
                    Log.Error(
                        $"No on-line mediator with address {operation.DestinationAddressOrDefault} found to setup.");
                }
            }
            else
            {
                // Note: This code path is filled with race conditions. Not intended for production scenarios.
                IWirelessDevice targetDevice =
                    Devices.Values.SingleOrDefault(
                        device => device.Address == operation.DestinationAddressOrDefault && device.IsPoweredOn);
                if (targetDevice != null)
                {
                    SetupDevice(targetDevice.Address, operation.AssignAddress, operation.CapabilitiesOrNone);
                }
                else
                {
                    Log.Error($"No on-line device with address {operation.DestinationAddressOrDefault} found to setup.");
                }
            }
        }

        private void SetupMediator([NotNull] WirelessNetworkAddress newAddress)
        {
            if (Mediator == null)
            {
                throw new InvalidOperationException("Mediator must be set first.");
            }

            Mediator.ChangeAddress(newAddress);
            ResetMediatorStatusAfterSetup();
        }

        private void SetupDevice([NotNull] WirelessNetworkAddress oldAddress,
            [NotNull] WirelessNetworkAddress newAddress, DeviceCapabilities capabilities)
        {
            IWirelessDevice device;
            if (Devices.TryRemove(oldAddress, out device))
            {
                Devices[newAddress] = device;
                deviceChangeLogger.ChangeAddress(oldAddress, newAddress, capabilities);

                device.ChangeAddress(newAddress);
                ResetMediatorStatusAfterSetup();
            }
        }

        private void ResetMediatorStatusAfterSetup()
        {
            if (awaitingAddressAssignment.Value)
            {
                awaitingAddressAssignment.Value = false;
                StatusCode = 0;
            }
        }

        public void PowerOn()
        {
            Log.Debug("Entering PowerOn.");
            if (connectionState.Value != MediatorConnectionState.Disconnected)
            {
                InnerPowerOff();
            }

            powerOffRequestedWaitHandle.Reset();

            Task.Factory.StartNew(SenderLoop, CancellationToken.None, TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            Log.Info("Mediator on-line.");

            Log.Debug("Leaving PowerOn.");
        }

        public void PowerOff()
        {
            Log.Debug("Entering PowerOff.");
            if (connectionState.Value != MediatorConnectionState.Disconnected)
            {
                InnerPowerOff();

                Log.Info("Mediator off-line.");
            }
            Log.Debug("Leaving PowerOff.");
        }

        private void InnerPowerOff()
        {
            Log.Debug("Starting PowerOff.");
            powerOffCompletedWaitHandle.Reset();
            powerOffRequestedWaitHandle.Set();
            powerOffCompletedWaitHandle.Wait();
            Log.Debug("Finished PowerOff.");
        }

        private void SenderLoop()
        {
            Log.Debug("Entering SenderLoop.");
            ChangeConnectionStateTo(MediatorConnectionState.WaitingForComPort, null);

            CirceComConnection connection = null;
            while (true)
            {
                try
                {
                    if (powerOffRequestedWaitHandle.IsSet)
                    {
                        Log.Debug("SenderLoop: PowerOff request received.");
                        connection?.Dispose();
                        ChangeConnectionStateTo(MediatorConnectionState.Disconnected, connectedComPort.Value);

                        powerOffCompletedWaitHandle.Set();
                        Log.Debug("Leaving SenderLoop.");
                        return;
                    }

                    switch (connectionState.Value)
                    {
                        case MediatorConnectionState.WaitingForComPort:
                            Log.Debug($"SenderLoop: Opening port {portName.Value ?? "(auto)"}.");
                            try
                            {
                                connection = ComPortSelector.GetConnection(AttachConnectionHandlers,
                                    DetachConnectionHandlers, portName.Value);
                                connectedComPort.Value = connection.PortName;
                                Log.Debug($"SenderLoop: Awaiting Login on port {connectedComPort.Value}.");
                                ChangeConnectionStateTo(MediatorConnectionState.WaitingForLogin, connectedComPort.Value);
                            }
                            catch (SerialConnectionException ex)
                            {
                                Log.Debug($"Unable to open an available COM port: {ex.Message}");
                            }
                            break;
                        case MediatorConnectionState.LoginReceived:
                            Log.Debug("SenderLoop: Responding with KeepAlive after incoming Login.");
                            CirceComConnection connectionNotNull1 = AssertConnectionNotNull(connection);

                            SendOperation(connectionNotNull1,
                                new KeepAliveOperation(protocolVersion.Value, mediatorStatus.Value));
                            ClearSendQueue();
                            ChangeConnectionStateTo(MediatorConnectionState.Connected, connectedComPort.Value);
                            break;
                        case MediatorConnectionState.Connected:
                            Log.Debug("SenderLoop: Processing outgoing operations queue.");
                            CirceComConnection connectionNotNull2 = AssertConnectionNotNull(connection);

                            Operation nextOperation;
                            while (sendQueue.TryTake(out nextOperation))
                            {
                                SendOperation(connectionNotNull2, nextOperation);
                            }

                            if (lastSendTime.Add(MaxIdleTime) < SystemContext.UtcNow())
                            {
                                SendOperation(connectionNotNull2,
                                    new KeepAliveOperation(KeepAliveOperation.CurrentProtocolVersion,
                                        mediatorStatus.Value));
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Unexpected error in SenderLoop.", ex);
                }

                // If powering off, must wake immediately. Otherwise, we'll sleep some time.
                //Log.Debug("Starting sleep on wait handle.");
                powerOffRequestedWaitHandle.Wait(250);
            }
        }

        private void AttachConnectionHandlers([NotNull] CirceComConnection connection)
        {
            connection.PacketSending += PacketSending;
            connection.PacketReceived += PacketReceived;
            connection.OperationReceived += ConnectionOnOperationReceived;
        }

        private void DetachConnectionHandlers([NotNull] CirceComConnection connection)
        {
            connection.PacketSending -= PacketSending;
            connection.PacketReceived -= PacketReceived;
            connection.OperationReceived -= ConnectionOnOperationReceived;
        }

        [AssertionMethod]
        [NotNull]
        private static CirceComConnection AssertConnectionNotNull([CanBeNull] CirceComConnection connection)
        {
            return Assertions.InternalValueIsNotNull(() => connection, () => connection);
        }

        private void ChangeConnectionStateTo(MediatorConnectionState state, [CanBeNull] string comPortName)
        {
            if (state != connectionState.Value)
            {
                connectionState.Value = state;
                ConnectionStateChanged?.Invoke(this, new MediatorConnectionStateEventArgs(state, comPortName));
            }
        }

        private void ClearSendQueue()
        {
            int count = 0;

            Operation unused;
            while (sendQueue.TryTake(out unused))
            {
                count++;
            }

            if (count > 0)
            {
                Log.Debug($"Flushed {count} outgoing operations from queue.");
            }
        }

        private DateTime lastSendTime;

        private void SendOperation([NotNull] CirceComConnection connection, [NotNull] Operation operation)
        {
            Guard.NotNull(connection, nameof(connection));
            Guard.NotNull(operation, nameof(operation));

            Log.Debug($"Outgoing {operation} on {connection.PortName}.");
            try
            {
                connection.Send(operation);
                lastSendTime = SystemContext.UtcNow();
            }
            catch (Exception ex)
            {
                Log.Error($"Error while sending {operation} on {connection}", ex);
            }
        }

        public void NotifyStatus([NotNull] DeviceStatus deviceStatus)
        {
            Guard.NotNull(deviceStatus, nameof(deviceStatus));

            deviceChangeLogger.Update(deviceStatus);
            sendQueue.Add(deviceStatus.ToOperation());
        }

        public void NotifyOffline([NotNull] WirelessNetworkAddress deviceAddress)
        {
            deviceChangeLogger.Remove(deviceAddress);
        }

        public void NotifyAction([NotNull] DeviceAction deviceAction)
        {
            Guard.NotNull(deviceAction, nameof(deviceAction));
            sendQueue.Add(deviceAction.ToOperation());
        }

        public void LogData([NotNull] byte[] data)
        {
            Guard.NotNullNorEmpty(data, nameof(data));

            var operation = new LogOperation(data);
            sendQueue.Add(operation);
        }

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "sendQueue",
            Justification =
                "BlockingCollection<T> cannot be disposed as long as consumers exist. Therefore, Dispose is not called from anywhere."
            )]
        public void Dispose()
        {
            sendQueue.CompleteAdding();

            if (connectionState.Value != MediatorConnectionState.Disconnected)
            {
                InnerPowerOff();
            }

            powerOffRequestedWaitHandle.Dispose();
            powerOffCompletedWaitHandle.Dispose();
        }

        private sealed class MediatorIncomingOperationDispatcher : IOperationAcceptor
        {
            [NotNull]
            private readonly CirceMediatorSessionManager owner;

            public MediatorIncomingOperationDispatcher([NotNull] CirceMediatorSessionManager owner)
            {
                Guard.NotNull(owner, nameof(owner));
                this.owner = owner;
            }

            void IOperationAcceptor.Accept(LoginOperation operation)
            {
                owner.Login();
            }

            void IOperationAcceptor.Accept(LogoutOperation operation)
            {
                owner.Logout();
            }

            void IOperationAcceptor.Accept(AlertOperation operation)
            {
                Guard.NotNull(operation, nameof(operation));

                IWirelessDevice targetDevice = GetPoweredOnDeviceWithAddressOrNull(operation.DestinationAddress);
                targetDevice?.Accept(operation);
            }

            void IOperationAcceptor.Accept(NetworkSetupOperation operation)
            {
                Guard.NotNull(operation, nameof(operation));

                IWirelessDevice targetDevice = GetPoweredOnDeviceWithAddressOrNull(operation.DestinationAddress);
                targetDevice?.Accept(operation);
            }

            void IOperationAcceptor.Accept(DeviceSetupOperation operation)
            {
                owner.Setup(operation);
            }

            void IOperationAcceptor.Accept(SynchronizeClocksOperation operation)
            {
                foreach (IWirelessDevice targetDevice in owner.Devices.Values)
                {
                    IWirelessDevice snapshot = targetDevice;
                    snapshot.Accept(operation);
                }
            }

            void IOperationAcceptor.Accept(VisualizeOperation operation)
            {
                Guard.NotNull(operation, nameof(operation));

                foreach (IWirelessDevice targetDevice in GetDevicesWithAddresses(operation.DestinationAddresses))
                {
                    IWirelessDevice snapshot = targetDevice;
                    snapshot.Accept(operation);
                }
            }

            void IOperationAcceptor.Accept(KeepAliveOperation operation)
            {
            }

            void IOperationAcceptor.Accept(NotifyStatusOperation operation)
            {
            }

            void IOperationAcceptor.Accept(NotifyActionOperation operation)
            {
            }

            [CanBeNull]
            private IWirelessDevice GetPoweredOnDeviceWithAddressOrNull([NotNull] WirelessNetworkAddress address)
            {
                return owner.Devices.Values.SingleOrDefault(device => device.Address == address && device.IsPoweredOn);
            }

            [NotNull]
            [ItemNotNull]
            private IEnumerable<IWirelessDevice> GetDevicesWithAddresses(
                [NotNull] [ItemNotNull] IEnumerable<WirelessNetworkAddress> addresses)
            {
                return addresses.Select(GetPoweredOnDeviceWithAddressOrNull).Where(device => device != null);
            }
        }
    }
}