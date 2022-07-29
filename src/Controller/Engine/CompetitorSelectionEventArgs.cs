namespace DogAgilityCompetition.Controller.Engine;

/// <summary />
public class CompetitorSelectionEventArgs : EventArgs
{
    public bool IsCurrentCompetitor { get; }

    public CompetitorSelectionEventArgs(bool isCurrentCompetitor)
    {
        IsCurrentCompetitor = isCurrentCompetitor;
    }
}
