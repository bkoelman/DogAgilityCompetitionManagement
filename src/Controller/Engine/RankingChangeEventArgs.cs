using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public sealed class RankingChangeEventArgs : EventArgs
    {
        [NotNull]
        [ItemNotNull]
        public IReadOnlyCollection<CompetitionRunResult> Rankings { get; private set; }

        [CanBeNull]
        public CompetitionRunResult PreviousRunResult { get; private set; }

        public RankingChangeEventArgs([NotNull] [ItemNotNull] IReadOnlyCollection<CompetitionRunResult> rankings,
            [CanBeNull] CompetitionRunResult previousRunResult)
        {
            Guard.NotNull(rankings, nameof(rankings));

            Rankings = rankings;
            PreviousRunResult = previousRunResult;
        }
    }
}