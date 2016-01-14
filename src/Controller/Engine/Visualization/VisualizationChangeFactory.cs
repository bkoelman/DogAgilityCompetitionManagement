using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Controller.Engine.Visualization.Changes;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Provides a factory for creating sets of visualization changes that represent higher-level scenarios.
    /// </summary>
    public static class VisualizationChangeFactory
    {
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<VisualizationChange> ClearAll()
        {
            return new VisualizationChange[]
            {
                new ClassInfoUpdate(null),
                new PrimaryTimeStopAndSet(null),
                new SecondaryTimeUpdate(null, false),
                new FaultCountUpdate(null),
                new RefusalCountUpdate(null),
                new EliminationUpdate(false),
                new CurrentCompetitorUpdate(null),
                new NextCompetitorUpdate(null),
                new PreviousCompetitorRunUpdate(null),
                new RankingsUpdate(new CompetitionRunResult[0])
            };
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<VisualizationChange> ClearCurrentRun()
        {
            return new VisualizationChange[]
            {
                new PrimaryTimeStopAndSet(null),
                new SecondaryTimeUpdate(null, false),
                new FaultCountUpdate(null),
                new RefusalCountUpdate(null),
                new EliminationUpdate(false)
            };
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<VisualizationChange> ResetCurrentRun()
        {
            return new VisualizationChange[]
            {
                new PrimaryTimeStopAndSet(TimeSpan.Zero),
                new SecondaryTimeUpdate(null, false),
                new FaultCountUpdate(0),
                new RefusalCountUpdate(0),
                new EliminationUpdate(false)
            };
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<VisualizationChange> ForExistingRun([NotNull] CompetitionRunResult existingRunResult,
            [CanBeNull] RecordedTime latestIntermediateTimeOrNull)
        {
            Guard.NotNull(existingRunResult, nameof(existingRunResult));

            TimeSpanWithAccuracy? finishTimeElapsed = null;
            TimeSpanWithAccuracy? intermediateTimeElapsed = null;

            if (existingRunResult.Timings != null)
            {
                if (existingRunResult.Timings.FinishTime != null)
                {
                    finishTimeElapsed =
                        existingRunResult.Timings.FinishTime.ElapsedSince(existingRunResult.Timings.StartTime);
                }

                if (latestIntermediateTimeOrNull != null)
                {
                    intermediateTimeElapsed =
                        latestIntermediateTimeOrNull.ElapsedSince(existingRunResult.Timings.StartTime);
                }
            }

            var changes = new List<VisualizationChange>
            {
                new EliminationUpdate(existingRunResult.IsEliminated),
                new FaultCountUpdate(existingRunResult.FaultCount),
                new RefusalCountUpdate(existingRunResult.RefusalCount),
                PrimaryTimeStopAndSet.FromTimeSpanWithAccuracy(finishTimeElapsed),
                SecondaryTimeUpdate.FromTimeSpanWithAccuracy(intermediateTimeElapsed, false)
            };
            return changes;
        }

        [NotNull]
        public static VisualizationChange CompetitorNumberBlink(bool isCurrentCompetitor, bool isEnabled)
        {
            return isCurrentCompetitor
                ? (VisualizationChange) new CurrentCompetitorNumberBlink(isEnabled)
                : new NextCompetitorNumberBlink(isEnabled);
        }

        [NotNull]
        public static VisualizationChange CompetitorNumberUpdate(bool isCurrentCompetitor, int competitorNumber)
        {
            return isCurrentCompetitor
                ? (VisualizationChange) new CurrentCompetitorNumberUpdate(competitorNumber)
                : new NextCompetitorNumberUpdate(competitorNumber);
        }
    }
}