using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that updates/clears competitor run result rankings.
/// </summary>
public sealed class RankingsUpdate : NotNullableVisualizationChange<IReadOnlyCollection<CompetitionRunResult>>
{
    public static RankingsUpdate Hidden => new(Array.Empty<CompetitionRunResult>());

    public RankingsUpdate(IReadOnlyCollection<CompetitionRunResult> value)
        : base(value)
    {
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetOrClearRankings(Value);
    }

    [Pure]
    public override string ToString()
    {
        return $"{GetType().Name} Value: [{Value.Count}]";
    }
}
