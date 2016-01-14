using System;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public class CompetitorSelectionEventArgs : EventArgs
    {
        public bool IsCurrentCompetitor { get; private set; }

        public CompetitorSelectionEventArgs(bool isCurrentCompetitor)
        {
            IsCurrentCompetitor = isCurrentCompetitor;
        }
    }
}