using System;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine;
using FluentAssertions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="NetworkComposition" /> objects in tests.
    /// </summary>
    public sealed class NetworkCompositionBuilder : ITestDataBuilder<NetworkComposition>
    {
        public NetworkComposition Build()
        {
            return composition;
        }

        [NotNull]
        private NetworkComposition composition =
            NetworkComposition.Empty.ChangeRequirements(
                CompetitionClassRequirements.Default.ChangeStartFinishMinDelayForSingleSensor(5.Minutes()))
                .ChangeRolesFor(new WirelessNetworkAddress("AAAAAA"),
                    DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad, DeviceRoles.Keypad)
                .ChangeRolesFor(new WirelessNetworkAddress("BBBBBB"), DeviceCapabilities.TimeSensor,
                    DeviceRoles.StartTimer)
                .ChangeRolesFor(new WirelessNetworkAddress("CCCCCC"), DeviceCapabilities.TimeSensor,
                    DeviceRoles.FinishTimer);

        [NotNull]
        public NetworkCompositionBuilder WithStartFinishMinDelayForSingleSensor(
            TimeSpan startFinishMinDelayForSingleSensor)
        {
            composition =
                composition.ChangeRequirements(
                    composition.Requirements.ChangeStartFinishMinDelayForSingleSensor(startFinishMinDelayForSingleSensor));
            return this;
        }

        [NotNull]
        public NetworkCompositionBuilder WithoutStartFinishMinDelayForSingleSensor()
        {
            composition =
                composition.ChangeRequirements(
                    composition.Requirements.ChangeStartFinishMinDelayForSingleSensor(TimeSpan.Zero));
            return this;
        }

        [NotNull]
        public NetworkCompositionBuilder WithDeviceInRoles([NotNull] WirelessNetworkAddress deviceAddress,
            DeviceCapabilities capabilities, DeviceRoles roles)
        {
            composition = composition.ChangeRolesFor(deviceAddress, capabilities, roles);
            return this;
        }
    }
}