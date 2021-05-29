using System;
using System.Collections.Generic;
using System.Text;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using FluentAssertions;
using FluentAssertions.Extensions;
using JetBrains.Annotations;
using NUnit.Framework;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.CirceSpecs
{
    /// <summary>
    /// Tests for reading/writing all CIRCE operations to/from binary form.
    /// </summary>
    [TestFixture]
    public sealed class CirceOperationBinaryFormat
    {
        [Test]
        public void When_writing_login_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new LoginOperation();

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_login_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new LoginOperation();

            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_logout_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new LogoutOperation();

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_logout_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new LogoutOperation();
            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_alert_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new AlertOperation(new WirelessNetworkAddress("ABCDEF"));

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_alert_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new AlertOperation(new WirelessNetworkAddress("ABCDEF"));
            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_network_setup_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new NetworkSetupOperation(new WirelessNetworkAddress("ABCDEF"), true,
                DeviceRoles.StartTimer | DeviceRoles.FinishTimer | DeviceRoles.Keypad);

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('0'),
                ByteFor('4'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('7'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('7'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_network_setup_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('4'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('7'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('7'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new NetworkSetupOperation(new WirelessNetworkAddress("ABCDEF"), true,
                DeviceRoles.StartTimer | DeviceRoles.FinishTimer | DeviceRoles.Keypad);

            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_device_setup_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new DeviceSetupOperation(new WirelessNetworkAddress("ABCDEF"))
            {
                DestinationAddress = new WirelessNetworkAddress("654321"),
                Capabilities =
                    DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad | DeviceCapabilities.StartSensor |
                    DeviceCapabilities.FinishSensor | DeviceCapabilities.IntermediateSensor
            };

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('0'),
                ByteFor('5'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('6'),
                ByteFor('5'),
                ByteFor('4'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('6'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('9'),
                ByteFor(':'),
                ByteFor('3'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_device_setup_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('5'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('6'),
                ByteFor('5'),
                ByteFor('4'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('6'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('9'),
                ByteFor(':'),
                ByteFor('3'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new DeviceSetupOperation(new WirelessNetworkAddress("ABCDEF"))
            {
                DestinationAddress = new WirelessNetworkAddress("654321"),
                Capabilities =
                    DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad | DeviceCapabilities.StartSensor |
                    DeviceCapabilities.IntermediateSensor | DeviceCapabilities.FinishSensor
            };

            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_keep_alive_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new KeepAliveOperation(new Version(123, 456, 789), 1);

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('5'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('.'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('6'),
                ByteFor('.'),
                ByteFor('7'),
                ByteFor('8'),
                ByteFor('9'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_keep_alive_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('5'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('.'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('6'),
                ByteFor('.'),
                ByteFor('7'),
                ByteFor('8'),
                ByteFor('9'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new KeepAliveOperation(new Version(123, 456, 789), 1);
            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_notify_status_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new NotifyStatusOperation(new WirelessNetworkAddress("ABCDEF"), true,
                DeviceCapabilities.TimeSensor, DeviceRoles.IntermediateTimer3, 253)
            {
                BatteryStatus = 241,
                IsAligned = true,
                ClockSynchronization = ClockSynchronizationStatus.RequiresSync
            };

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('5'),
                ByteFor('2'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('6'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('9'),
                ByteFor(':'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('1'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('2'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('4'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('7'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_notify_status_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('5'),
                ByteFor('2'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('6'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('9'),
                ByteFor(':'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('1'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('2'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('4'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('7'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new NotifyStatusOperation(new WirelessNetworkAddress("ABCDEF"), true,
                DeviceCapabilities.TimeSensor, DeviceRoles.IntermediateTimer3, 253)
            {
                BatteryStatus = 241,
                IsAligned = true,
                ClockSynchronization = ClockSynchronizationStatus.RequiresSync
            };

            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_notify_action_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new NotifyActionOperation(new WirelessNetworkAddress("ABCDEF"))
            {
                InputKeys = RawDeviceKeys.Key2OrPassIntermediate | RawDeviceKeys.Key7,
                SensorTime = TimeSpan.FromMilliseconds(456.75)
            };

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('5'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('6'),
                ByteFor('6'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('7'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_notify_action_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('5'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('6'),
                ByteFor('6'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('7'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new NotifyActionOperation(new WirelessNetworkAddress("ABCDEF"))
            {
                InputKeys = RawDeviceKeys.Key2OrPassIntermediate | RawDeviceKeys.Key7,
                SensorTime = TimeSpan.FromMilliseconds(456.75)
            };

            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void When_writing_visualize_operation_it_must_be_correct()
        {
            // Arrange
            var operation = new VisualizeOperation(new[]
            {
                new WirelessNetworkAddress("ABCDEF"),
                new WirelessNetworkAddress("AABBCC")
            })
            {
                CurrentCompetitorNumber = 123,
                NextCompetitorNumber = 125,
                StartTimer = true,
                PrimaryTimerValue = 25.Seconds().And(943.Milliseconds()),
                SecondaryTimerValue = 16.Seconds().And(123.Milliseconds()),
                FaultCount = 5,
                RefusalCount = 10,
                Eliminated = true,
                PreviousPlacement = 23
            };

            // Act
            byte[] buffer = PacketWriter.Write(operation, false);

            // Assert
            buffer.Should().BeEquivalentTo(new byte[]
            {
                2,
                ByteFor('0'),
                ByteFor('7'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('8'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('9'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('1'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor('9'),
                ByteFor('4'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('8'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('6'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor(':'),
                ByteFor('5'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('3'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('0'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('C'),
                ByteFor('\t'),
                3
            });
        }

        [Test]
        public void When_reading_visualize_operation_it_must_be_correct()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('7'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('8'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('2'),
                ByteFor('9'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('0'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('1'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('5'),
                ByteFor('9'),
                ByteFor('4'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('2'),
                ByteFor(':'),
                ByteFor('5'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('3'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('0'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('5'),
                ByteFor(':'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('D'),
                ByteFor('E'),
                ByteFor('F'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('A'),
                ByteFor('A'),
                ByteFor('B'),
                ByteFor('B'),
                ByteFor('C'),
                ByteFor('C'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('8'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('6'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Operation operation = reader.Read(buffer);

            // Assert
            var expected = new VisualizeOperation(new[]
            {
                new WirelessNetworkAddress("ABCDEF"),
                new WirelessNetworkAddress("AABBCC")
            })
            {
                CurrentCompetitorNumber = 123,
                NextCompetitorNumber = 125,
                StartTimer = true,
                PrimaryTimerValue = 25.Seconds().And(943.Milliseconds()),
                SecondaryTimerValue = 16.Seconds().And(123.Milliseconds()),
                FaultCount = 5,
                RefusalCount = 10,
                Eliminated = true,
                PreviousPlacement = 23
            };

            operation.Should().BeEquivalentTo(expected, options => options.IncludingAllRuntimeProperties());
        }

        private static byte ByteFor(char ch)
        {
            return (byte)ch;
        }

        [NotNull]
        // ReSharper disable once UnusedMember.Local
        // Reason: Handy helper when writing new tests.
        private static string DumpBytes([NotNull] IEnumerable<byte> bytes)
        {
            var textBuilder = new StringBuilder();

            foreach (byte bt in bytes)
            {
                if (textBuilder.Length > 0)
                {
                    textBuilder.Append(", ");
                }

                if (bt == 9)
                {
                    textBuilder.Append("ByteFor('\\t')");
                }
                else if (bt < 32)
                {
                    textBuilder.Append($"{(int)bt}");
                }
                else
                {
                    textBuilder.Append($"ByteFor('{(char)bt}')");
                }
            }

            return textBuilder.ToString();
        }
    }
}
