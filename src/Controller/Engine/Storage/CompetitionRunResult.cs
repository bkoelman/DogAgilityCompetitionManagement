using System.Text;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage;

/// <summary>
/// The outcome of a competition run, achieved by a single competitor.
/// </summary>
/// <remarks>
/// Deeply immutable by design to allow for safe cross-thread member access.
/// </remarks>
public class CompetitionRunResult
{
    public const int FaultStepSize = 5;
    public const int MaxFaultValue = 95;
    public const int RefusalStepSize = 5;
    public const int EliminationThreshold = 3;
    public const int MaxRefusalsValue = RefusalStepSize * EliminationThreshold;

    public Competitor Competitor { get; }
    public CompetitionRunTimings? Timings { get; }
    public int FaultCount { get; }
    public int RefusalCount { get; }
    public bool IsEliminated { get; }
    public int Placement { get; }

    public string PlacementText => Placement > 0 ? Placement.ToString() : string.Empty;

    public bool HasCompleted => IsEliminated || HasFinished;

    public bool HasFinished => Timings?.FinishTime != null;

    public CompetitionRunResult(Competitor competitor)
    {
        Guard.NotNull(competitor, nameof(competitor));

        Competitor = competitor;
    }

    protected CompetitionRunResult(Competitor competitor, CompetitionRunTimings? timings, int faultCount, int refusalCount, bool isEliminated, int placement)
        : this(competitor)
    {
        Timings = timings;
        FaultCount = faultCount;
        RefusalCount = refusalCount;
        IsEliminated = isEliminated;
        Placement = placement;
    }

    public CompetitionRunResult ChangeCompetitor(Competitor competitor)
    {
        Guard.NotNull(competitor, nameof(competitor));

        return new CompetitionRunResult(competitor, Timings, FaultCount, RefusalCount, IsEliminated, Placement);
    }

    public CompetitionRunResult ChangeTimings(CompetitionRunTimings? timings)
    {
        return new CompetitionRunResult(Competitor, timings, FaultCount, RefusalCount, IsEliminated, Placement);
    }

    public CompetitionRunResult ChangeFaultCount(int faultCount)
    {
        AssertFaultCountIsValid(faultCount);

        return new CompetitionRunResult(Competitor, Timings, faultCount, RefusalCount, IsEliminated, Placement);
    }

    [AssertionMethod]
    public static void AssertFaultCountIsValid(int faultCount)
    {
        if (faultCount < 0 || faultCount > MaxFaultValue || faultCount % FaultStepSize != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(faultCount), faultCount,
                $"faultCount must be in range [0-{MaxFaultValue}] and dividable by {FaultStepSize}.");
        }
    }

    public CompetitionRunResult ChangeRefusalCount(int refusalCount)
    {
        AssertRefusalCountIsValid(refusalCount);

        return new CompetitionRunResult(Competitor, Timings, FaultCount, refusalCount, IsEliminated, Placement);
    }

    [AssertionMethod]
    public static void AssertRefusalCountIsValid(int refusalCount)
    {
        if (refusalCount < 0 || refusalCount > MaxRefusalsValue || refusalCount % RefusalStepSize != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(refusalCount), refusalCount,
                $"refusalCount must be in range [0-{MaxRefusalsValue}] and dividable by {RefusalStepSize}.");
        }
    }

    public CompetitionRunResult ChangeIsEliminated(bool isEliminated)
    {
        return new CompetitionRunResult(Competitor, Timings, FaultCount, RefusalCount, isEliminated, Placement);
    }

    public CompetitionRunResult ChangePlacement(int placement)
    {
        return new CompetitionRunResult(Competitor, Timings, FaultCount, RefusalCount, IsEliminated, placement);
    }

    public CompetitionRunTimings UpdateTimingsFrom(TimeSpanWithAccuracy? intermediateTime1, TimeSpanWithAccuracy? intermediateTime2,
        TimeSpanWithAccuracy? intermediateTime3, TimeSpanWithAccuracy? finishTime)
    {
        RecordedTime startTime = CreateBestPossibleStartTime();

        CompetitionRunTimings timings = Timings ?? new CompetitionRunTimings(startTime);

        // @formatter:keep_existing_linebreaks true

        timings = timings
            .ChangeIntermediateTime1(TryCreateRecordedTime(startTime, intermediateTime1))
            .ChangeIntermediateTime2(TryCreateRecordedTime(startTime, intermediateTime2))
            .ChangeIntermediateTime3(TryCreateRecordedTime(startTime, intermediateTime3))
            .ChangeFinishTime(TryCreateRecordedTime(startTime, finishTime));

        // @formatter:keep_existing_linebreaks restore

        return timings;
    }

    private RecordedTime CreateBestPossibleStartTime()
    {
        // Note: In case we have no start time, we generate one here (with high precision). A low-precision elapsed
        // time is caused by either one or both times to be low precision. Although we cannot know the
        // precision of start time here, the nett effect when the precision of an elapsed time is recalculated will
        // be the same as long as we assume that the start time was high precision.
        RecordedTime startTime = Timings?.StartTime ?? new RecordedTime(TimeSpan.Zero, SystemContext.UtcNow());
        return startTime;
    }

    public CompetitionRunTimings UpdateFinishTimeFrom(TimeSpanWithAccuracy? finishTime)
    {
        RecordedTime startTime = CreateBestPossibleStartTime();

        CompetitionRunTimings timings = Timings ?? new CompetitionRunTimings(startTime);
        timings = timings.ChangeFinishTime(TryCreateRecordedTime(startTime, finishTime));

        return timings;
    }

    private static RecordedTime? TryCreateRecordedTime(RecordedTime startTime, TimeSpanWithAccuracy? elapsed)
    {
        return elapsed != null ? startTime.Add(elapsed.Value) : null;
    }

    [Pure]
    public override string ToString()
    {
        var textBuilder = new StringBuilder();
        textBuilder.Append(Competitor);

        if (IsEliminated)
        {
            textBuilder.Append(" has been eliminated");
        }
        else if (Timings?.FinishTime != null)
        {
            textBuilder.Append(" finished at ");
            TimeSpanWithAccuracy finishTime = Timings.FinishTime.ElapsedSince(Timings.StartTime);
            textBuilder.Append(finishTime);

            if (FaultCount > 0)
            {
                textBuilder.Append(" with ");
                textBuilder.Append(FaultCount);
                textBuilder.Append(" faults");
            }

            if (RefusalCount > 0)
            {
                textBuilder.Append(FaultCount > 0 ? " and " : " with ");
                textBuilder.Append(RefusalCount);
                textBuilder.Append(" refusals");
            }
        }

        return textBuilder.ToString();
    }

    public static bool AreEquivalent(CompetitionRunResult? first, CompetitionRunResult? second)
    {
        return EqualitySupport.EqualsWithNulls(first, second, CompetitorRunResultsAreEqual);
    }

    public static bool AreEquivalentRun(CompetitionRunResult? first, CompetitionRunResult? second)
    {
        return EqualitySupport.EqualsWithNulls(first, second, RunResultsAreEqual);
    }

    private static bool CompetitorRunResultsAreEqual(CompetitionRunResult first, CompetitionRunResult second)
    {
        return CompetitorsAreEqual(first, second) && RunResultsAreEqual(first, second);
    }

    private static bool CompetitorsAreEqual(CompetitionRunResult first, CompetitionRunResult second)
    {
        return first.Competitor == second.Competitor;
    }

    private static bool RunResultsAreEqual(CompetitionRunResult first, CompetitionRunResult second)
    {
        return EqualitySupport.EqualsWithNulls(first.Timings, second.Timings, FinishTimesAreEqual) && first.FaultCount == second.FaultCount &&
            first.RefusalCount == second.RefusalCount && first.IsEliminated == second.IsEliminated;
    }

    private static bool FinishTimesAreEqual(CompetitionRunTimings firstTimings, CompetitionRunTimings secondTimings)
    {
        return firstTimings.FinishTime == secondTimings.FinishTime;
    }
}
