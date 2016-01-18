using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Used for data binding in <see cref="CompetitionRunResultsGrid" />.
    /// </summary>
    public sealed class CompetitionRunResultRowInGrid
    {
        [NotNull]
        private readonly CompetitionRunResult original;

        [CanBeNull]
        private TimeSpanWithAccuracy? intermediateTime1;

        [CanBeNull]
        private TimeSpanWithAccuracy? intermediateTime2;

        [CanBeNull]
        private TimeSpanWithAccuracy? intermediateTime3;

        [CanBeNull]
        private TimeSpanWithAccuracy? finishTime;

        private int faultCount;
        private int refusalCount;

        public int CompetitorNumber { get; private set; }

        [NotNull]
        public string HandlerName { get; private set; }

        [NotNull]
        public string DogName { get; private set; }

        [CanBeNull]
        public string CountryCode { get; private set; }

        [NotNull]
        public string IntermediateTime1
        {
            get
            {
                return intermediateTime1?.ToString() ?? string.Empty;
            }
            set
            {
                intermediateTime1 = ParseForceUserEdited(value);
            }
        }

        [NotNull]
        public string IntermediateTime2
        {
            get
            {
                return intermediateTime2?.ToString() ?? string.Empty;
            }
            set
            {
                intermediateTime2 = ParseForceUserEdited(value);
            }
        }

        [NotNull]
        public string IntermediateTime3
        {
            get
            {
                return intermediateTime3?.ToString() ?? string.Empty;
            }
            set
            {
                intermediateTime3 = ParseForceUserEdited(value);
            }
        }

        [NotNull]
        public string FinishTime
        {
            get
            {
                return finishTime?.ToString() ?? string.Empty;
            }
            set
            {
                finishTime = ParseForceUserEdited(value);
            }
        }

        public int FaultCount
        {
            get
            {
                return faultCount;
            }
            set
            {
                CompetitionRunResult.AssertFaultCountIsValid(value);
                faultCount = value;
            }
        }

        public int RefusalCount
        {
            get
            {
                return refusalCount;
            }
            set
            {
                CompetitionRunResult.AssertRefusalCountIsValid(value);
                refusalCount = value;
            }
        }

        public bool IsEliminated { get; set; }

        [NotNull]
        public string PlacementText { get; private set; }

        private CompetitionRunResultRowInGrid([NotNull] CompetitionRunResult original, [NotNull] string handlerName,
            [NotNull] string dogName)
        {
            Guard.NotNull(original, nameof(original));
            Guard.NotNullNorEmpty(handlerName, nameof(handlerName));
            Guard.NotNullNorEmpty(dogName, nameof(dogName));

            this.original = original;
            HandlerName = handlerName;
            DogName = dogName;
            PlacementText = string.Empty;
        }

        [CanBeNull]
        private static TimeSpanWithAccuracy? ParseForceUserEdited([CanBeNull] string timeValue)
        {
            TimeSpanWithAccuracy? result = TimeSpanWithAccuracy.FromString(timeValue);
            return result?.ChangeAccuracy(TimeAccuracy.UserEdited);
        }

        [NotNull]
        public static CompetitionRunResultRowInGrid FromCompetitionRunResult([NotNull] CompetitionRunResult source)
        {
            Guard.NotNull(source, nameof(source));

            return new CompetitionRunResultRowInGrid(source, source.Competitor.HandlerName, source.Competitor.DogName)
            {
                CompetitorNumber = source.Competitor.Number,
                CountryCode = source.Competitor.CountryCode,
                intermediateTime1 = source.Timings?.IntermediateTime1?.ElapsedSince(source.Timings.StartTime),
                intermediateTime2 = source.Timings?.IntermediateTime2?.ElapsedSince(source.Timings.StartTime),
                intermediateTime3 = source.Timings?.IntermediateTime3?.ElapsedSince(source.Timings.StartTime),
                finishTime = source.Timings?.FinishTime?.ElapsedSince(source.Timings.StartTime),
                faultCount = source.FaultCount,
                refusalCount = source.RefusalCount,
                IsEliminated = source.IsEliminated,
                PlacementText = source.PlacementText
            };
        }

        [NotNull]
        public CompetitionRunResult ToCompetitionRunResult()
        {
            CompetitionRunTimings timings = original.UpdateTimingsFrom(intermediateTime1, intermediateTime2,
                intermediateTime3, finishTime);

            return
                original.ChangeTimings(timings)
                    .ChangeFaultCount(FaultCount)
                    .ChangeRefusalCount(RefusalCount)
                    .ChangeIsEliminated(IsEliminated);
        }
    }
}