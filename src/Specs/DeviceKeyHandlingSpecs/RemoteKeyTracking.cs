using System;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Specs.Facilities;
using FluentAssertions;
using FluentAssertions.Extensions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace DogAgilityCompetition.Specs.DeviceKeyHandlingSpecs
{
    /// <summary />
    [TestFixture]
    public sealed class RemoteKeyTracking
    {
        [NotNull]
        private static readonly WirelessNetworkAddress Source = new("ABCDEF");

        [CanBeNull]
        private static readonly TimeSpan? NullTime = null;

        [Test]
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

        [Test]
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
    }
}
