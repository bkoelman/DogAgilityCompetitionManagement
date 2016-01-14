using System;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.MediatorEmulator.UI.Forms;
using JetBrains.Annotations;
using log4net;

namespace DogAgilityCompetition.MediatorEmulator
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
        private static void Main([NotNull] [ItemNotNull] string[] args)
        {
            Log.Info("Application started.");

            StartupArguments startupArguments = StartupArguments.Parse(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EmulatorForm(startupArguments));

            Log.Info("Application ended.");
        }
    }
}