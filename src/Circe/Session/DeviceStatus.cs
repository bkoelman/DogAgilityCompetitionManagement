using System;
using System.Text;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Provides an immutable representation of the data in a CIRCE <see cref="NotifyStatusOperation" />.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class DeviceStatus : IEquatable<DeviceStatus>
    {
        private const DeviceRoles IntermediateTimers = DeviceRoles.IntermediateTimer1 | DeviceRoles.IntermediateTimer2 | DeviceRoles.IntermediateTimer3;

        public WirelessNetworkAddress DeviceAddress { get; }
        public bool IsInNetwork { get; }
        public DeviceCapabilities Capabilities { get; }
        public DeviceRoles Roles { get; }
        public int SignalStrength { get; }
        public int? BatteryStatus { get; }
        public bool? IsAligned { get; }
        public ClockSynchronizationStatus? ClockSynchronization { get; }
        public bool? HasVersionMismatch { get; }

        public string DeviceType
        {
            get
            {
                if ((Capabilities & DeviceCapabilities.ControlKeypad) != 0)
                {
                    return (Capabilities & DeviceCapabilities.NumericKeypad) != 0 ? "Competition remote" : "Training remote";
                }

                switch (Capabilities)
                {
                    case DeviceCapabilities.TimeSensor:
                        return "Gate";
                    case DeviceCapabilities.Display:
                        return "Display";
                    default:
                        return "Unknown";
                }
            }
        }

        public DeviceStatus(WirelessNetworkAddress deviceAddress, bool isInNetwork, DeviceCapabilities capabilities, DeviceRoles roles, int signalStrength,
            int? batteryStatus, bool? isAligned, ClockSynchronizationStatus? clockSynchronization, bool? hasVersionMismatch)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            DeviceAddress = deviceAddress;
            IsInNetwork = isInNetwork;
            Capabilities = capabilities;
            Roles = ReduceRolesToComplyWith(capabilities, roles);
            SignalStrength = signalStrength;
            BatteryStatus = batteryStatus;
            IsAligned = isAligned;
            ClockSynchronization = clockSynchronization;
            HasVersionMismatch = hasVersionMismatch;
        }

        private static DeviceRoles ReduceRolesToComplyWith(DeviceCapabilities capabilities, DeviceRoles roles)
        {
            var allowed = DeviceRoles.None;

            if ((capabilities & DeviceCapabilities.ControlKeypad) != 0)
            {
                allowed |= DeviceRoles.Keypad;
            }

            if ((capabilities & DeviceCapabilities.StartSensor) != 0 || (capabilities & DeviceCapabilities.TimeSensor) != 0)
            {
                allowed |= DeviceRoles.StartTimer;
            }

            if ((capabilities & DeviceCapabilities.IntermediateSensor) != 0 || (capabilities & DeviceCapabilities.TimeSensor) != 0)
            {
                allowed |= IntermediateTimers;
            }

            if ((capabilities & DeviceCapabilities.FinishSensor) != 0 || (capabilities & DeviceCapabilities.TimeSensor) != 0)
            {
                allowed |= DeviceRoles.FinishTimer;
            }

            if ((capabilities & DeviceCapabilities.Display) != 0)
            {
                allowed |= DeviceRoles.Display;
            }

            return roles & allowed;
        }

        public static DeviceStatus FromOperation(NotifyStatusOperation operation)
        {
            Guard.NotNull(operation, nameof(operation));

            // Justification for nullable suppression: Operation has been validated for required parameters when this code is reached.
            return new DeviceStatus(operation.OriginatingAddress!, operation.GetMembership!.Value, operation.Capabilities!.Value, operation.Roles!.Value,
                operation.SignalStrength!.Value, operation.BatteryStatus, operation.IsAligned, operation.ClockSynchronization, operation.HasVersionMismatch);
        }

        public NotifyStatusOperation ToOperation()
        {
            return new(DeviceAddress, IsInNetwork, Capabilities, Roles, SignalStrength)
            {
                BatteryStatus = BatteryStatus,
                IsAligned = IsAligned,
                ClockSynchronization = ClockSynchronization,
                HasVersionMismatch = HasVersionMismatch
            };
        }

        public DeviceStatus ChangeIsInNetwork(bool isInNetwork)
        {
            return new(DeviceAddress, isInNetwork, Capabilities, Roles, SignalStrength, BatteryStatus, IsAligned, ClockSynchronization, HasVersionMismatch);
        }

        public DeviceStatus ChangeRoles(DeviceRoles roles)
        {
            return new(DeviceAddress, IsInNetwork, Capabilities, roles, SignalStrength, BatteryStatus, IsAligned, ClockSynchronization, HasVersionMismatch);
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            using (var formatter = new ObjectFormatter(textBuilder, this))
            {
                formatter.Append(DeviceAddress, nameof(DeviceAddress));
                formatter.Append(IsInNetwork, nameof(IsInNetwork));
                formatter.Append(Capabilities, nameof(Capabilities));
                formatter.Append(Roles, nameof(Roles));
                formatter.Append(SignalStrength, nameof(SignalStrength));
                formatter.Append(BatteryStatus, nameof(BatteryStatus));
                formatter.Append(IsAligned, nameof(IsAligned));
                formatter.Append(ClockSynchronization, nameof(ClockSynchronization));
                formatter.Append(HasVersionMismatch, nameof(HasVersionMismatch));
            }

            return textBuilder.ToString();
        }

        public bool Equals(DeviceStatus? other)
        {
            return !ReferenceEquals(other, null) && other.DeviceAddress == DeviceAddress && other.IsInNetwork == IsInNetwork &&
                other.Capabilities == Capabilities && other.Roles == Roles && other.SignalStrength == SignalStrength && other.BatteryStatus == BatteryStatus &&
                other.IsAligned == IsAligned && other.ClockSynchronization == ClockSynchronization && other.HasVersionMismatch == HasVersionMismatch;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DeviceStatus);
        }

        public override int GetHashCode()
        {
            return DeviceAddress.GetHashCode() ^ IsInNetwork.GetHashCode() ^ Capabilities.GetHashCode() ^ Roles.GetHashCode() ^ SignalStrength.GetHashCode() ^
                BatteryStatus.GetHashCode() ^ IsAligned.GetHashCode() ^ ClockSynchronization.GetHashCode() ^ HasVersionMismatch.GetHashCode();
        }

        public static bool operator ==(DeviceStatus? left, DeviceStatus? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(DeviceStatus? left, DeviceStatus? right)
        {
            return !(left == right);
        }
    }
}
