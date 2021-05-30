using System;
using System.Diagnostics;
using System.Reflection;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Logs the execution duration of a code block for diagnostics purposes.
    /// </summary>
    public sealed class CodeTimer : IDisposable
    {
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private readonly string text;
        private readonly Stopwatch stopwatch = new();

        public CodeTimer(string text)
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
