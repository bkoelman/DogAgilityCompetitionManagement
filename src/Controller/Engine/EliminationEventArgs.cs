using System;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class EliminationEventArgs : EventArgs
    {
        public EliminationEventArgs(bool isEliminated)
        {
            IsEliminated = isEliminated;
        }

        public bool IsEliminated { get; private set; }
    }
}