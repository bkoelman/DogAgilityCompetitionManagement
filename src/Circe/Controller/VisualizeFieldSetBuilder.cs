using System;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// A builder to construct an immutable <see cref="VisualizeFieldSet" /> instance.
    /// </summary>
    public sealed class VisualizeFieldSetBuilder
    {
        private int? currentCompetitorNumber;
        private int? nextCompetitorNumber;
        private bool startPrimaryTimer;
        private TimeSpan? primaryTimerValue;
        private TimeSpan? secondaryTimerValue;
        private int? currentFaultCount;
        private int? currentRefusalCount;
        private bool? currentIsEliminated;
        private int? previousPlacement;

        public VisualizeFieldSet Build()
        {
            return new(currentCompetitorNumber, nextCompetitorNumber, startPrimaryTimer, primaryTimerValue, secondaryTimerValue, currentFaultCount,
                currentRefusalCount, currentIsEliminated, previousPlacement);
        }

        public VisualizeFieldSetBuilder WithCurrentCompetitorNumber(int number)
        {
            currentCompetitorNumber = number;
            return this;
        }

        public VisualizeFieldSetBuilder WithNextCompetitorNumber(int number)
        {
            nextCompetitorNumber = number;
            return this;
        }

        public VisualizeFieldSetBuilder WithPrimaryTimerIsActive(bool isActive)
        {
            startPrimaryTimer = isActive;
            return this;
        }

        public VisualizeFieldSetBuilder WithPrimaryTimerValue(TimeSpan value)
        {
            primaryTimerValue = value;
            return this;
        }

        public VisualizeFieldSetBuilder WithSecondaryTimerValue(TimeSpan value)
        {
            secondaryTimerValue = value;
            return this;
        }

        public VisualizeFieldSetBuilder WithCurrentFaultCount(int faultCount)
        {
            currentFaultCount = faultCount;
            return this;
        }

        public VisualizeFieldSetBuilder WithCurrentRefusalCount(int refusalCount)
        {
            currentRefusalCount = refusalCount;
            return this;
        }

        public VisualizeFieldSetBuilder WithElimination(bool isEliminated)
        {
            currentIsEliminated = isEliminated;
            return this;
        }

        public VisualizeFieldSetBuilder WithPreviousPlacement(int placement)
        {
            previousPlacement = placement;
            return this;
        }
    }
}
