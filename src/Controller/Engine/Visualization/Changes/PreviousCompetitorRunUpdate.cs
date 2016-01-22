using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears details about previous competitor result.
    /// </summary>
    public sealed class PreviousCompetitorRunUpdate : NullableVisualizationChange<CompetitionRunResult>
    {
        [NotNull]
        public static PreviousCompetitorRunUpdate Hidden => new PreviousCompetitorRunUpdate(null);

        public PreviousCompetitorRunUpdate([CanBeNull] CompetitionRunResult value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.SetOrClearPreviousCompetitorRun(Value);
        }
    }
}