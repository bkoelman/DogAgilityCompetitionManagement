using System;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary />
    public sealed class ClockSynchronizationCompletedEventArgs : EventArgs
    {
        public ClockSynchronizationResult Result { get; }

        public ClockSynchronizationCompletedEventArgs(ClockSynchronizationResult result)
        {
            Result = result;
        }
    }
}
