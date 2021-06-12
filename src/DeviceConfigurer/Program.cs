using System.IO;
using System.Reflection;
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
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            //CirceSpecExamples.DumpScenarios();

            StartupArguments? startupArguments = StartupArguments.Parse(args);

            if (startupArguments != null)
            {
                ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo("DogAgilityCompetition.DeviceConfigurer.log4net.config"));

                Log.Info("Application started.");

                var process = new MainProcess(startupArguments);
                process.Run();

                Log.Info("Application ended.");
            }
        }
    }
}
