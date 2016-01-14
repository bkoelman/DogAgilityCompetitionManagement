using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears competitor run result rankings.
    /// </summary>
    public sealed class RankingsUpdate : NotNullableVisualizationChange<IReadOnlyCollection<CompetitionRunResult>>
    {
        public RankingsUpdate([NotNull] [ItemNotNull] IReadOnlyCollection<CompetitionRunResult> value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));

            actor.SetOrClearRankings(Value);
        }

        [Pure]
        public override string ToString() => $"{GetType().Name} Value: [{Value.Count}]";
    }
}