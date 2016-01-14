using System;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary />
    public sealed class ClockSynchronizationCompletedEventArgs : EventArgs
    {
        public ClockSynchronizationResult Result { get; private set; }

        public ClockSynchronizationCompletedEventArgs(ClockSynchronizationResult result)
        {
            Result = result;
        }
    }
}