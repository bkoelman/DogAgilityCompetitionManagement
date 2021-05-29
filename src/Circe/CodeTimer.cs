using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Logs the execution duration of a code block for diagnostics purposes.
    /// </summary>
    public sealed class CodeTimer : IDisposable
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private readonly string text;

        [NotNull]
        private readonly Stopwatch stopwatch = new();

        public CodeTimer([NotNull] string text)
        {
            Guard.NotNullNorEmpty(text, nameof(text));

            this.text = text;
            stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            Log.Debug($"Duration of {text}: {stopwatch.ElapsedMilliseconds} msec");
        }
    }
}
