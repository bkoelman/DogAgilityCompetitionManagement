using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Facilities
{
    internal sealed class TrackerEventListener : IDisposable
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