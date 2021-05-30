using System;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class RemoteKeyModifierEventArgs : DeviceTimeEventArgs
    {
        public RemoteKeyModifier Modifier { get; }

        public RemoteKeyModifierEventArgs(WirelessNetworkAddress source, RemoteKeyModifier modifier, TimeSpan? sensorTime)
            : base(source, sensorTime)
        {
            Modifier = modifier;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Modifier={Modifier}, SensorTime={SensorTime}, Source={Source}";
        }
    }
}
