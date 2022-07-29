using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary />
public sealed class CompetitorNumberSelectionEventArgs : CompetitorSelectionEventArgs
{
    public int CompetitorNumber { get; }

    public CompetitorNumberSelectionEventArgs(int competitorNumber, bool isCurrentCompetitor)
        : base(isCurrentCompetitor)
    {
        Guard.GreaterOrEqual(competitorNumber, nameof(competitorNumber), 1);

        CompetitorNumber = competitorNumber;
    }
}
