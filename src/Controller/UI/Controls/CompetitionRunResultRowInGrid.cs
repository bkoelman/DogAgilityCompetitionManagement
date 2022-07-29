using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.UI.Controls;

/// <summary>
/// Used for data binding in <see cref="CompetitionRunResultsGrid" />.
/// </summary>
public sealed class CompetitionRunResultRowInGrid
{
    private readonly CompetitionRunResult original;

    private TimeSpanWithAccuracy? intermediateTime1;
    private TimeSpanWithAccuracy? intermediateTime2;
    private TimeSpanWithAccuracy? intermediateTime3;
    private TimeSpanWithAccuracy? finishTime;
    private int faultCount;
    private int refusalCount;

    public int CompetitorNumber { get; private set; }
    public string HandlerName { get; }
    public string DogName { get; }
    public string? CountryCode { get; private set; }

    public string IntermediateTime1
    {
        get => intermediateTime1?.ToString() ?? string.Empty;
        set => intermediateTime1 = ParseForceUserEdited(value);
    }

    public string IntermediateTime2
    {
        get => intermediateTime2?.ToString() ?? string.Empty;
        set => intermediateTime2 = ParseForceUserEdited(value);
    }

    public string IntermediateTime3
    {
        get => intermediateTime3?.ToString() ?? string.Empty;
        set => intermediateTime3 = ParseForceUserEdited(value);
    }

    public string FinishTime
    {
        get => finishTime?.ToString() ?? string.Empty;
        set => finishTime = ParseForceUserEdited(value);
    }

    public int FaultCount
    {
        get => faultCount;
        set
        {
            CompetitionRunResult.AssertFaultCountIsValid(value);
            faultCount = value;
        }
    }

    public int RefusalCount
    {
        get => refusalCount;
        set
        {
            CompetitionRunResult.AssertRefusalCountIsValid(value);
            refusalCount = value;
        }
    }

    public bool IsEliminated { get; set; }
    public string PlacementText { get; private set; }

    private CompetitionRunResultRowInGrid(CompetitionRunResult original, string handlerName, string dogName)
    {
        Guard.NotNull(original, nameof(original));
        Guard.NotNullNorEmpty(handlerName, nameof(handlerName));
        Guard.NotNullNorEmpty(dogName, nameof(dogName));

        this.original = original;
        HandlerName = handlerName;
        DogName = dogName;
        PlacementText = string.Empty;
    }

    private static TimeSpanWithAccuracy? ParseForceUserEdited(string? timeValue)
    {
        TimeSpanWithAccuracy? result = TimeSpanWithAccuracy.FromString(timeValue);
        return result?.ChangeAccuracy(TimeAccuracy.UserEdited);
    }

    public static CompetitionRunResultRowInGrid FromCompetitionRunResult(CompetitionRunResult source)
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

    public CompetitionRunResult ToCompetitionRunResult()
    {
        CompetitionRunTimings timings = original.UpdateTimingsFrom(intermediateTime1, intermediateTime2, intermediateTime3, finishTime);

        // @formatter:keep_existing_linebreaks true

        return original
            .ChangeTimings(timings)
            .ChangeFaultCount(FaultCount)
            .ChangeRefusalCount(RefusalCount)
            .ChangeIsEliminated(IsEliminated);

        // @formatter:keep_existing_linebreaks restore
    }
}
