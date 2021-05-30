using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that starts/stops number blinking for current competitor.
    /// </summary>
    public sealed class CurrentCompetitorNumberBlink : NotNullableVisualizationChange<bool>
    {
        public static CurrentCompetitorNumberBlink On => new(true);
        public static CurrentCompetitorNumberBlink Off => new(false);

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
