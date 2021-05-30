using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Sorts a set of competition run results and recalculates placements.
    /// </summary>
    public sealed class ClassPlacementCalculator
    {
        private readonly CompetitionRunResultRankingComparer comparer;

        public ClassPlacementCalculator(CompetitionClassModel modelSnapshot)
        {
            Guard.NotNull(modelSnapshot, nameof(modelSnapshot));

            comparer = new CompetitionRunResultRankingComparer(modelSnapshot, RankingComparisonMode.Regular);
        }

        [Pure]
        public IEnumerable<CompetitionRunResult> Recalculate(IEnumerable<CompetitionRunResult> runResults)
        {
            Guard.NotNull(runResults, nameof(runResults));

            using (new CodeTimer("Recalculate rankings"))
            {
                var sorted = new SortedSet<CompetitionRunResult>(runResults, comparer);
                var newResults = new List<CompetitionRunResult>();

                int placementCounter = 0;
                CompetitionRunResult? previousResult = null;

                foreach (CompetitionRunResult runResult in sorted)
                {
                    if (runResult.HasFinished && !runResult.IsEliminated)
                    {
                        int placement;

                        if (previousResult != null && comparer.IsPlacementEqual(runResult, previousResult))
                        {
                            placement = previousResult.Placement;
                        }
                        else
                        {
                            placementCounter++;
                            placement = placementCounter;
                        }

                        CompetitionRunResult runResultWithPlacement = runResult.ChangePlacement(placement);
                        newResults.Add(runResultWithPlacement);
                        previousResult = runResultWithPlacement;
                    }
                    else
                    {
                        CompetitionRunResult runResultWithoutPlacement = runResult.ChangePlacement(0);
                        newResults.Add(runResultWithoutPlacement);
                    }
                }

                return newResults;
            }
        }
    }
}
