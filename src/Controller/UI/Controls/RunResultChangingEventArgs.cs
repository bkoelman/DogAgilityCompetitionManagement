using System;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary />
    public sealed class RunResultChangingEventArgs : EventArgs
    {
        [NotNull]
        public CompetitionRunResult OriginalRunResult { get; }

        [NotNull]
        public CompetitionRunResult NewRunResult { get; }

        [CanBeNull]
        public string ErrorMessage { get; set; }

        public RunResultChangingEventArgs([NotNull] CompetitionRunResult originalRunResult, [NotNull] CompetitionRunResult newRunResult)
        {
            Guard.NotNull(originalRunResult, nameof(originalRunResult));
            Guard.NotNull(newRunResult, nameof(newRunResult));

            OriginalRunResult = originalRunResult;
            NewRunResult = newRunResult;
        }
    }
}
