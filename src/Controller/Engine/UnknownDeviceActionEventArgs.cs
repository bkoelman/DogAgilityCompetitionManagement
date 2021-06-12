using System;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class UnknownDeviceActionEventArgs : DeviceTimeEventArgs
    {
        public RemoteKey? Key { get; }

        public UnknownDeviceActionEventArgs(WirelessNetworkAddress source, TimeSpan? sensorTime, RemoteKey? key)
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
