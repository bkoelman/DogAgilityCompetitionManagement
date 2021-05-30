using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private static readonly TimeSpan MaxIdleTime = TimeSpan.FromMilliseconds(700);

        private readonly FreshEnum<MediatorConnectionState> connectionState = new(MediatorConnectionState.Disconnected);
        private readonly FreshObjectReference<string?> connectedComPort = new(null);
        private readonly MediatorIncomingOperationDispatcher operationDispatcher;
        private readonly BlockingCollection<Operation> sendQueue = new();
        private readonly ManualResetEventSlim powerOffRequestedWaitHandle = new(false);
        private readonly ManualResetEventSlim powerOffCompletedWaitHandle = new(false);
        private readonly FreshObjectReference<Version> protocolVersion = new(KeepAliveOperation.CurrentProtocolVersion);
        private readonly FreshObjectReference<string?> portName = new(null);
        private readonly FreshInt32 mediatorStatus = new(0);
        private readonly FreshBoolean awaitingAddressAssignment = new(false);
        private readonly FreshObjectReference<IWirelessDevice?> mediator = new(null);
        private readonly DeviceStatusChangeLogger deviceChangeLogger = new();

        private DateTime lastSendTime;

        public string? ComPortName
        {
            get => portName.Value;
            set => portName.Value = value;
        }

        public Version ProtocolVersion
        {
            get => protocolVersion.Value;
            set
            {
                Guard.NotNull(value, nameof(value));
                protocolVersion.Value = value;
            }
        }

        public int StatusCode
        {
            get => mediatorStatus.Value;
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

        public IWirelessDevice? Mediator
        {
            get => mediator.Value;
            set
            {
                Guard.NotNull(value, nameof(value));
                mediator.Value = value;
            }
        }

        public ConcurrentDictionary<WirelessNetworkAddress, IWirelessDevice> Devices { get; } = new();

        public event EventHandler? PacketSending;
        public event EventHandler? PacketReceived;
        public event EventHandler<MediatorConnectionStateEventArgs>? ConnectionStateChanged;
        public event EventHandler? StatusCodeChanged;

        public CirceMediatorSessionManager()
        {
            operationDispatcher = new MediatorIncomingOperationDispatcher(this);
        }

        private void ConnectionOnOperationReceived(object? sender, IncomingOperationEventArgs e)
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

        private void Setup(DeviceSetupOperation operation)
        {
            if (operation.CapabilitiesOrNone == DeviceCapabilities.None)
            {
                if (Mediator != null && Mediator.Address == operation.DestinationAddressOrDefault && Mediator.IsPoweredOn)
                {
                    SetupMediator(operation.AssignAddress!);
                }
                else
                {
                    Log.Error($"No on-line mediator with address {operation.DestinationAddressOrDefault} found to setup.");
                }
            }
            else
            {
                // Note: This code path is filled with race conditions. Not intended for production scenarios.
                IWirelessDevice? targetDevice =
                    Devices.Values.SingleOrDefault(device => device.Address == operation.DestinationAddressOrDefault && device.IsPoweredOn);

                if (targetDevice != null)
                {
                    SetupDevice(targetDevice.Address, operation.AssignAddress!, operation.CapabilitiesOrNone);
                }
                else
                {
                    Log.Error($"No on-line device with address {operation.DestinationAddressOrDefault} found to setup.");
                }
            }
        }

        private void SetupMediator(WirelessNetworkAddress newAddress)
        {
            if (Mediator == null)
            {
                throw new InvalidOperationException("Mediator must be set first.");
            }

            Mediator.ChangeAddress(newAddress);
            ResetMediatorStatusAfterSetup();
        }

        private void SetupDevice(WirelessNetworkAddress oldAddress, WirelessNetworkAddress newAddress, DeviceCapabilities capabilities)
        {
            if (Devices.TryRemove(oldAddress, out IWirelessDevice? device))
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

            Task.Factory.StartNew(SenderLoop, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
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

            CirceComConnection? connection = null;

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
                                connection = ComPortSelector.GetConnection(AttachConnectionHandlers, DetachConnectionHandlers, portName.Value);
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

                            SendOperation(connectionNotNull1, new KeepAliveOperation(protocolVersion.Value, mediatorStatus.Value));
                            ClearSendQueue();
                            ChangeConnectionStateTo(MediatorConnectionState.Connected, connectedComPort.Value);
                            break;
                        case MediatorConnectionState.Connected:
                            Log.Debug("SenderLoop: Processing outgoing operations queue.");
                            CirceComConnection connectionNotNull2 = AssertConnectionNotNull(connection);

                            while (sendQueue.TryTake(out Operation? nextOperation))
                            {
                                SendOperation(connectionNotNull2, nextOperation);
                            }

                            if (lastSendTime.Add(MaxIdleTime) < SystemContext.UtcNow())
                            {
                                SendOperation(connectionNotNull2, new KeepAliveOperation(KeepAliveOperation.CurrentProtocolVersion, mediatorStatus.Value));
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

        private void AttachConnectionHandlers(CirceComConnection connection)
        {
            connection.PacketSending += PacketSending;
            connection.PacketReceived += PacketReceived;
            connection.OperationReceived += ConnectionOnOperationReceived;
        }

        private void DetachConnectionHandlers(CirceComConnection connection)
        {
            connection.PacketSending -= PacketSending;
            connection.PacketReceived -= PacketReceived;
            connection.OperationReceived -= ConnectionOnOperationReceived;
        }

        [AssertionMethod]
        private static CirceComConnection AssertConnectionNotNull(CirceComConnection? connection)
        {
            return Assertions.InternalValueIsNotNull(() => connection, () => connection);
        }

        private void ChangeConnectionStateTo(MediatorConnectionState state, string? comPortName)
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

            while (sendQueue.TryTake(out _))
            {
                count++;
            }

            if (count > 0)
            {
                Log.Debug($"Flushed {count} outgoing operations from queue.");
            }
        }

        private void SendOperation(CirceComConnection connection, Operation operation)
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

        public void NotifyStatus(DeviceStatus deviceStatus)
        {
            Guard.NotNull(deviceStatus, nameof(deviceStatus));

            deviceChangeLogger.Update(deviceStatus);
            sendQueue.Add(deviceStatus.ToOperation());
        }

        public void NotifyOffline(WirelessNetworkAddress deviceAddress)
        {
            deviceChangeLogger.Remove(deviceAddress);
        }

        public void NotifyAction(DeviceAction deviceAction)
        {
            Guard.NotNull(deviceAction, nameof(deviceAction));
            sendQueue.Add(deviceAction.ToOperation());
        }

        public void LogData(byte[] data)
        {
            Guard.NotNullNorEmpty(data, nameof(data));

            var operation = new LogOperation(data);
            sendQueue.Add(operation);
        }

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
            private readonly CirceMediatorSessionManager owner;

            public MediatorIncomingOperationDispatcher(CirceMediatorSessionManager owner)
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

                IWirelessDevice? targetDevice = GetPoweredOnDeviceWithAddressOrNull(operation.DestinationAddress);
                targetDevice?.Accept(operation);
            }

            void IOperationAcceptor.Accept(NetworkSetupOperation operation)
            {
                Guard.NotNull(operation, nameof(operation));

                IWirelessDevice? targetDevice = GetPoweredOnDeviceWithAddressOrNull(operation.DestinationAddress);
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

            private IWirelessDevice? GetPoweredOnDeviceWithAddressOrNull(WirelessNetworkAddress? address)
            {
                return address == null ? null : owner.Devices.Values.SingleOrDefault(device => device.Address == address && device.IsPoweredOn);
            }

            private IEnumerable<IWirelessDevice> GetDevicesWithAddresses(IEnumerable<WirelessNetworkAddress> addresses)
            {
                // @formatter:keep_existing_linebreaks true

                IEnumerable<IWirelessDevice> devices = addresses
                    .Select(GetPoweredOnDeviceWithAddressOrNull)
                    .Where(device => device != null)
                    .Cast<IWirelessDevice>();

                // @formatter:keep_existing_linebreaks restore

                return devices;
            }
        }
    }
}
