using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine;
using FluentAssertions.Extensions;

namespace DogAgilityCompetition.Specs.Builders;

/// <summary>
/// Enables composition of <see cref="NetworkComposition" /> objects in tests.
/// </summary>
public sealed class NetworkCompositionBuilder : ITestDataBuilder<NetworkComposition>
{
    private NetworkComposition composition = NetworkComposition.Empty
        .ChangeRequirements(CompetitionClassRequirements.Default.ChangeStartFinishMinDelayForSingleSensor(5.Minutes()))
        .ChangeRolesFor(new WirelessNetworkAddress("AAAAAA"), DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad, DeviceRoles.Keypad)
        .ChangeRolesFor(new WirelessNetworkAddress("BBBBBB"), DeviceCapabilities.TimeSensor, DeviceRoles.StartTimer)
        .ChangeRolesFor(new WirelessNetworkAddress("CCCCCC"), DeviceCapabilities.TimeSensor, DeviceRoles.FinishTimer);

    public NetworkComposition Build()
    {
        return composition;
    }

    public NetworkCompositionBuilder WithStartFinishMinDelayForSingleSensor(TimeSpan startFinishMinDelayForSingleSensor)
    {
        composition = composition.ChangeRequirements(composition.Requirements.ChangeStartFinishMinDelayForSingleSensor(startFinishMinDelayForSingleSensor));
        return this;
    }

    public NetworkCompositionBuilder WithoutStartFinishMinDelayForSingleSensor()
    {
        composition = composition.ChangeRequirements(composition.Requirements.ChangeStartFinishMinDelayForSingleSensor(TimeSpan.Zero));
        return this;
    }

    public NetworkCompositionBuilder WithDeviceInRoles(WirelessNetworkAddress deviceAddress, DeviceCapabilities capabilities, DeviceRoles roles)
    {
        composition = composition.ChangeRolesFor(deviceAddress, capabilities, roles);
        return this;
    }
}
