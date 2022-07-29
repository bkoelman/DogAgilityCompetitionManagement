namespace DogAgilityCompetition.Controller.Engine;

/// <summary />
public sealed class EliminationEventArgs : EventArgs
{
    public bool IsEliminated { get; }

    public EliminationEventArgs(bool isEliminated)
    {
        IsEliminated = isEliminated;
    }
}
