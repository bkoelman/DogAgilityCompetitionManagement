using System;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that freezes the running timer to a specific value or clears it.
    /// </summary>
    public sealed class PrimaryTimeStopAndSet : NullableVisualizationChange<TimeSpan?>
    {
        public static PrimaryTimeStopAndSet Hidden => new(null);
        public static PrimaryTimeStopAndSet Zero => new(TimeSpan.Zero);

        public PrimaryTimeStopAndSet(TimeSpan? value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.StopAndSetOrClearPrimaryTime(Value);
        }

        public static PrimaryTimeStopAndSet FromTimeSpanWithAccuracy(TimeSpanWithAccuracy? value)
        {
            return new(value?.TimeValue);
        }
    }
}
