using System;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that freezes the running timer to a specific value or clears it.
    /// </summary>
    public sealed class PrimaryTimeStopAndSet : NullableVisualizationChange<TimeSpan?>
    {
        [NotNull]
        public static PrimaryTimeStopAndSet Hidden => new(null);

        [NotNull]
        public static PrimaryTimeStopAndSet Zero => new(TimeSpan.Zero);

        public PrimaryTimeStopAndSet([CanBeNull] TimeSpan? value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.StopAndSetOrClearPrimaryTime(Value);
        }

        [NotNull]
        public static PrimaryTimeStopAndSet FromTimeSpanWithAccuracy([CanBeNull] TimeSpanWithAccuracy? value)
        {
            return new(value?.TimeValue);
        }
    }
}
