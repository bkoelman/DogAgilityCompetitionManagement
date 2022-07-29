using System.Reflection;

namespace DogAgilityCompetition.Circe;

/// <summary>
/// Writes logging for locks that are waited for, obtained and released. Intended for analyzing multi-threading issues, such as race conditions and
/// deadlocks.
/// </summary>
public sealed class LockTracker : IDisposable
{
    private const string BlockingToObtainStateLock = ": Blocking to obtain state lock.";
    private const string StateLockObtained = ": State lock obtained.";
    private const string StateLockReleased = ": State lock released.";

    private readonly ISystemLogger log;
    private readonly string source;

    public LockTracker(ISystemLogger log, MethodBase source)
        : this(log, GetNameOfMethod(source))
    {
    }

    public LockTracker(ISystemLogger log, string source)
    {
        Guard.NotNull(log, nameof(log));
        Guard.NotNullNorEmpty(source, nameof(source));

        this.log = log;
        this.source = source;

        Acquiring();
    }

    private static string GetNameOfMethod(MethodBase source)
    {
        Guard.NotNull(source, nameof(source));

        return source.Name;
    }

    private void Acquiring()
    {
        log.Debug(source + BlockingToObtainStateLock);
    }

    public void Acquired()
    {
        log.Debug(source + StateLockObtained);
    }

    private void Released()
    {
        log.Debug(source + StateLockReleased);
    }

    public void Dispose()
    {
        Released();
    }

    public static bool IsLockMessage(string message)
    {
        Guard.NotNull(message, nameof(message));

        // @formatter:keep_existing_linebreaks true

        return
            message.IndexOf(BlockingToObtainStateLock, StringComparison.Ordinal) != -1 ||
            message.IndexOf(StateLockObtained, StringComparison.Ordinal) != -1 ||
            message.IndexOf(StateLockReleased, StringComparison.Ordinal) != -1;

        // @formatter:keep_existing_linebreaks restore
    }
}
