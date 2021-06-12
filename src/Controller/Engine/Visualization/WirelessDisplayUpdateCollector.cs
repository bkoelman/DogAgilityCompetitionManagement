using System;
using System.Collections.Generic;
using System.Drawing;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// A builder that collects visualization changes into an immutable set.
    /// </summary>
    public sealed class WirelessDisplayUpdateCollector : IVisualizationActor
    {
        private const int CirceHiddenCompetitorNumber = 0;
        private const int CirceHiddenPlacement = 0;
        private const int CirceHiddenFaultsRefusals = 99;

        private static readonly TimeSpan CirceHiddenTime = TimeSpan.FromMilliseconds(999999);

        private readonly VisualizeFieldSetBuilder builder = new();

        public VisualizeFieldSet GetResult()
        {
            return builder.Build();
        }

        void IVisualizationActor.SetClass(CompetitionClassInfo? classInfo)
        {
        }

        void IVisualizationActor.StartPrimaryTimer()
        {
            builder.WithPrimaryTimerIsActive(true);
        }

        void IVisualizationActor.StopAndSetOrClearPrimaryTime(TimeSpan? time)
        {
            builder.WithPrimaryTimerIsActive(false);
            builder.WithPrimaryTimerValue(time ?? CirceHiddenTime);
        }

        void IVisualizationActor.SetOrClearSecondaryTime(TimeSpan? time, bool doBlink)
        {
            builder.WithSecondaryTimerValue(time ?? CirceHiddenTime);
        }

        void IVisualizationActor.SetOrClearFaultCount(int? count)
        {
            builder.WithCurrentFaultCount(count ?? CirceHiddenFaultsRefusals);
        }

        void IVisualizationActor.SetOrClearRefusalCount(int? count)
        {
            builder.WithCurrentRefusalCount(count ?? CirceHiddenFaultsRefusals);
        }

        void IVisualizationActor.SetElimination(bool isEliminated)
        {
            builder.WithElimination(isEliminated);
        }

        void IVisualizationActor.SetOrClearCurrentCompetitor(Competitor? competitor)
        {
            builder.WithCurrentCompetitorNumber(competitor?.Number ?? CirceHiddenCompetitorNumber);
        }

        void IVisualizationActor.SetCurrentCompetitorNumber(int number)
        {
            builder.WithCurrentCompetitorNumber(number);
        }

        void IVisualizationActor.BlinkCurrentCompetitorNumber(bool isEnabled)
        {
        }

        void IVisualizationActor.SetOrClearNextCompetitor(Competitor? competitor)
        {
            builder.WithNextCompetitorNumber(competitor?.Number ?? CirceHiddenCompetitorNumber);
        }

        void IVisualizationActor.SetNextCompetitorNumber(int number)
        {
            builder.WithNextCompetitorNumber(number);
        }

        void IVisualizationActor.BlinkNextCompetitorNumber(bool isEnabled)
        {
        }

        void IVisualizationActor.SetOrClearPreviousCompetitorRun(CompetitionRunResult? competitorRunResult)
        {
            builder.WithPreviousPlacement(competitorRunResult?.Placement ?? CirceHiddenPlacement);
        }

        void IVisualizationActor.SetOrClearRankings(IEnumerable<CompetitionRunResult> rankings)
        {
        }

        void IVisualizationActor.SetClockSynchronizationMode(ClockSynchronizationMode mode)
        {
        }

        void IVisualizationActor.StartAnimation(Bitmap bitmap)
        {
        }

        void IVisualizationActor.PlaySound(string? path)
        {
        }
    }
}
