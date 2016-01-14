using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Specs.Facilities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogAgilityCompetition.Specs.DeviceKeyHandlingSpecs
{
    /// <summary />
    [TestClass]
    public sealed partial class RemoteKeyTracking
    {
        [NotNull]
        private static readonly WirelessNetworkAddress Source = new WirelessNetworkAddress("ABCDEF");

        [CanBeNull]
        private static readonly TimeSpan? NullTime = null;

        [TestMethod]
        public void When_no_raw_keys_are_included_it_must_raise_event_without_keys()
        {
            // Arrange
            TimeSpan sensorTime = 5.Minutes();
            var deviceAction = new DeviceAction(Source, null, sensorTime);
            var tracker = new RemoteKeyTracker();

            // Act
            using (var listener = new TrackerEventListener(tracker))
            {
                tracker.ProcessDeviceAction(deviceAction);

                // Assert
                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeMissingKeyFor(Source, sensorTime);
            }
        }

        [TestMethod]
        public void When_no_raw_keys_and_no_time_are_included_it_must_raise_event_without_keys_and_time()
        {
            // Arrange
            var deviceAction = new DeviceAction(Source, null, NullTime);
            var tracker = new RemoteKeyTracker();

            // Act
            using (var listener = new TrackerEventListener(tracker))
            {
                tracker.ProcessDeviceAction(deviceAction);

                // Assert
                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeMissingKeyFor(Source, NullTime);
            }
        }

        private sealed class TrackerEventListener : IDisposable
        {
            [NotNull]
            [ItemNotNull]
            public List<EventArgsWithName<RemoteKeyTracker>> EventsCollected { get; } =
                new List<EventArgsWithName<RemoteKeyTracker>>();

            [CanBeNull]
            private RemoteKeyTracker source;

            public TrackerEventListener([NotNull] RemoteKeyTracker source)
            {
                Guard.NotNull(source, nameof(source));

                this.source = source;

                AttachTrackerHandlers();
            }

            private void SourceOnModifierKeyDown([CanBeNull] object sender, [NotNull] RemoteKeyModifierEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("ModifierKeyDown", e));
            }

            private void SourceOnKeyDown([CanBeNull] object sender, [NotNull] RemoteKeyEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("KeyDown", e));
            }

            private void SourceOnKeyUp([CanBeNull] object sender, [NotNull] RemoteKeyEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("KeyUp", e));
            }

            private void SourceOnModifierKeyUp([CanBeNull] object sender, [NotNull] RemoteKeyModifierEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("ModifierKeyUp", e));
            }

            private void SourceOnMissingKey([CanBeNull] object sender, [NotNull] DeviceTimeEventArgs e)
            {
                EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("MissingKey", e));
            }

            public void Dispose()
            {
                if (source != null)
                {
                    DetachTrackerHandlers();

                    source = null;
                }
            }

            private void AttachTrackerHandlers()
            {
                if (source != null)
                {
                    source.ModifierKeyDown += SourceOnModifierKeyDown;
                    source.KeyDown += SourceOnKeyDown;
                    source.KeyUp += SourceOnKeyUp;
                    source.ModifierKeyUp += SourceOnModifierKeyUp;
                    source.MissingKey += SourceOnMissingKey;
                }
            }

            private void DetachTrackerHandlers()
            {
                if (source != null)
                {
                    source.ModifierKeyDown -= SourceOnModifierKeyDown;
                    source.KeyDown -= SourceOnKeyDown;
                    source.KeyUp -= SourceOnKeyUp;
                    source.ModifierKeyUp -= SourceOnModifierKeyUp;
                    source.MissingKey -= SourceOnMissingKey;
                }
            }
        }
    }

    public static class RawDeviceKeyExtensions
    {
        public static RawDeviceKeys Push(this RawDeviceKeys source, RawDeviceKeys key)
        {
            return source | key;
        }

        public static RawDeviceKeys Release(this RawDeviceKeys source, RawDeviceKeys key)
        {
            return source & ~key;
        }
    }

    public static class EventArgsWithNameForRemoteKeyTrackerExtensions
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