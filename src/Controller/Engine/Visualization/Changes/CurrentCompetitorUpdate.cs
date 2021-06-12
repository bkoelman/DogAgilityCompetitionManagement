using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears details about current competitor.
    /// </summary>
    public sealed class CurrentCompetitorUpdate : NullableVisualizationChange<Competitor>
    {
        public static CurrentCompetitorUpdate Hidden => new(null);

        public CurrentCompetitorUpdate(Competitor? value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.SetOrClearCurrentCompetitor(Value);
        }
    }
}
