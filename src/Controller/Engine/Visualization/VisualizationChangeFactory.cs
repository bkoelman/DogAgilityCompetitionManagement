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
                ClassInfoUpdate.Hidden,
                PrimaryTimeStopAndSet.Hidden,
                SecondaryTimeUpdate.Hidden,
                FaultCountUpdate.Hidden,
                RefusalCountUpdate.Hidden,
                EliminationUpdate.Off,
                CurrentCompetitorUpdate.Hidden,
                NextCompetitorUpdate.Hidden,
                PreviousCompetitorRunUpdate.Hidden,
                RankingsUpdate.Hidden
            };
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<VisualizationChange> ClearCurrentRun()
        {
            return new VisualizationChange[]
            {
                PrimaryTimeStopAndSet.Hidden,
                SecondaryTimeUpdate.Hidden,
                FaultCountUpdate.Hidden,
                RefusalCountUpdate.Hidden,
                EliminationUpdate.Off
            };
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<VisualizationChange> ResetCurrentRun()
        {
            return new VisualizationChange[]
            {
                PrimaryTimeStopAndSet.Zero,
                SecondaryTimeUpdate.Hidden,
                FaultCountUpdate.Zero,
                RefusalCountUpdate.Zero,
                EliminationUpdate.Off
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

            return new List<VisualizationChange>
            {
                new EliminationUpdate(existingRunResult.IsEliminated),
                new FaultCountUpdate(existingRunResult.FaultCount),
                new RefusalCountUpdate(existingRunResult.RefusalCount),
                PrimaryTimeStopAndSet.FromTimeSpanWithAccuracy(finishTimeElapsed),
                SecondaryTimeUpdate.FromTimeSpanWithAccuracy(intermediateTimeElapsed, false)
            };
        }

        [NotNull]
        public static VisualizationChange CompetitorNumberBlinkOn(bool isCurrentCompetitor)
        {
            return isCurrentCompetitor
                ? (VisualizationChange) CurrentCompetitorNumberBlink.On
                : NextCompetitorNumberBlink.On;
        }

        [NotNull]
        public static VisualizationChange CompetitorNumberBlinkOff(bool isCurrentCompetitor)
        {
            return isCurrentCompetitor
                ? (VisualizationChange) CurrentCompetitorNumberBlink.Off
                : NextCompetitorNumberBlink.Off;
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