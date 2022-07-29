using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// Lists various logical network composition problems that prevent from starting a competition run.
/// </summary>
public sealed class NetworkComplianceMismatch
{
    public static readonly NetworkComplianceMismatch MissingKeypad = new("MissingKeypad", "Network must contain a device in role Keypad.");

    public static readonly NetworkComplianceMismatch MissingStartTimer = new("MissingStartTimer", "Network must contain a device in role StartTimer.");

    public static readonly NetworkComplianceMismatch MissingIntermediate1Timer =
        new("MissingIntermediate1Timer", "Network must contain a device in role Intermediate 1 Timer.");

    public static readonly NetworkComplianceMismatch MissingIntermediate2Timer =
        new("MissingIntermediate2Timer", "Network must contain a device in role Intermediate 2 Timer.");

    public static readonly NetworkComplianceMismatch MissingIntermediate3Timer =
        new("MissingIntermediate3Timer", "Network must contain a device in role Intermediate 3 Timer.");

    public static readonly NetworkComplianceMismatch MissingFinishTimer = new("MissingFinishTimer", "Network must contain a device in role FinishTimer.");

    public static readonly NetworkComplianceMismatch MissingDelayForStartFinishTimer = new("MissingDelayForStartFinishTimer",
        "Must specify minimum delay between passage of Start and Finish when using single Start/Finish gate.");

    public static readonly NetworkComplianceMismatch GateIsStartAndIntermediate =
        new("GateIsStartAndIntermediate", "Gate cannot be in both roles Start Timer and Intermediate Timer.");

    public static readonly NetworkComplianceMismatch GateIsFinishAndIntermediate =
        new("GateIsFinishAndIntermediate", "Gate cannot be in both roles Intermediate Timer and Finish Timer.");

    public static readonly NetworkComplianceMismatch GateInMultipleIntermediateTimerRoles =
        new("GateInMultipleIntermediateTimerRoles", "Gate cannot be in role Intermediate 1/2/3 Timer at the same time.");

    public static readonly NetworkComplianceMismatch IntermediateMissing32 = new("IntermediateMissing32",
        "Network cannot contain a device in role Intermediate 3 without another device in role Intermediate 2.");

    public static readonly NetworkComplianceMismatch IntermediateMissing21 = new("IntermediateMissing21",
        "Network cannot contain a device in role Intermediate 2 without another device in role Intermediate 1.");

    public string Name { get; }
    public string Message { get; }
    public WirelessNetworkAddress? DeviceAddress { get; }

    private NetworkComplianceMismatch(string name, string message, WirelessNetworkAddress? deviceAddress = null)
    {
        Guard.NotNullNorEmpty(name, nameof(name));
        Guard.NotNullNorEmpty(message, nameof(message));

        Name = name;
        Message = message;
        DeviceAddress = deviceAddress;
    }

    public static NetworkComplianceMismatch CreateForOfflineDevice(WirelessNetworkAddress deviceAddress)
    {
        Guard.NotNull(deviceAddress, nameof(deviceAddress));

        return new NetworkComplianceMismatch("DeviceOffline", $"Device {deviceAddress} has gone off-line.", deviceAddress);
    }

    [Pure]
    public override string ToString()
    {
        return DeviceAddress != null ? $"{Name} ({DeviceAddress})" : Name;
    }
}
