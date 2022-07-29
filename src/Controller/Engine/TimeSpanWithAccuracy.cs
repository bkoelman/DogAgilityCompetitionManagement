using System.Globalization;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// Represents a <see cref="TimeSpan" />, rounded to whole milliseconds, along with a flag that indicates where the time value came from.
/// </summary>
/// <remarks>
/// Deeply immutable by design to allow for safe cross-thread member access.
/// </remarks>
public readonly struct TimeSpanWithAccuracy : IFormattable, IEquatable<TimeSpanWithAccuracy>
{
    private const string TimeRegexFormat = @"^(?<Seconds>[0-9]{1,3})DecimalSeparatorPlaceholder(?<Milliseconds>[0-9][0-9][0-9])(?<AccuracySymbol>[~*]?)$";

    /// <summary>
    /// Time value, in whole milliseconds.
    /// </summary>
    public TimeSpan TimeValue { get; }

    public TimeAccuracy Accuracy { get; }

    public TimeSpanWithAccuracy(TimeSpan timeValue, TimeAccuracy accuracy)
    {
        TimeValue = GetTimeValueRoundedToWholeMilliseconds(timeValue);
        Accuracy = accuracy;
    }

    private static TimeSpan GetTimeValueRoundedToWholeMilliseconds(TimeSpan source)
    {
        double wholeSecondsTruncated = Math.Truncate(source.TotalSeconds);
        double ticksRemainingForMilliseconds = source.Ticks - wholeSecondsTruncated * TimeSpan.TicksPerSecond;

        double fractionalMilliseconds = ticksRemainingForMilliseconds / TimeSpan.TicksPerMillisecond;
        double wholeMillisecondsRounded = Math.Round(fractionalMilliseconds, MidpointRounding.AwayFromZero);

        return TimeSpan.FromSeconds(wholeSecondsTruncated).Add(TimeSpan.FromMilliseconds(wholeMillisecondsRounded));
    }

    public TimeSpanWithAccuracy ChangeAccuracy(TimeAccuracy timeAccuracy)
    {
        return new TimeSpanWithAccuracy(TimeValue, timeAccuracy);
    }

    public static TimeSpanWithAccuracy? FromString(string? text, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        string trimmed = text.Trim();

        Regex timeRegex = CreateTimeRegexFor(formatProvider);
        Match match = timeRegex.Match(trimmed);

        if (match.Success)
        {
            string accuracySymbol = match.Groups["AccuracySymbol"].Value;
            string seconds = match.Groups["Seconds"].Value;
            string milliseconds = match.Groups["Milliseconds"].Value;

            TimeSpan timeValue = TimeSpan.FromSeconds(int.Parse(seconds)) + TimeSpan.FromMilliseconds(int.Parse(milliseconds));

            TimeAccuracy accuracy = GetAccuracyForSymbol(accuracySymbol);
            return new TimeSpanWithAccuracy(timeValue, accuracy);
        }

        throw new FormatException($"Time value '{text}' is invalid. Use format: s.mmm.");
    }

    private static Regex CreateTimeRegexFor(IFormatProvider? formatProvider)
    {
        formatProvider ??= NumberFormatInfo.InvariantInfo;
        var formatInfo = NumberFormatInfo.GetInstance(formatProvider);
        string separator = Regex.Escape(formatInfo.NumberDecimalSeparator);

        string timeRegexPattern = TimeRegexFormat.Replace("DecimalSeparatorPlaceholder", separator);
        var timeRegex = new Regex(timeRegexPattern);
        return timeRegex;
    }

    private static TimeAccuracy GetAccuracyForSymbol(string? symbol)
    {
        switch (symbol)
        {
            case "~":
                return TimeAccuracy.LowPrecision;
            case "*":
                return TimeAccuracy.UserEdited;
            default:
                return TimeAccuracy.HighPrecision;
        }
    }

    [Pure]
    public override string ToString()
    {
        return ToString(null, null);
    }

    [Pure]
    public string ToString(string? format /* discarded */, IFormatProvider? formatProvider)
    {
        formatProvider ??= NumberFormatInfo.InvariantInfo;
        var formatInfo = NumberFormatInfo.GetInstance(formatProvider);

        double secondsTruncated = Math.Truncate(TimeValue.TotalSeconds);
        string timeString = $"{secondsTruncated:##0}{formatInfo.NumberDecimalSeparator}{TimeValue.Milliseconds:000}";

        switch (Accuracy)
        {
            case TimeAccuracy.HighPrecision:
                return timeString;
            case TimeAccuracy.LowPrecision:
                return timeString + "~";
            case TimeAccuracy.UserEdited:
                return timeString + "*";
            default:
                throw ExceptionFactory.CreateNotSupportedExceptionFor(Accuracy);
        }
    }

    [Pure]
    public bool Equals(TimeSpanWithAccuracy other)
    {
        return other.TimeValue == TimeValue && other.Accuracy == Accuracy;
    }

    [Pure]
    public override bool Equals(object? obj)
    {
        return obj is TimeSpanWithAccuracy timeSpanWithAccuracy && Equals(timeSpanWithAccuracy);
    }

    [Pure]
    public override int GetHashCode()
    {
        return TimeValue.GetHashCode() ^ Accuracy.GetHashCode();
    }

    [Pure]
    public static bool operator ==(TimeSpanWithAccuracy left, TimeSpanWithAccuracy right)
    {
        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(TimeSpanWithAccuracy left, TimeSpanWithAccuracy right)
    {
        return !(left == right);
    }
}
