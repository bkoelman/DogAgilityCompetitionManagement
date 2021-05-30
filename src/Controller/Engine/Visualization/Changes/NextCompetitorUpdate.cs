using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears details about next competitor.
    /// </summary>
    public sealed class NextCompetitorUpdate : NullableVisualizationChange<Competitor>
    {
        public static NextCompetitorUpdate Hidden => new(null);

        public NextCompetitorUpdate(Competitor? value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.SetOrClearNextCompetitor(Value);
        }
    }
}
