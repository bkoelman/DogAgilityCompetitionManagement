using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.MediatorEmulator.UI.Forms;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace DogAgilityCompetition.MediatorEmulator
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
        [STAThread]
        private static void Main(string[] args)
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo("log4net.config"));

            Log.Info("Application started.");

            StartupArguments startupArguments = StartupArguments.Parse(args);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EmulatorForm(startupArguments));

            Log.Info("Application ended.");
        }
    }
}
