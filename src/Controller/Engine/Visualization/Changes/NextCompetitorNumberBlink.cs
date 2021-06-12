using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that starts/stops number blinking for next competitor.
    /// </summary>
    public sealed class NextCompetitorNumberBlink : NotNullableVisualizationChange<bool>
    {
        public static NextCompetitorNumberBlink On => new(true);
        public static NextCompetitorNumberBlink Off => new(false);

        public NextCompetitorNumberBlink(bool value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.BlinkNextCompetitorNumber(Value);
        }
    }
}
