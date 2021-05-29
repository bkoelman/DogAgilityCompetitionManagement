using System;
using System.Text;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Provides an immutable representation of the data in a CIRCE <see cref="NotifyActionOperation" />.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class DeviceAction : IEquatable<DeviceAction>
    {
        [NotNull]
        public WirelessNetworkAddress DeviceAddress { get; }

        [CanBeNull]
        public RawDeviceKeys? InputKeys { get; }

        [CanBeNull]
        public TimeSpan? SensorTime { get; }

        public DeviceAction([NotNull] WirelessNetworkAddress deviceAddress, [CanBeNull] RawDeviceKeys? inputKeys, [CanBeNull] TimeSpan? sensorTime)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            DeviceAddress = deviceAddress;
            InputKeys = inputKeys;
            SensorTime = sensorTime;
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            using (var formatter = new ObjectFormatter(textBuilder, this))
            {
                formatter.Append(() => DeviceAddress, () => DeviceAddress);
                formatter.Append(() => InputKeys, () => InputKeys);
                formatter.Append(() => SensorTime, () => SensorTime);
            }

            return textBuilder.ToString();
        }

        [NotNull]
        public static DeviceAction FromOperation([NotNull] NotifyActionOperation operation)
        {
            Guard.NotNull(operation, nameof(operation));

            // ReSharper disable once AssignNullToNotNullAttribute
            // Reason: Operation has been validated for required parameters when this code is reached.
            return new DeviceAction(operation.OriginatingAddress, operation.InputKeys, operation.SensorTime);
        }

        [NotNull]
        public Operation ToOperation()
        {
            return new NotifyActionOperation(DeviceAddress)
            {
                InputKeys = InputKeys,
                SensorTime = SensorTime
            };
        }

        public bool Equals(DeviceAction other)
        {
            return !ReferenceEquals(other, null) && other.DeviceAddress == DeviceAddress && other.InputKeys == InputKeys && other.SensorTime == SensorTime;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DeviceAction);
        }

        public override int GetHashCode()
        {
            return DeviceAddress.GetHashCode() ^ InputKeys.GetHashCode() ^ SensorTime.GetHashCode();
        }

        public static bool operator ==([CanBeNull] DeviceAction left, [CanBeNull] DeviceAction right)
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

        public static bool operator !=([CanBeNull] DeviceAction left, [CanBeNull] DeviceAction right)
        {
            return !(left == right);
        }
    }
}
