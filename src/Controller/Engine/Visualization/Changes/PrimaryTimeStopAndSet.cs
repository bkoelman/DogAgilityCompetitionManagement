using System;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that freezes/clears the running timer to a specific value.
    /// </summary>
    public sealed class PrimaryTimeStopAndSet : NullableVisualizationChange<TimeSpan?>
    {
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
            return new PrimaryTimeStopAndSet(value?.TimeValue);
        }
    }
}