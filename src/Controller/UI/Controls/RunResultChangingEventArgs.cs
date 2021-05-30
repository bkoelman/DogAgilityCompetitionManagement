using System;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary />
    public sealed class RunResultChangingEventArgs : EventArgs
    {
        public CompetitionRunResult OriginalRunResult { get; }
        public CompetitionRunResult NewRunResult { get; }
        public string? ErrorMessage { get; set; }

        public RunResultChangingEventArgs(CompetitionRunResult originalRunResult, CompetitionRunResult newRunResult)
        {
            Guard.NotNull(originalRunResult, nameof(originalRunResult));
            Guard.NotNull(newRunResult, nameof(newRunResult));

            OriginalRunResult = originalRunResult;
            NewRunResult = newRunResult;
        }
    }
}
