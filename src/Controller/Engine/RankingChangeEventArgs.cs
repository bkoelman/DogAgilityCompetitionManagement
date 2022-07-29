using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary />
public sealed class RankingChangeEventArgs : EventArgs
{
    public IReadOnlyCollection<CompetitionRunResult> Rankings { get; }
    public CompetitionRunResult? PreviousRunResult { get; }

    public RankingChangeEventArgs(IReadOnlyCollection<CompetitionRunResult> rankings, CompetitionRunResult? previousRunResult)
    {
        Guard.NotNull(rankings, nameof(rankings));

        Rankings = rankings;
        PreviousRunResult = previousRunResult;
    }
}
