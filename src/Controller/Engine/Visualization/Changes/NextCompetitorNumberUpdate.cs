using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that changes displayed next competitor number.
    /// </summary>
    public sealed class NextCompetitorNumberUpdate : NotNullableVisualizationChange<int>
    {
        public NextCompetitorNumberUpdate(int value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.SetNextCompetitorNumber(Value);
        }
    }
}
