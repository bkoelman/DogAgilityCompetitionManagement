using System;
using System.Reflection;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Writes logging for locks that are waited for, obtained and released. Intended for analyzing multi-threading issues, such as race conditions and
    /// deadlocks.
    /// </summary>
    public sealed class LockTracker : IDisposable
    {
        private const string BlockingToObtainStateLock = ": Blocking to obtain state lock.";
        private const string StateLockObtained = ": State lock obtained.";
        private const string StateLockReleased = ": State lock released.";

        [NotNull]
        private readonly ISystemLogger log;

        [NotNull]
        private readonly string source;

        public LockTracker([NotNull] ISystemLogger log, [NotNull] MethodBase source)
            : this(log, GetNameOfMethod(source))
        {
        }

        public LockTracker([NotNull] ISystemLogger log, [NotNull] string source)
        {
            Guard.NotNull(log, nameof(log));
            Guard.NotNullNorEmpty(source, nameof(source));

            this.log = log;
            this.source = source;

            Acquiring();
        }

        [NotNull]
        private static string GetNameOfMethod([NotNull] MethodBase source)
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

        public static bool IsLockMessage([NotNull] string message)
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
}
