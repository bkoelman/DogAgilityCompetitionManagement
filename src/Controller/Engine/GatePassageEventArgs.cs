using System;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class GatePassageEventArgs : DeviceTimeEventArgs
    {
        public GatePassage GatePassage { get; }

        public GatePassageEventArgs([NotNull] WirelessNetworkAddress source, [CanBeNull] TimeSpan? sensorTime,
            GatePassage gatePassage)
            : base(source, sensorTime)
        {
            GatePassage = gatePassage;
        }

        public override string ToString()
            => $"{GetType().Name}: GatePassage={GatePassage}, SensorTime={SensorTime}, Source={Source}";
    }
}