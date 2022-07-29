using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// Performs calculations to support competitor run result ordering.
/// </summary>
public sealed class CompetitorAssessmentCalculator
{
    private readonly CompetitionRunResult competitor;
    private readonly CompetitionClassModel modelSnapshot;

    /// <summary>
    /// Gets the time at which this competitor passed the finish gate.
    /// </summary>
    /// <value>
    /// The measured time, or <see cref="TimeSpan.Zero" /> when competitor did not pass the finish gate.
    /// </value>
    public TimeSpan FinishTime
    {
        get
        {
            if (competitor.Timings?.FinishTime != null)
            {
                TimeSpanWithAccuracy elapsed = competitor.Timings.FinishTime.ElapsedSince(competitor.Timings.StartTime);
                return elapsed.TimeValue;
            }

            return TimeSpan.Zero;
        }
    }

    /// <summary>
    /// Gets the amount of time that FinishTime is above Standard Course Time.
    /// </summary>
    /// <value>
    /// The amount of time, or <see cref="TimeSpan.Zero" /> when competitor finished within Standard Course Time -or- did not pass the finish gate.
    /// </value>
    public TimeSpan OverrunTime
    {
        get
        {
            if (modelSnapshot.ClassInfo.StandardCourseTime == null)
            {
                return TimeSpan.Zero;
            }

            TimeSpan delta = FinishTime - modelSnapshot.ClassInfo.StandardCourseTime.Value;
            return ZeroWhenNegative(delta);
        }
    }

    /// <summary>
    /// Gets the total amount of non-measured additional time, consisting of the time for faults, refusals and Standard Course Time overrun. This does not
    /// include the measured time at which competitor passed the finish gate.
    /// </summary>
    /// <value>
    /// The total amount of non-measured additional time.
    /// </value>
    public TimeSpan PenaltyTime => FaultRefusalTime + OverrunTime;

    /// <summary>
    /// Gets the amount of time, due to faults and refusals.
    /// </summary>
    /// <value>
    /// The combined time for faults and refusals.
    /// </value>
    public TimeSpan FaultRefusalTime => TimeSpan.FromSeconds(competitor.FaultCount + competitor.RefusalCount);

    public CompetitorAssessmentCalculator(CompetitionRunResult competitor, CompetitionClassModel modelSnapshot)
    {
        Guard.NotNull(competitor, nameof(competitor));
        Guard.NotNull(modelSnapshot, nameof(modelSnapshot));
        this.competitor = competitor;
        this.modelSnapshot = modelSnapshot;
    }

    private static TimeSpan ZeroWhenNegative(TimeSpan time)
    {
        return time < TimeSpan.Zero ? TimeSpan.Zero : time;
    }
}
