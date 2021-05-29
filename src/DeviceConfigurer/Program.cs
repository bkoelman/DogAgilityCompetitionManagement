using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace DogAgilityCompetition.DeviceConfigurer
{
    /// <summary>
    /// The application entry point.
    /// </summary>
    internal static class Program
    {
        [NotNull]
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main([NotNull] [ItemNotNull] string[] args)
        {
            //CirceSpecExamples.DumpScenarios();

            StartupArguments startupArguments = StartupArguments.Parse(args);

            if (startupArguments != null)
            {
                ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo("log4net.config"));

                Log.Info("Application started.");

                var process = new MainProcess(startupArguments);
                process.Run();

                Log.Info("Application ended.");
            }
        }
    }
}
