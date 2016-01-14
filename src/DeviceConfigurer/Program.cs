using System.Reflection;
using JetBrains.Annotations;
using log4net;

namespace DogAgilityCompetition.DeviceConfigurer
{
    /// <summary>
    /// The application entry point.
    /// </summary>
    internal static class Program
    {
        [NotNull]
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main([NotNull] [ItemNotNull] string[] args)
        {
            //CirceSpecExamples.DumpScenarios();

            StartupArguments startupArguments = StartupArguments.Parse(args);
            if (startupArguments != null)
            {
                Log.Info("Application started.");

                var process = new MainProcess(startupArguments);
                process.Run();

                Log.Info("Application ended.");
            }
        }
    }
}