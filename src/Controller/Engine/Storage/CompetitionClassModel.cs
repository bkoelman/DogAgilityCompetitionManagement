using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// The root for information about a competition, including class, competitors and run results.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class CompetitionClassModel
    {
        [NotNull]
        [ItemNotNull]
        public IReadOnlyCollection<CompetitionRunResult> Results { get; }

        [NotNull]
        public CompetitionClassInfo ClassInfo { get; }

        [CanBeNull]
        public int? LastCompletedCompetitorNumber { get; }

        public int IntermediateTimerCount { get; }
        public TimeSpan StartFinishMinDelayForSingleSensor { get; }

        [NotNull]
        public CompetitionAlerts Alerts { get; }

        public CompetitionClassModel()
        {
            Results = new ReadOnlyCollection<CompetitionRunResult>(new List<CompetitionRunResult>());
            LastCompletedCompetitorNumber = null;
            ClassInfo = new CompetitionClassInfo();
            IntermediateTimerCount = 0;
            StartFinishMinDelayForSingleSensor = TimeSpan.FromSeconds(3);
            Alerts = CompetitionAlerts.Empty;
        }

        private CompetitionClassModel([NotNull] [ItemNotNull] IReadOnlyCollection<CompetitionRunResult> results, [NotNull] CompetitionClassInfo classInfo,
            [CanBeNull] int? lastCompletedCompetitorNumber, int intermediateTimerCount, TimeSpan startFinishMinDelayForSingleSensor,
            [CanBeNull] CompetitionAlerts alerts)
        {
            AssertUniqueCompetitors(results);

            Results = results;
            ClassInfo = classInfo;
            LastCompletedCompetitorNumber = lastCompletedCompetitorNumber;
            IntermediateTimerCount = intermediateTimerCount;
            StartFinishMinDelayForSingleSensor = startFinishMinDelayForSingleSensor;
            Alerts = alerts ?? CompetitionAlerts.Empty;
        }

        [AssertionMethod]
        private static void AssertUniqueCompetitors([NotNull] [ItemNotNull] IEnumerable<CompetitionRunResult> results)
        {
            var map = new Dictionary<int, CompetitionRunResult>();

            foreach (CompetitionRunResult result in results)
            {
                if (map.ContainsKey(result.Competitor.Number))
                {
                    throw new ArgumentException($"Competitor number {result.Competitor.Number} occurs multiple times.");
                }

                map[result.Competitor.Number] = result;
            }
        }

        [Pure]
        [CanBeNull]
        public CompetitionRunResult GetLastCompletedOrNull()
        {
            return LastCompletedCompetitorNumber != null ? GetRunResultOrNull(LastCompletedCompetitorNumber.Value) : null;
        }

        [Pure]
        [NotNull]
        public CompetitionRunResult GetRunResultFor(int competitorNumber)
        {
            CompetitionRunResult runResult = GetRunResultOrNull(competitorNumber);

            if (runResult == null)
            {
                throw new ArgumentException($"Competitor {competitorNumber} does not exist.");
            }

            return runResult;
        }

        [Pure]
        [CanBeNull]
        public RecordedTime GetLatestIntermediateTimeOrNull(int competitorNumber)
        {
            CompetitionRunResult runResult = GetRunResultOrNull(competitorNumber);

            if (runResult?.Timings != null)
            {
                if (IntermediateTimerCount >= 3 && runResult.Timings.IntermediateTime3 != null)
                {
                    return runResult.Timings.IntermediateTime3;
                }

                if (IntermediateTimerCount == 2 && runResult.Timings.IntermediateTime2 != null)
                {
                    return runResult.Timings.IntermediateTime2;
                }

                if (IntermediateTimerCount == 1 && runResult.Timings.IntermediateTime1 != null)
                {
                    return runResult.Timings.IntermediateTime1;
                }
            }

            return null;
        }

        [Pure]
        [CanBeNull]
        public CompetitionRunResult GetRunResultOrNull(int competitorNumber)
        {
            return Results.SingleOrDefault(r => r.Competitor.Number == competitorNumber);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel ChangeRunResults([NotNull] [ItemNotNull] IEnumerable<CompetitionRunResult> runResults)
        {
            Guard.NotNull(runResults, nameof(runResults));

            var newResultList = new ReadOnlyCollection<CompetitionRunResult>(runResults.ToList());

            return new CompetitionClassModel(newResultList, ClassInfo, LastCompletedCompetitorNumber, IntermediateTimerCount,
                StartFinishMinDelayForSingleSensor, Alerts);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel ChangeRunResult([NotNull] CompetitionRunResult runResult)
        {
            Guard.NotNull(runResult, nameof(runResult));

            var newResults = new List<CompetitionRunResult>();

            bool found = false;

            foreach (CompetitionRunResult result in Results)
            {
                if (result.Competitor.Number == runResult.Competitor.Number)
                {
                    newResults.Add(runResult);
                    found = true;
                }
                else
                {
                    newResults.Add(result);
                }
            }

            if (!found)
            {
                throw new KeyNotFoundException($"No competitor found with number {runResult.Competitor.Number}");
            }

            var newResultList = new ReadOnlyCollection<CompetitionRunResult>(newResults);

            return new CompetitionClassModel(newResultList, ClassInfo, LastCompletedCompetitorNumber, IntermediateTimerCount,
                StartFinishMinDelayForSingleSensor, Alerts);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel ChangeClassInfo([NotNull] CompetitionClassInfo classInfo)
        {
            Guard.NotNull(classInfo, nameof(classInfo));

            return new CompetitionClassModel(Results, classInfo, LastCompletedCompetitorNumber, IntermediateTimerCount, StartFinishMinDelayForSingleSensor,
                Alerts);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel SafeChangeLastCompletedCompetitorNumber([CanBeNull] int? lastCompletedCompetitorNumber)
        {
            if (lastCompletedCompetitorNumber != null)
            {
                CompetitionRunResult runResult = GetRunResultOrNull(lastCompletedCompetitorNumber.Value);

                if (runResult == null)
                {
                    lastCompletedCompetitorNumber = null;
                }
            }

            return new CompetitionClassModel(Results, ClassInfo, lastCompletedCompetitorNumber, IntermediateTimerCount, StartFinishMinDelayForSingleSensor,
                Alerts);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel ChangeIntermediateTimerCount(int intermediateTimerCount)
        {
            Guard.InRangeInclusive(intermediateTimerCount, nameof(intermediateTimerCount), 0, 3);

            return new CompetitionClassModel(Results, ClassInfo, LastCompletedCompetitorNumber, intermediateTimerCount, StartFinishMinDelayForSingleSensor,
                Alerts);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel ChangeStartFinishMinDelayForSingleSensor(TimeSpan startFinishMinDelayForSingleSensor)
        {
            AssertNotNegative(startFinishMinDelayForSingleSensor);

            return new CompetitionClassModel(Results, ClassInfo, LastCompletedCompetitorNumber, IntermediateTimerCount, startFinishMinDelayForSingleSensor,
                Alerts);
        }

        [AssertionMethod]
        private static void AssertNotNegative(TimeSpan startFinishMinDelayForSingleSensor)
        {
            if (startFinishMinDelayForSingleSensor < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(startFinishMinDelayForSingleSensor), startFinishMinDelayForSingleSensor,
                    "Minimum delay between passage of Start and Finish gates cannot be negative.");
            }
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel ChangeAlerts([CanBeNull] CompetitionAlerts alerts)
        {
            return new(Results, ClassInfo, LastCompletedCompetitorNumber, IntermediateTimerCount, StartFinishMinDelayForSingleSensor, alerts);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel GetSortedAscendingByCompetitorNumber()
        {
            List<CompetitionRunResult> sorted = Results.ToList();
            sorted.Sort((first, second) => first.Competitor.Number.CompareTo(second.Competitor.Number));

            return ChangeRunResults(sorted);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel FilterCompletedAndSortedAscendingByPlacement()
        {
            IEnumerable<CompetitionRunResult> filtered = Results.Where(r => r.Placement > 0).OrderBy(r => r.Placement);
            return ChangeRunResults(filtered);
        }

        [Pure]
        [NotNull]
        public CompetitionClassModel RecalculatePlacements()
        {
            var calculator = new ClassPlacementCalculator(this);
            IEnumerable<CompetitionRunResult> runResultsRecalculated = calculator.Recalculate(Results);
            return ChangeRunResults(runResultsRecalculated);
        }

        [Pure]
        public int GetBestStartingCompetitorNumber()
        {
            AssertCompetitorsExist();

            CompetitionClassModel snapshotSorted = GetSortedAscendingByCompetitorNumber();

            CompetitionRunResult lastCompleted = snapshotSorted.GetLastCompletedOrNull();

            if (lastCompleted != null)
            {
                // @formatter:keep_existing_linebreaks true

                CompetitionRunResult nextUncompletedCompetitor = snapshotSorted.Results
                    .SkipWhile(r => r.Competitor.Number != lastCompleted.Competitor.Number)
                    .Skip(1)
                    .SkipWhile(r => r.HasCompleted)
                    .FirstOrDefault();

                // @formatter:keep_existing_linebreaks restore

                if (nextUncompletedCompetitor != null)
                {
                    return nextUncompletedCompetitor.Competitor.Number;
                }
            }

            CompetitionRunResult firstUncompletedCompetitor = snapshotSorted.Results.FirstOrDefault(r => !r.HasCompleted);
            return firstUncompletedCompetitor?.Competitor.Number ?? snapshotSorted.Results.First().Competitor.Number;
        }

        [AssertionMethod]
        private void AssertCompetitorsExist()
        {
            if (!Results.Any())
            {
                throw new InvalidOperationException("No competitors available.");
            }
        }

        [Pure]
        [CanBeNull]
        public int? GetBestNextCompetitorNumberOrNull([CanBeNull] int? currentCompetitorNumber)
        {
            if (currentCompetitorNumber != null)
            {
                CompetitionClassModel snapshotSorted = GetSortedAscendingByCompetitorNumber();

                // @formatter:keep_existing_linebreaks true

                CompetitionRunResult firstUncompletedCompetitorAfterCurrent = snapshotSorted.Results
                    .SkipWhile(r => r.Competitor.Number != currentCompetitorNumber.Value)
                    .Skip(1)
                    .SkipWhile(r => r.HasCompleted)
                    .FirstOrDefault();

                // @formatter:keep_existing_linebreaks restore

                if (firstUncompletedCompetitorAfterCurrent != null)
                {
                    return firstUncompletedCompetitorAfterCurrent.Competitor.Number;
                }

                CompetitionRunResult firstUncompletedCompetitor =
                    snapshotSorted.Results.FirstOrDefault(r => !r.HasCompleted && r.Competitor.Number != currentCompetitorNumber.Value);

                return firstUncompletedCompetitor?.Competitor.Number;
            }

            return null;
        }
    }
}
