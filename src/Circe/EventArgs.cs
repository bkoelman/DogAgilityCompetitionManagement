using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public sealed class EventArgs<T> : EventArgs
    {
        [NotNull]
        public T Argument { get; private set; }

        public EventArgs([NotNull] T argument)
        {
            Guard.NotNull(argument, nameof(argument));

            Argument = argument;
        }
    }
}