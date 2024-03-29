using System.Globalization;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// A time value, as reported by a hardware device or software emulation.
/// </summary>
/// <remarks>
/// Deeply immutable by design to allow for safe cross-thread member access.
/// </remarks>
public sealed class RecordedTime : IEquatable<RecordedTime>
{
    public TimeSpan? HardwareSynchronizedTime { get; }
    public DateTime SoftwareTimeInUtc { get; }
    public TimeAccuracy Accuracy { get; }

    public RecordedTime(TimeSpan? hardwareSynchronizedTime, DateTime softwareTimeInUtc, TimeAccuracy? accuracy = null)
    {
        if (accuracy == null)
        {
            Accuracy = hardwareSynchronizedTime == null ? TimeAccuracy.LowPrecision : TimeAccuracy.HighPrecision;
        }
        else if (accuracy == TimeAccuracy.HighPrecision && hardwareSynchronizedTime == null)
        {
            Accuracy = TimeAccuracy.LowPrecision;
        }
        else
        {
            Accuracy = accuracy.Value;
        }

        HardwareSynchronizedTime = Accuracy == TimeAccuracy.UserEdited ? null : hardwareSynchronizedTime;
        SoftwareTimeInUtc = GetTimeValueRoundedToWholeMilliseconds(softwareTimeInUtc);
    }

    private static DateTime GetTimeValueRoundedToWholeMilliseconds(DateTime source)
    {
        return new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, source.Millisecond, source.Kind);
    }

    public TimeSpanWithAccuracy ElapsedSince(RecordedTime other)
    {
        Guard.NotNull(other, nameof(other));

        TimeSpan softTimeElapsed = SoftwareTimeInUtc - other.SoftwareTimeInUtc;

        if (HardwareSynchronizedTime != null && other.HardwareSynchronizedTime != null)
        {
            TimeSpan hardTimeElapsed = HardwareSynchronizedTime.Value - other.HardwareSynchronizedTime.Value;

            // When device restarts, it takes at least 4 seconds before it is reconnected.
            double measureDelta = Math.Abs(softTimeElapsed.TotalSeconds - hardTimeElapsed.TotalSeconds);
            bool isValidSensorTime = measureDelta < 4 && hardTimeElapsed >= TimeSpan.Zero;

            if (isValidSensorTime)
            {
                TimeAccuracy effectiveAccuracy = Combine(Accuracy, other.Accuracy, true);
                return new TimeSpanWithAccuracy(hardTimeElapsed, effectiveAccuracy);
            }
        }

        TimeAccuracy nonHighAccuracy = Combine(Accuracy, other.Accuracy, false);
        return new TimeSpanWithAccuracy(softTimeElapsed, nonHighAccuracy);
    }

    private static TimeAccuracy Combine(TimeAccuracy first, TimeAccuracy second, bool allowHighResolutionMeasure)
    {
        if (first == TimeAccuracy.UserEdited || second == TimeAccuracy.UserEdited)
        {
            return TimeAccuracy.UserEdited;
        }

        if (first == TimeAccuracy.LowPrecision || second == TimeAccuracy.LowPrecision)
        {
            return TimeAccuracy.LowPrecision;
        }

        return allowHighResolutionMeasure ? TimeAccuracy.HighPrecision : TimeAccuracy.LowPrecision;
    }

    public RecordedTime Add(TimeSpanWithAccuracy offset)
    {
        TimeSpan? hardwareTime = offset.Accuracy == TimeAccuracy.HighPrecision ? HardwareSynchronizedTime + offset.TimeValue : null;
        TimeAccuracy effectiveAccuracy = Combine(Accuracy, offset.Accuracy, true);
        DateTime softwareTimeInUtc = SoftwareTimeInUtc + offset.TimeValue;

        return new RecordedTime(hardwareTime, softwareTimeInUtc, effectiveAccuracy);
    }

    [Pure]
    public bool Equals(RecordedTime? other)
    {
        return !ReferenceEquals(other, null) && HardwareSynchronizedTime == other.HardwareSynchronizedTime && SoftwareTimeInUtc == other.SoftwareTimeInUtc &&
            Accuracy == other.Accuracy;
    }

    [Pure]
    public override bool Equals(object? obj)
    {
        return Equals(obj as RecordedTime);
    }

    [Pure]
    public override int GetHashCode()
    {
        return HardwareSynchronizedTime.GetHashCode() ^ SoftwareTimeInUtc.GetHashCode() ^ Accuracy.GetHashCode();
    }

    [Pure]
    public override string ToString()
    {
        string timeValue = HardwareSynchronizedTime?.ToString("hh\\:mm\\:ss\\.fffffff", CultureInfo.InvariantCulture) ??
            SoftwareTimeInUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);

        switch (Accuracy)
        {
            case TimeAccuracy.UserEdited:
                return $"{timeValue}*";
            case TimeAccuracy.LowPrecision:
                return $"{timeValue}~";
            default:
                return timeValue;
        }
    }

    [Pure]
    public static bool operator ==(RecordedTime? left, RecordedTime? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (ReferenceEquals(left, null))
        {
            return false;
        }

        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(RecordedTime? left, RecordedTime? right)
    {
        return !(left == right);
    }
}
