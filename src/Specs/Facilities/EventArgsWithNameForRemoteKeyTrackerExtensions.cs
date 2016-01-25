using System;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine;
using FluentAssertions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Facilities
{
    internal static class EventArgsWithNameForRemoteKeyTrackerExtensions
    {
        public static void ShouldBeModifierKeyDownFor(
            [NotNull] this EventArgsWithName<RemoteKeyTracker> eventArgsWithName,
            [NotNull] WirelessNetworkAddress source, RemoteKeyModifier modifier)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));
            Guard.NotNull(source, nameof(source));

            eventArgsWithName.Name.Should().Be("ModifierKeyDown");
            eventArgsWithName.EventArgs.Should().BeOfType<RemoteKeyModifierEventArgs>();

            var remoteKeyModifierEventArgs = (RemoteKeyModifierEventArgs) eventArgsWithName.EventArgs;
            remoteKeyModifierEventArgs.Source.Should().Be(source);
            remoteKeyModifierEventArgs.Modifier.Should().Be(modifier);
        }

        public static void ShouldBeKeyDownFor([NotNull] this EventArgsWithName<RemoteKeyTracker> eventArgsWithName,
            [NotNull] WirelessNetworkAddress source, RemoteKey key)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));
            Guard.NotNull(source, nameof(source));

            eventArgsWithName.Name.Should().Be("KeyDown");
            eventArgsWithName.EventArgs.Should().BeOfType<RemoteKeyEventArgs>();

            var remoteKeyModifierEventArgs = (RemoteKeyEventArgs) eventArgsWithName.EventArgs;
            remoteKeyModifierEventArgs.Source.Should().Be(source);
            remoteKeyModifierEventArgs.Key.Should().Be(key);
        }

        public static void ShouldBeKeyUpFor([NotNull] this EventArgsWithName<RemoteKeyTracker> eventArgsWithName,
            [NotNull] WirelessNetworkAddress source, RemoteKey key)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));
            Guard.NotNull(source, nameof(source));

            eventArgsWithName.Name.Should().Be("KeyUp");
            eventArgsWithName.EventArgs.Should().BeOfType<RemoteKeyEventArgs>();

            var remoteKeyModifierEventArgs = (RemoteKeyEventArgs) eventArgsWithName.EventArgs;
            remoteKeyModifierEventArgs.Source.Should().Be(source);
            remoteKeyModifierEventArgs.Key.Should().Be(key);
        }

        public static void ShouldBeModifierKeyUpFor(
            [NotNull] this EventArgsWithName<RemoteKeyTracker> eventArgsWithName,
            [NotNull] WirelessNetworkAddress source, RemoteKeyModifier modifier)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));
            Guard.NotNull(source, nameof(source));

            eventArgsWithName.Name.Should().Be("ModifierKeyUp");
            eventArgsWithName.EventArgs.Should().BeOfType<RemoteKeyModifierEventArgs>();

            var remoteKeyModifierEventArgs = (RemoteKeyModifierEventArgs) eventArgsWithName.EventArgs;
            remoteKeyModifierEventArgs.Source.Should().Be(source);
            remoteKeyModifierEventArgs.Modifier.Should().Be(modifier);
        }

        public static void ShouldBeMissingKeyFor([NotNull] this EventArgsWithName<RemoteKeyTracker> eventArgsWithName,
            [NotNull] WirelessNetworkAddress source, [CanBeNull] TimeSpan? sensorTime)
        {
            Guard.NotNull(eventArgsWithName, nameof(eventArgsWithName));
            Guard.NotNull(source, nameof(source));

            eventArgsWithName.Name.Should().Be("MissingKey");
            eventArgsWithName.EventArgs.Should().BeOfType<DeviceTimeEventArgs>();

            var deviceTimeEventArgs = (DeviceTimeEventArgs) eventArgsWithName.EventArgs;
            deviceTimeEventArgs.Source.Should().Be(source);
            deviceTimeEventArgs.SensorTime.Should().Be(sensorTime);
        }
    }
}