using System;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary />
    // ReSharper disable once UnusedTypeParameter
    // Justification: Generic argument is used to enable creation of test-specific extensions methods.
    public sealed class EventArgsWithName<TTestScope>
    {
        public string Name { get; }
        public EventArgs EventArgs { get; }

        public EventArgsWithName(string name, EventArgs eventArgs)
        {
            Guard.NotNullNorEmpty(name, nameof(name));
            Guard.NotNull(eventArgs, nameof(eventArgs));

            Name = name;
            EventArgs = eventArgs;
        }

        public override string ToString()
        {
            return $"{Name}: {EventArgs}";
        }
    }
}
