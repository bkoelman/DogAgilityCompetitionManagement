using System;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class RemoteKeyEventArgs : DeviceTimeEventArgs
    {
        public RemoteKey Key { get; }

        public RemoteKeyEventArgs([NotNull] WirelessNetworkAddress source, RemoteKey key, [CanBeNull] TimeSpan? sensorTime)
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
