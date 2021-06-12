using DogAgilityCompetition.Circe;

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
        public RecordedTime StartTime { get; }
        public RecordedTime? IntermediateTime1 { get; }
        public RecordedTime? IntermediateTime2 { get; }
        public RecordedTime? IntermediateTime3 { get; }
        public RecordedTime? FinishTime { get; }

        public CompetitionRunTimings(RecordedTime startTime)
        {
            Guard.NotNull(startTime, nameof(startTime));

            StartTime = startTime;
        }

        private CompetitionRunTimings(RecordedTime startTime, RecordedTime? intermediateTime1, RecordedTime? intermediateTime2, RecordedTime? intermediateTime3,
            RecordedTime? finishTime)
            : this(startTime)
        {
            IntermediateTime1 = intermediateTime1;
            IntermediateTime2 = intermediateTime2;
            IntermediateTime3 = intermediateTime3;
            FinishTime = finishTime;
        }

        public CompetitionRunTimings ChangeIntermediateTime1(RecordedTime? intermediateTime1)
        {
            return new(StartTime, intermediateTime1, IntermediateTime2, IntermediateTime3, FinishTime);
        }

        public CompetitionRunTimings ChangeIntermediateTime2(RecordedTime? intermediateTime2)
        {
            return new(StartTime, IntermediateTime1, intermediateTime2, IntermediateTime3, FinishTime);
        }

        public CompetitionRunTimings ChangeIntermediateTime3(RecordedTime? intermediateTime3)
        {
            return new(StartTime, IntermediateTime1, IntermediateTime2, intermediateTime3, FinishTime);
        }

        public CompetitionRunTimings ChangeFinishTime(RecordedTime? finishTime)
        {
            return new(StartTime, IntermediateTime1, IntermediateTime2, IntermediateTime3, finishTime);
        }
    }
}
