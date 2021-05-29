using System;
using System.Text;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// Provides an immutable representation of the data in a CIRCE <see cref="VisualizeOperation" />.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public struct VisualizeFieldSet : IEquatable<VisualizeFieldSet>
    {
        [CanBeNull]
        public int? CurrentCompetitorNumber { get; }

        [CanBeNull]
        public int? NextCompetitorNumber { get; }

        public bool StartPrimaryTimer { get; }

        [CanBeNull]
        public TimeSpan? PrimaryTimerValue { get; }

        [CanBeNull]
        public TimeSpan? SecondaryTimerValue { get; }

        [CanBeNull]
        public int? CurrentFaultCount { get; }

        [CanBeNull]
        public int? CurrentRefusalCount { get; }

        [CanBeNull]
        public bool? CurrentIsEliminated { get; }

        [CanBeNull]
        public int? PreviousPlacement { get; }

        public bool IsEmpty =>
            CurrentCompetitorNumber == null && NextCompetitorNumber == null && !StartPrimaryTimer && PrimaryTimerValue == null && SecondaryTimerValue == null &&
            CurrentFaultCount == null && CurrentRefusalCount == null && CurrentIsEliminated == null && PreviousPlacement == null;

        public VisualizeFieldSet([CanBeNull] int? currentCompetitorNumber, [CanBeNull] int? nextCompetitorNumber, bool startPrimaryTimer,
            [CanBeNull] TimeSpan? primaryTimerValue, [CanBeNull] TimeSpan? secondaryTimerValue, [CanBeNull] int? currentFaultCount,
            [CanBeNull] int? currentRefusalCount, [CanBeNull] bool? currentIsEliminated, [CanBeNull] int? previousPlacement)
        {
            CurrentCompetitorNumber = currentCompetitorNumber;
            NextCompetitorNumber = nextCompetitorNumber;
            StartPrimaryTimer = startPrimaryTimer;
            PrimaryTimerValue = primaryTimerValue;
            SecondaryTimerValue = secondaryTimerValue;
            CurrentFaultCount = currentFaultCount;
            CurrentRefusalCount = currentRefusalCount;
            CurrentIsEliminated = currentIsEliminated;
            PreviousPlacement = previousPlacement;
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            using (var formatter = new ObjectFormatter(textBuilder, this))
            {
                int? tempCurrentCompetitorNumber = CurrentCompetitorNumber;
                formatter.Append(() => tempCurrentCompetitorNumber, () => tempCurrentCompetitorNumber);

                int? tempNextCompetitorNumber = NextCompetitorNumber;
                formatter.Append(() => tempNextCompetitorNumber, () => tempNextCompetitorNumber);

                bool tempPrimaryTimerIsActive = StartPrimaryTimer;
                formatter.Append(() => tempPrimaryTimerIsActive, () => tempPrimaryTimerIsActive);

                TimeSpan? tempPrimaryTimerValue = PrimaryTimerValue;
                formatter.Append(() => tempPrimaryTimerValue, () => tempPrimaryTimerValue);

                TimeSpan? tempSecondaryTimerValue = SecondaryTimerValue;
                formatter.Append(() => tempSecondaryTimerValue, () => tempSecondaryTimerValue);

                int? tempCurrentFaultCount = CurrentFaultCount;
                formatter.Append(() => tempCurrentFaultCount, () => tempCurrentFaultCount);

                int? tempCurrentRefusalCount = CurrentRefusalCount;
                formatter.Append(() => tempCurrentRefusalCount, () => tempCurrentRefusalCount);

                bool? tempCurrentIsEliminated = CurrentIsEliminated;
                formatter.Append(() => tempCurrentIsEliminated, () => tempCurrentIsEliminated);

                int? tempPreviousPlacement = PreviousPlacement;
                formatter.Append(() => tempPreviousPlacement, () => tempPreviousPlacement);
            }

            return textBuilder.ToString();
        }

        [Pure]
        public bool Equals(VisualizeFieldSet other)
        {
            return other.CurrentCompetitorNumber == CurrentCompetitorNumber && other.NextCompetitorNumber == NextCompetitorNumber &&
                other.StartPrimaryTimer == StartPrimaryTimer && other.PrimaryTimerValue == PrimaryTimerValue &&
                other.SecondaryTimerValue == SecondaryTimerValue && other.CurrentFaultCount == CurrentFaultCount &&
                other.CurrentRefusalCount == CurrentRefusalCount && other.CurrentIsEliminated == CurrentIsEliminated &&
                other.PreviousPlacement == PreviousPlacement;
        }

        [Pure]
        public override bool Equals([CanBeNull] object obj)
        {
            return obj is VisualizeFieldSet visualizeFieldSet && Equals(visualizeFieldSet);
        }

        [Pure]
        public override int GetHashCode()
        {
            return GetHashCodeForNullable(CurrentCompetitorNumber) ^ GetHashCodeForNullable(NextCompetitorNumber) ^ StartPrimaryTimer.GetHashCode() ^
                GetHashCodeForNullable(PrimaryTimerValue) ^ GetHashCodeForNullable(SecondaryTimerValue) ^ GetHashCodeForNullable(CurrentFaultCount) ^
                GetHashCodeForNullable(CurrentRefusalCount) ^ GetHashCodeForNullable(CurrentIsEliminated) ^ GetHashCodeForNullable(PreviousPlacement);
        }

        private static int GetHashCodeForNullable<T>([CanBeNull] T? value)
            where T : struct
        {
            return value?.GetHashCode() ?? 0;
        }

        [Pure]
        public static bool operator ==(VisualizeFieldSet left, VisualizeFieldSet right)
        {
            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(VisualizeFieldSet left, VisualizeFieldSet right)
        {
            return !(left == right);
        }
    }
}
