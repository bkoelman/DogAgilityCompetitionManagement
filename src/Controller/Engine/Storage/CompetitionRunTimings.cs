using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// The recorded times for a competitor, measured during a competition run.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class CompetitionRunTimings
    {
        [NotNull]
        public RecordedTime StartTime { get; }

        [CanBeNull]
        public RecordedTime IntermediateTime1 { get; }

        [CanBeNull]
        public RecordedTime IntermediateTime2 { get; }

        [CanBeNull]
        public RecordedTime IntermediateTime3 { get; }

        [CanBeNull]
        public RecordedTime FinishTime { get; }

        public CompetitionRunTimings([NotNull] RecordedTime startTime)
        {
            Guard.NotNull(startTime, nameof(startTime));

            StartTime = startTime;
        }

        private CompetitionRunTimings([NotNull] RecordedTime startTime, [CanBeNull] RecordedTime intermediateTime1, [CanBeNull] RecordedTime intermediateTime2,
            [CanBeNull] RecordedTime intermediateTime3, [CanBeNull] RecordedTime finishTime)
            : this(startTime)
        {
            IntermediateTime1 = intermediateTime1;
            IntermediateTime2 = intermediateTime2;
            IntermediateTime3 = intermediateTime3;
            FinishTime = finishTime;
        }

        [NotNull]
        public CompetitionRunTimings ChangeIntermediateTime1([CanBeNull] RecordedTime intermediateTime1)
        {
            return new(StartTime, intermediateTime1, IntermediateTime2, IntermediateTime3, FinishTime);
        }

        [NotNull]
        public CompetitionRunTimings ChangeIntermediateTime2([CanBeNull] RecordedTime intermediateTime2)
        {
            return new(StartTime, IntermediateTime1, intermediateTime2, IntermediateTime3, FinishTime);
        }

        [NotNull]
        public CompetitionRunTimings ChangeIntermediateTime3([CanBeNull] RecordedTime intermediateTime3)
        {
            return new(StartTime, IntermediateTime1, IntermediateTime2, intermediateTime3, FinishTime);
        }

        [NotNull]
        public CompetitionRunTimings ChangeFinishTime([CanBeNull] RecordedTime finishTime)
        {
            return new(StartTime, IntermediateTime1, IntermediateTime2, IntermediateTime3, finishTime);
        }
    }
}
