using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that starts/stops number blinking for current competitor.
    /// </summary>
    public sealed class CurrentCompetitorNumberBlink : NotNullableVisualizationChange<bool>
    {
        [NotNull]
        public static CurrentCompetitorNumberBlink On => new CurrentCompetitorNumberBlink(true);

        [NotNull]
        public static CurrentCompetitorNumberBlink Off => new CurrentCompetitorNumberBlink(false);

        public CurrentCompetitorNumberBlink(bool value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.BlinkCurrentCompetitorNumber(Value);
        }
    }
}