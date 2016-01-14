using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// A builder to construct an immutable <see cref="VisualizeFieldSet" /> instance.
    /// </summary>
    public sealed class VisualizeFieldSetBuilder
    {
        [CanBeNull]
        private int? currentCompetitorNumber;

        [CanBeNull]
        private int? nextCompetitorNumber;

        private bool startPrimaryTimer;

        [CanBeNull]
        private TimeSpan? primaryTimerValue;

        [CanBeNull]
        private int? currentFaultCount;

        [CanBeNull]
        private int? currentRefusalCount;

        [CanBeNull]
        private bool? currentIsEliminated;

        [CanBeNull]
        private int? previousPlacement;

        public VisualizeFieldSet Build()
        {
            return new VisualizeFieldSet(currentCompetitorNumber, nextCompetitorNumber, startPrimaryTimer,
                primaryTimerValue, currentFaultCount, currentRefusalCount, currentIsEliminated, previousPlacement);
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithCurrentCompetitorNumber(int number)
        {
            currentCompetitorNumber = number;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithNextCompetitorNumber(int number)
        {
            nextCompetitorNumber = number;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithPrimaryTimerIsActive(bool isActive)
        {
            startPrimaryTimer = isActive;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithPrimaryTimerValue(TimeSpan value)
        {
            primaryTimerValue = value;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithCurrentFaultCount(int faultCount)
        {
            currentFaultCount = faultCount;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithCurrentRefusalCount(int refusalCount)
        {
            currentRefusalCount = refusalCount;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithElimination(bool isEliminated)
        {
            currentIsEliminated = isEliminated;
            return this;
        }

        [NotNull]
        public VisualizeFieldSetBuilder WithPreviousPlacement(int placement)
        {
            previousPlacement = placement;
            return this;
        }
    }
}