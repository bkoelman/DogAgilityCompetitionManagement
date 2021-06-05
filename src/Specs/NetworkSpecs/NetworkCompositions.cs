using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.NetworkSpecs
{
    /// <summary>
    /// Tests for composing logical networks.
    /// </summary>
    [TestFixture]
    public sealed class NetworkCompositions
    {
        private static readonly WirelessNetworkAddress DeviceAddress = new("ABCDEF");

        [Test]
        public void When_setting_a_negative_delay_it_must_fail()
        {
            // Arrange
            TimeSpan negativeTime = -1.Minutes();
            NetworkComposition composition = new NetworkCompositionBuilder().Build();
            CompetitionClassRequirements requirements = composition.Requirements;

            // Act
            Action action = () => requirements.ChangeStartFinishMinDelayForSingleSensor(negativeTime);

            // Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void When_delay_is_specified_and_start_and_finish_is_same_sensor_it_must_succeed()
        {
            // Arrange
            TimeSpan minDelay = 1.Minutes();

            NetworkComposition composition = new NetworkCompositionBuilder()
                .WithStartFinishMinDelayForSingleSensor(minDelay)
                .Build();

            // Act
            composition = composition.ChangeRolesFor(DeviceAddress, DeviceCapabilities.TimeSensor, DeviceRoles.StartTimer | DeviceRoles.FinishTimer);

            // Assert
            composition.IsInRoleStartTimer(DeviceAddress).Should().BeTrue();
            composition.IsInRoleFinishTimer(DeviceAddress).Should().BeTrue();
            composition.Requirements.StartFinishMinDelayForSingleSensor.Should().Be(minDelay);
            composition.IsStartFinishGate(DeviceAddress).Should().BeTrue();
        }

        [Test]
        public void When_no_delay_is_specified_and_start_and_finish_is_same_sensor_it_must_fail()
        {
            // Arrange
            NetworkComposition composition = new NetworkCompositionBuilder()
                .WithoutStartFinishMinDelayForSingleSensor()
                .WithDeviceInRoles(DeviceAddress, DeviceCapabilities.TimeSensor,
                    DeviceRoles.StartTimer | DeviceRoles.FinishTimer)
                .Build();

            // Act
            IList<NetworkComplianceMismatch> mismatches = composition.AssertComplianceWithRequirements();

            // Assert
            mismatches.Should().HaveCount(1);
            mismatches[0].Name.Should().Be("MissingDelayForStartFinishTimer");
        }

        [Test]
        public void When_adding_a_device_in_role_start_timer_it_must_be_stored()
        {
            // Arrange
            NetworkComposition composition = new NetworkCompositionBuilder().Build();

            // Act
            composition = composition.ChangeRolesFor(DeviceAddress, DeviceCapabilities.StartSensor, DeviceRoles.StartTimer);

            // Assert
            composition.IsInRoleStartTimer(DeviceAddress).Should().BeTrue();
        }

        [Test]
        public void When_adding_devices_in_role_intermediate_timer_they_must_be_stored()
        {
            // Arrange
            var timer1 = new WirelessNetworkAddress("AAAAAA");
            var timer2 = new WirelessNetworkAddress("BBBBBB");
            var timer3 = new WirelessNetworkAddress("CCCCCC");
            NetworkComposition composition = new NetworkCompositionBuilder().Build();

            // Act
            composition = composition
                .ChangeRolesFor(timer1, DeviceCapabilities.IntermediateSensor, DeviceRoles.IntermediateTimer1)
                .ChangeRolesFor(timer2, DeviceCapabilities.IntermediateSensor, DeviceRoles.IntermediateTimer2)
                .ChangeRolesFor(timer3, DeviceCapabilities.IntermediateSensor, DeviceRoles.IntermediateTimer3);

            // Assert
            composition.IsInRoleIntermediateTimer1(timer1).Should().BeTrue();
            composition.IsInRoleIntermediateTimer2(timer2).Should().BeTrue();
            composition.IsInRoleIntermediateTimer3(timer3).Should().BeTrue();
        }

        [Test]
        public void When_adding_a_device_in_role_finish_timer_it_must_be_stored()
        {
            // Arrange
            NetworkComposition composition = new NetworkCompositionBuilder().Build();

            // Act
            composition = composition.ChangeRolesFor(DeviceAddress, DeviceCapabilities.FinishSensor, DeviceRoles.FinishTimer);

            // Assert
            composition.IsInRoleFinishTimer(DeviceAddress).Should().BeTrue();
        }

        [Test]
        public void When_adding_a_device_in_role_display_it_must_be_stored()
        {
            // Arrange
            NetworkComposition composition = new NetworkCompositionBuilder().Build();

            // Act
            composition = composition.ChangeRolesFor(DeviceAddress, DeviceCapabilities.Display, DeviceRoles.Display);

            // Assert
            composition.IsInRoleDisplay(DeviceAddress).Should().BeTrue();
        }

        [Test]
        public void When_adding_an_empty_device_it_must_fail()
        {
            // Arrange
            const WirelessNetworkAddress? missingDevice = null;
            NetworkComposition composition = new NetworkCompositionBuilder().Build();

            // Act
            // Justification for nullable suppression: The goal of this test is to cause failure when no device is specified.
            Action action = () => composition = composition.ChangeRolesFor(missingDevice!, DeviceCapabilities.FinishSensor, DeviceRoles.FinishTimer);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void When_adding_a_device_with_an_empty_role_it_must_not_be_in_any_known_roles()
        {
            // Arrange
            const DeviceRoles emptyRole = DeviceRoles.None;
            NetworkComposition composition = new NetworkCompositionBuilder().Build();

            // Act
            composition = composition.ChangeRolesFor(DeviceAddress, DeviceCapabilities.FinishSensor, emptyRole);

            // Assert
            composition.IsInRoleStartTimer(DeviceAddress).Should().BeFalse();
            composition.IsInRoleIntermediateTimer1(DeviceAddress).Should().BeFalse();
            composition.IsInRoleIntermediateTimer2(DeviceAddress).Should().BeFalse();
            composition.IsInRoleIntermediateTimer3(DeviceAddress).Should().BeFalse();
            composition.IsInRoleFinishTimer(DeviceAddress).Should().BeFalse();
            composition.IsStartFinishGate(DeviceAddress).Should().BeFalse();
        }
    }
}
