using System;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public class DeviceTimeEventArgs : DeviceEventArgs
    {
        [CanBeNull]
        public TimeSpan? SensorTime { get; }

        public DeviceTimeEventArgs([NotNull] WirelessNetworkAddress source, [CanBeNull] TimeSpan? sensorTime)
            : base(source)
        {
            SensorTime = sensorTime;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: SensorTime={SensorTime}, Source={Source}";
        }
    }
}
