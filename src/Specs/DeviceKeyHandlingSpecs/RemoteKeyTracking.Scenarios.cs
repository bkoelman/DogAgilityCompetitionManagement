using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Controller.Engine;
using FluentAssertions;
using NUnit.Framework;

namespace DogAgilityCompetition.Specs.DeviceKeyHandlingSpecs
{
    /// <summary>
    /// Tests how the up/down state of keys on wireless remote controls is tracked.
    /// </summary>
    public sealed partial class RemoteKeyTracking
    {
        // Note: For description of these scenarios, see file: "Key change detection.xlsx"

        [Test]
        public void Scenario1()
        {
            var tracker = new RemoteKeyTracker();
            var rawKeysDown = RawDeviceKeys.None;

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }
        }

        [Test]
        public void Scenario2()
        {
            var tracker = new RemoteKeyTracker();
            var rawKeysDown = RawDeviceKeys.None;

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key3OrToggleElimination);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key3OrToggleElimination);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key3OrToggleElimination);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key3OrToggleElimination);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }
        }

        [Test]
        public void Scenario3()
        {
            var tracker = new RemoteKeyTracker();
            var rawKeysDown = RawDeviceKeys.None;

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterNextCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterNextCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterNextCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterNextCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key3OrToggleElimination);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key3OrToggleElimination);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key3OrToggleElimination);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key3OrToggleElimination);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }
        }

        [Test]
        public void Scenario4()
        {
            var tracker = new RemoteKeyTracker();
            var rawKeysDown = RawDeviceKeys.None;

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterNextCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterNextCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key3OrToggleElimination);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key3OrToggleElimination);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key3OrToggleElimination);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key3OrToggleElimination);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterNextCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterNextCompetitor);
            }
        }

        [Test]
        public void Scenario5()
        {
            var tracker = new RemoteKeyTracker();
            var rawKeysDown = RawDeviceKeys.None;

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterNextCompetitor);
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(3);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterNextCompetitor);
                listener.EventsCollected[1].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
                listener.EventsCollected[2].ShouldBeKeyDownFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key1OrPlaySoundA);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key3OrToggleElimination);
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key3OrToggleElimination);
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterNextCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(2);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterNextCompetitor);
                listener.EventsCollected[1].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }
        }

        [Test]
        public void Scenario6()
        {
            var tracker = new RemoteKeyTracker();
            var rawKeysDown = RawDeviceKeys.None;

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key1OrPlaySoundA);
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key1OrPlaySoundA);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(2);
                listener.EventsCollected[0].ShouldBeKeyDownFor(Source, RemoteKey.Key2OrPassIntermediate);
                listener.EventsCollected[1].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.Key2OrPassIntermediate);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeKeyUpFor(Source, RemoteKey.Key2OrPassIntermediate);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Push(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyDownFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }

            using (var listener = new TrackerEventListener(tracker))
            {
                rawKeysDown = rawKeysDown.Release(RawDeviceKeys.EnterCurrentCompetitor);
                tracker.ProcessDeviceAction(new DeviceAction(Source, rawKeysDown, NullTime));

                listener.EventsCollected.Should().HaveCount(1);
                listener.EventsCollected[0].ShouldBeModifierKeyUpFor(Source, RemoteKeyModifier.EnterCurrentCompetitor);
            }
        }
    }
}