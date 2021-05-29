using System;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class UnknownDeviceActionEventArgs : DeviceTimeEventArgs
    {
        [CanBeNull]
        public RemoteKey? Key { get; }

        public UnknownDeviceActionEventArgs([NotNull] WirelessNetworkAddress source, [CanBeNull] TimeSpan? sensorTime, [CanBeNull] RemoteKey? key)
            : base(source, sensorTime)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: SensorTime={SensorTime}, Source={Source}";
        }
    }
}
