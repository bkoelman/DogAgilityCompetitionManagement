using System;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Specs.Facilities;
using FluentAssertions;
using NUnit.Framework;

namespace DogAgilityCompetition.Specs.CirceSpecs
{
    /// <summary>
    /// Integration tests (require USB loop-back cable) for a CIRCE logical session, from a Controller perspective.
    /// </summary>
    [TestFixture]
    public sealed class CirceControllerSessionEvents
    {
        [Test]
        [Category("UsbLoopback")]
        public void When_keep_alive_with_incorrect_version_is_received_it_must_disconnect()
        {
            // Arrange
            using (var testRunner = new CirceUsbLoopbackTestRunner())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.ProtocolVersionMismatch)
                    {
                        testRunner.SignalSucceeded();
                    }
                };

                testRunner.ProtocolVersion = new Version(99, 88, 77);

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_keep_alive_with_unconfigured_mediator_status_is_received_it_must_disconnect()
        {
            // Arrange
            using (var testRunner = new CirceUsbLoopbackTestRunner())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.MediatorUnconfigured)
                    {
                        testRunner.SignalSucceeded();
                    }
                };

                testRunner.MediatorStatusCode = KnownMediatorStatusCode.MediatorUnconfigured;

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_keep_alive_operation_is_received_it_must_raise_event_for_mediator_status_change()
        {
            // Arrange
            const int mediatorStatusCode = 5;

            using (var testRunner = new CirceUsbLoopbackTestRunner())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.Connection.Send(new KeepAliveOperation(KeepAliveOperation.CurrentProtocolVersion,
                            mediatorStatusCode));
                    }
                };

                testRunner.RemoteSessionManager.DeviceTracker.MediatorStatusChanged += (s, e) =>
                {
                    if (e.Argument == mediatorStatusCode)
                    {
                        testRunner.SignalSucceeded();
                    }
                };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_notify_status_operation_is_received_it_must_raise_event_for_added_device()
        {
            // Arrange
            var deviceAddress = new WirelessNetworkAddress("AABBCC");
            const bool getMembership = true;
            const DeviceCapabilities capabilities =
                DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad | DeviceCapabilities.StartSensor |
                    DeviceCapabilities.FinishSensor | DeviceCapabilities.IntermediateSensor;
            const DeviceRoles roles = DeviceRoles.StartTimer | DeviceRoles.FinishTimer;
            const int signalStrength = 25;
            const int batteryStatus = 83;
            const bool isAligned = true;
            const ClockSynchronizationStatus clockSynchronization = ClockSynchronizationStatus.RequiresSync;
            const bool hasVersionMismatch = false;

            using (var testRunner = new CirceUsbLoopbackTestRunner<DeviceStatus>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.Connection.Send(new NotifyStatusOperation(deviceAddress, getMembership,
                            capabilities, roles, signalStrength)
                        {
                            BatteryStatus = batteryStatus,
                            IsAligned = isAligned,
                            ClockSynchronization = clockSynchronization,
                            HasVersionMismatch = false
                        });
                    }
                };

                testRunner.RemoteSessionManager.DeviceTracker.DeviceAdded +=
                    (s, e) => { testRunner.SignalSucceeded(e.Argument); };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
                testRunner.Result.DeviceAddress.Should().Be(deviceAddress);
                testRunner.Result.IsInNetwork.Should().Be(getMembership);
                testRunner.Result.Capabilities.Should().Be(capabilities);
                testRunner.Result.Roles.Should().Be(roles);
                testRunner.Result.SignalStrength.Should().Be(signalStrength);
                testRunner.Result.BatteryStatus.Should().Be(batteryStatus);
                testRunner.Result.IsAligned.Should().Be(isAligned);
                testRunner.Result.ClockSynchronization.Should().Be(clockSynchronization);
                testRunner.Result.HasVersionMismatch.Should().Be(hasVersionMismatch);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_notify_status_operation_is_received_twice_it_must_raise_event_for_changed_device()
        {
            // Arrange
            var deviceAddress = new WirelessNetworkAddress("AABBCC");
            const bool getMembership = true;
            const DeviceCapabilities capabilities =
                DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad | DeviceCapabilities.StartSensor |
                    DeviceCapabilities.FinishSensor | DeviceCapabilities.IntermediateSensor;
            const DeviceRoles roles = DeviceRoles.StartTimer | DeviceRoles.FinishTimer;
            const int signalStrength = 99;
            const int batteryStatus = 83;
            const bool isAligned = true;
            const ClockSynchronizationStatus clockSynchronization = ClockSynchronizationStatus.RequiresSync;

            using (var testRunner = new CirceUsbLoopbackTestRunner<DeviceStatus>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.Connection.Send(new NotifyStatusOperation(deviceAddress, getMembership,
                            capabilities, roles, 25));

                        testRunner.Connection.Send(new NotifyStatusOperation(deviceAddress, getMembership,
                            capabilities, roles, signalStrength)
                        {
                            BatteryStatus = batteryStatus,
                            IsAligned = isAligned,
                            ClockSynchronization = clockSynchronization
                        });
                    }
                };

                testRunner.RemoteSessionManager.DeviceTracker.DeviceChanged +=
                    (s, e) => { testRunner.SignalSucceeded(e.Argument); };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
                testRunner.Result.DeviceAddress.Should().Be(deviceAddress);
                testRunner.Result.IsInNetwork.Should().Be(getMembership);
                testRunner.Result.Capabilities.Should().Be(capabilities);
                testRunner.Result.Roles.Should().Be(roles);
                testRunner.Result.SignalStrength.Should().Be(signalStrength);
                testRunner.Result.BatteryStatus.Should().Be(batteryStatus);
                testRunner.Result.IsAligned.Should().Be(isAligned);
                testRunner.Result.ClockSynchronization.Should().Be(clockSynchronization);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_notify_status_operation_is_no_longer_received_it_must_raise_event_for_removed_device()
        {
            // Arrange
            var deviceAddress = new WirelessNetworkAddress("AABBCC");

            using (var testRunner = new CirceUsbLoopbackTestRunner<WirelessNetworkAddress>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.Connection.Send(new NotifyStatusOperation(deviceAddress, true,
                            DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad |
                                DeviceCapabilities.StartSensor | DeviceCapabilities.FinishSensor |
                                DeviceCapabilities.IntermediateSensor, DeviceRoles.StartTimer | DeviceRoles.FinishTimer,
                            25));
                    }
                };

                testRunner.RemoteSessionManager.DeviceTracker.DeviceRemoved +=
                    (s, e) => { testRunner.SignalSucceeded(e.Argument); };

                testRunner.RunTimeout = TimeSpan.FromSeconds(5);

                // Act
                bool succeeded = testRunner.StartWithKeepAliveLoopInBackground();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().Be(deviceAddress);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_notify_action_operation_is_received_it_must_raise_event_for_action()
        {
            // Arrange
            var deviceAddress = new WirelessNetworkAddress("AABBCC");
            const RawDeviceKeys inputKeys = RawDeviceKeys.Key1OrPlaySoundA;
            TimeSpan sensorTime = TimeSpan.FromMilliseconds(3456);

            using (var testRunner = new CirceUsbLoopbackTestRunner<DeviceAction>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.Connection.Send(new NotifyActionOperation(deviceAddress)
                        {
                            InputKeys = inputKeys,
                            SensorTime = sensorTime
                        });
                    }
                };

                testRunner.RemoteSessionManager.DeviceActionReceived +=
                    (s, e) => { testRunner.SignalSucceeded(e.Argument); };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
                testRunner.Result.DeviceAddress.Should().Be(deviceAddress);
                testRunner.Result.InputKeys.Should().Be(inputKeys);
                testRunner.Result.SensorTime.Should().Be(sensorTime);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_alert_is_requested_it_must_send_operation()
        {
            // Arrange
            var deviceAddress = new WirelessNetworkAddress("AABBCC");

            using (var testRunner = new CirceUsbLoopbackTestRunner<AlertOperation>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.RemoteSessionManager.AlertAsync(deviceAddress);
                    }
                };

                testRunner.OperationReceived += (s, e) =>
                {
                    var alertOperation = e.Operation as AlertOperation;
                    if (alertOperation != null)
                    {
                        testRunner.SignalSucceeded(alertOperation);
                    }
                };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
                testRunner.Result.DestinationAddress.Should().Be(deviceAddress);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_network_setup_is_requested_it_must_send_operation()
        {
            // Arrange
            var deviceAddress = new WirelessNetworkAddress("AABBCC");
            const bool setMembership = true;
            const DeviceRoles roles = DeviceRoles.Keypad;

            using (var testRunner = new CirceUsbLoopbackTestRunner<NetworkSetupOperation>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.RemoteSessionManager.NetworkSetupAsync(deviceAddress, setMembership, roles);
                    }
                };

                testRunner.OperationReceived += (s, e) =>
                {
                    var networkSetupOperation = e.Operation as NetworkSetupOperation;
                    if (networkSetupOperation != null)
                    {
                        testRunner.SignalSucceeded(networkSetupOperation);
                    }
                };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
                testRunner.Result.DestinationAddress.Should().Be(deviceAddress);
                testRunner.Result.SetMembership.Should().Be(setMembership);
                testRunner.Result.Roles.Should().Be(roles);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_clock_synchronization_is_requested_it_must_send_operation()
        {
            // Arrange
            using (var testRunner = new CirceUsbLoopbackTestRunner<SynchronizeClocksOperation>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.RemoteSessionManager.SynchronizeClocksAsync();
                    }
                };

                testRunner.OperationReceived += (s, e) =>
                {
                    var synchronizeClocksOperation = e.Operation as SynchronizeClocksOperation;
                    if (synchronizeClocksOperation != null)
                    {
                        testRunner.SignalSucceeded(synchronizeClocksOperation);
                    }
                };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_visualize_is_requested_it_must_send_operation()
        {
            var destinations = new[] { new WirelessNetworkAddress("AABBCC"), new WirelessNetworkAddress("DDEEFF") };
            VisualizeFieldSet visualizeFieldSet = new VisualizeFieldSetBuilder()
                .WithCurrentCompetitorNumber(123)
                .WithNextCompetitorNumber(125)
                .WithPrimaryTimerIsActive(true)
                .WithPrimaryTimerValue(25.Seconds().And(993.Milliseconds()))
                .WithCurrentFaultCount(5)
                .WithCurrentRefusalCount(10)
                .WithElimination(true)
                .WithPreviousPlacement(23)
                .Build();

            // Arrange
            using (var testRunner = new CirceUsbLoopbackTestRunner<VisualizeOperation>())
            {
                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        testRunner.RemoteSessionManager.VisualizeAsync(destinations, visualizeFieldSet);
                    }
                };

                testRunner.OperationReceived += (s, e) =>
                {
                    var visualizeOperation = e.Operation as VisualizeOperation;
                    if (visualizeOperation != null)
                    {
                        testRunner.SignalSucceeded(visualizeOperation);
                    }
                };

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
                testRunner.Result.DestinationAddresses.Should().Equal(destinations);
                testRunner.Result.CurrentCompetitorNumber.Should().Be(visualizeFieldSet.CurrentCompetitorNumber);
                testRunner.Result.NextCompetitorNumber.Should().Be(visualizeFieldSet.NextCompetitorNumber);
                testRunner.Result.StartTimer.Should().Be(visualizeFieldSet.StartPrimaryTimer);
                testRunner.Result.TimerValue.Should().Be(visualizeFieldSet.PrimaryTimerValue);
                testRunner.Result.FaultCount.Should().Be(visualizeFieldSet.CurrentFaultCount);
                testRunner.Result.RefusalCount.Should().Be(visualizeFieldSet.CurrentRefusalCount);
                testRunner.Result.Eliminated.Should().Be(visualizeFieldSet.CurrentIsEliminated);
                testRunner.Result.PreviousPlacement.Should().Be(visualizeFieldSet.PreviousPlacement);
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_connection_becomes_idle_it_must_send_logout_operation()
        {
            // Arrange
            using (var testRunner = new CirceUsbLoopbackTestRunner<LogoutOperation>())
            {
                testRunner.OperationReceived += (s, e) =>
                {
                    var logoutOperation = e.Operation as LogoutOperation;
                    if (logoutOperation != null)
                    {
                        testRunner.SignalSucceeded(logoutOperation);
                    }
                };

                testRunner.RunTimeout = TimeSpan.FromSeconds(3);

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
                testRunner.Result.Should().NotBeNull();
            }
        }

        [Test]
        [Category("UsbLoopback")]
        public void When_connection_becomes_idle_it_must_reconnect()
        {
            // Arrange
            var seenLogout = new FreshBoolean(false);

            using (var testRunner = new CirceUsbLoopbackTestRunner())
            {
                testRunner.OperationReceived += (s, e) =>
                {
                    var logoutOperation = e.Operation as LogoutOperation;
                    if (logoutOperation != null)
                    {
                        seenLogout.Value = true;
                    }
                };

                testRunner.RemoteSessionManager.ConnectionStateChanged += (s, e) =>
                {
                    if (e.State == ControllerConnectionState.Connected)
                    {
                        if (seenLogout.Value)
                        {
                            testRunner.SignalSucceeded();
                        }
                    }
                };

                testRunner.RunTimeout = TimeSpan.FromSeconds(5);

                // Act
                bool succeeded = testRunner.Start();

                // Assert
                succeeded.Should().Be(true, "<USB loopback cable must be connected and COM port must be correct.>");
            }
        }
    }
}