using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Forms;
using JetBrains.Annotations;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace DogAgilityCompetition.Controller
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
        [STAThread]
        private static void Main()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo("log4net.config"));

            Log.Info("Application started.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            using (SystemPowerManagementProvider.InStateDisabled)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }

            Log.Info("Application ended.");
        }

        private static void TaskSchedulerOnUnobservedTaskException([CanBeNull] object sender, [NotNull] UnobservedTaskExceptionEventArgs e)
        {
            Log.Error("Unhandled exception in TaskScheduler.", e.Exception);
            e.SetObserved();
        }

        private static void CurrentDomainOnUnhandledException([CanBeNull] object sender, [NotNull] UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Error($"Unhandled managed exception in AppDomain (isTerminating={e.IsTerminating})", ex);
            }
            else
            {
                Log.Error($"Unhandled unmanaged exception in AppDomain (isTerminating={e.IsTerminating}): {e.ExceptionObject}");
            }
        }
    }
}
