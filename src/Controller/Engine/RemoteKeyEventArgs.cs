using System;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class RemoteKeyEventArgs : DeviceTimeEventArgs
    {
        public RemoteKey Key { get; }

        public RemoteKeyEventArgs(WirelessNetworkAddress source, RemoteKey key, TimeSpan? sensorTime)
            : base(source, sensorTime)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Key={Key}, SensorTime={SensorTime}, Source={Source}";
        }
    }
}
