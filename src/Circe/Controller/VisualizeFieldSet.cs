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
    public readonly struct VisualizeFieldSet : IEquatable<VisualizeFieldSet>
    {
        public int? CurrentCompetitorNumber { get; }
        public int? NextCompetitorNumber { get; }
        public bool StartPrimaryTimer { get; }
        public TimeSpan? PrimaryTimerValue { get; }
        public TimeSpan? SecondaryTimerValue { get; }
        public int? CurrentFaultCount { get; }
        public int? CurrentRefusalCount { get; }
        public bool? CurrentIsEliminated { get; }
        public int? PreviousPlacement { get; }

        public bool IsEmpty =>
            CurrentCompetitorNumber == null && NextCompetitorNumber == null && !StartPrimaryTimer && PrimaryTimerValue == null && SecondaryTimerValue == null &&
            CurrentFaultCount == null && CurrentRefusalCount == null && CurrentIsEliminated == null && PreviousPlacement == null;

        public VisualizeFieldSet(int? currentCompetitorNumber, int? nextCompetitorNumber, bool startPrimaryTimer, TimeSpan? primaryTimerValue,
            TimeSpan? secondaryTimerValue, int? currentFaultCount, int? currentRefusalCount, bool? currentIsEliminated, int? previousPlacement)
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
                formatter.Append(CurrentCompetitorNumber, nameof(CurrentCompetitorNumber));
                formatter.Append(NextCompetitorNumber, nameof(NextCompetitorNumber));
                formatter.Append(StartPrimaryTimer, nameof(StartPrimaryTimer));
                formatter.Append(PrimaryTimerValue, nameof(PrimaryTimerValue));
                formatter.Append(SecondaryTimerValue, nameof(SecondaryTimerValue));
                formatter.Append(CurrentFaultCount, nameof(CurrentFaultCount));
                formatter.Append(CurrentRefusalCount, nameof(CurrentRefusalCount));
                formatter.Append(CurrentIsEliminated, nameof(CurrentIsEliminated));
                formatter.Append(PreviousPlacement, nameof(PreviousPlacement));
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
        public override bool Equals(object? obj)
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

        private static int GetHashCodeForNullable<T>(T? value)
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
