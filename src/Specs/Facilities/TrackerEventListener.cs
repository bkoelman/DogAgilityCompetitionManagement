using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;

namespace DogAgilityCompetition.Specs.Facilities
{
    internal sealed class TrackerEventListener : IDisposable
    {
        private RemoteKeyTracker? source;

        public List<EventArgsWithName<RemoteKeyTracker>> EventsCollected { get; } = new();

        public TrackerEventListener(RemoteKeyTracker source)
        {
            Guard.NotNull(source, nameof(source));

            this.source = source;

            AttachTrackerHandlers();
        }

        private void SourceOnModifierKeyDown(object? sender, RemoteKeyModifierEventArgs e)
        {
            EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("ModifierKeyDown", e));
        }

        private void SourceOnKeyDown(object? sender, RemoteKeyEventArgs e)
        {
            EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("KeyDown", e));
        }

        private void SourceOnKeyUp(object? sender, RemoteKeyEventArgs e)
        {
            EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("KeyUp", e));
        }

        private void SourceOnModifierKeyUp(object? sender, RemoteKeyModifierEventArgs e)
        {
            EventsCollected.Add(new EventArgsWithName<RemoteKeyTracker>("ModifierKeyUp", e));
        }

        private void SourceOnMissingKey(object? sender, DeviceTimeEventArgs e)
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
