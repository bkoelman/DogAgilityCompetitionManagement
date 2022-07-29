using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary />
public sealed class GatePassageEventArgs : DeviceTimeEventArgs
{
    public GatePassage GatePassage { get; }

    public GatePassageEventArgs(WirelessNetworkAddress source, TimeSpan? sensorTime, GatePassage gatePassage)
        : base(source, sensorTime)
    {
        GatePassage = gatePassage;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: GatePassage={GatePassage}, SensorTime={SensorTime}, Source={Source}";
    }
}
