using System;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary />
    // ReSharper disable once UnusedTypeParameter
    // Reason: Generic argument is used to enable creation of test-specific extensions methods.
    public sealed class EventArgsWithName<TTestScope>
    {
        [NotNull]
        public string Name { get; }

        [NotNull]
        public EventArgs EventArgs { get; }

        public EventArgsWithName([NotNull] string name, [NotNull] EventArgs eventArgs)
        {
            Guard.NotNullNorEmpty(name, nameof(name));
            Guard.NotNull(eventArgs, nameof(eventArgs));

            Name = name;
            EventArgs = eventArgs;
        }

        public override string ToString() => $"{Name}: {EventArgs}";
    }
}