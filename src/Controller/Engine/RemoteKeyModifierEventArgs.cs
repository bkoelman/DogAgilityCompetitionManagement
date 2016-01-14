using System;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class RemoteKeyModifierEventArgs : DeviceTimeEventArgs
    {
        public RemoteKeyModifier Modifier { get; }

        public RemoteKeyModifierEventArgs([NotNull] WirelessNetworkAddress source, RemoteKeyModifier modifier,
            [CanBeNull] TimeSpan? sensorTime)
            : base(source, sensorTime)
        {
            Modifier = modifier;
        }

        public override string ToString()
            => $"{GetType().Name}: Modifier={Modifier}, SensorTime={SensorTime}, Source={Source}";
    }
}