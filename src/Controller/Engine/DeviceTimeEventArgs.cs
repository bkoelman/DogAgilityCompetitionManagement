using System;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public class DeviceTimeEventArgs : DeviceEventArgs
    {
        public TimeSpan? SensorTime { get; }

        public DeviceTimeEventArgs(WirelessNetworkAddress source, TimeSpan? sensorTime)
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
