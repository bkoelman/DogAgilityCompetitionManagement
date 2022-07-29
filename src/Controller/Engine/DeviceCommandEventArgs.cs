using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary />
public sealed class DeviceCommandEventArgs : DeviceEventArgs
{
    public DeviceCommand Command { get; }

    public DeviceCommandEventArgs(WirelessNetworkAddress source, DeviceCommand command)
        : base(source)
    {
        Command = command;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: Command={Command}, Source={Source}";
    }
}
