using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace DogAgilityCompetition.Controller.UI
{
    /// <summary>
    /// Log4net receiver that writes messages to a <see cref="TextBox" />.
    /// </summary>
    public sealed class TextBoxAppender : AppenderSkeleton
    {
        [NotNull]
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private readonly FreshBoolean isFrozen = new(false);

        [NotNull]
        private readonly FreshEnum<TextBoxAppenderMode> mode = new(TextBoxAppenderMode.All);

        [NotNull]
        private readonly FreshEnum<TextBoxAppenderSwitches> switches = new(TextBoxAppenderSwitches.None);

        [CanBeNull]
        private TextBox logTextBox;

        public static bool IsFrozen
        {
            get
            {
                TextBoxAppender appender = GetActiveAppenders().FirstOrDefault();
                return appender != null && appender.isFrozen.Value;
            }
            set
            {
                foreach (TextBoxAppender appender in GetActiveAppenders())
                {
                    appender.isFrozen.Value = value;
                }
            }
        }

        public static TextBoxAppenderMode Mode
        {
            get
            {
                TextBoxAppender appender = GetActiveAppenders().FirstOrDefault();
                return appender?.mode.Value ?? TextBoxAppenderMode.All;
            }
            set
            {
                foreach (TextBoxAppender appender in GetActiveAppenders())
                {
                    appender.mode.Value = value;
                }
            }
        }

        public static TextBoxAppenderSwitches Switches
        {
            get
            {
                TextBoxAppender appender = GetActiveAppenders().FirstOrDefault();
                return appender?.switches.Value ?? TextBoxAppenderSwitches.None;
            }
            set
            {
                foreach (TextBoxAppender appender in GetActiveAppenders())
                {
                    appender.switches.Value = value;
                }
            }
        }

        public static void Subscribe([NotNull] TextBox textBox)
        {
            Guard.NotNull(textBox, nameof(textBox));

            foreach (TextBoxAppender appender in GetActiveAppenders())
            {
                appender.SetTextBox(textBox);
            }
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<TextBoxAppender> GetActiveAppenders()
        {
            return Log.Logger.Repository.GetAppenders().OfType<TextBoxAppender>();
        }

        private void SetTextBox([CanBeNull] TextBox textBox)
        {
            logTextBox = textBox;
        }

        protected override void Append([NotNull] LoggingEvent loggingEvent)
        {
            try
            {
                if (!isFrozen.Value && logTextBox is { Created: true })
                {
                    bool include = ApplyFilter(loggingEvent);

                    if (include)
                    {
                        logTextBox.EnsureOnMainThread(() =>
                        {
                            string message = RenderLoggingEvent(loggingEvent);
                            logTextBox.AppendText(message);
                        });
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            // Reason: In case an unhandled exception is thrown (Task or AppDomain), it ultimately gets logged.
            // This results in an attempt to also write to the TextBox, which may not be possible
            // at that time. To prevent an endless loop (this method causing a new AppDomain/Task exception)
            // and to ensure the TextBox issues does not prevent that the unhandled exception gets written 
            // to log files, we silently swallow any exceptions here.
            catch (Exception)
            {
            }
        }

        private bool ApplyFilter([NotNull] LoggingEvent loggingEvent)
        {
            if ((switches.Value & TextBoxAppenderSwitches.HideLockSleep) != 0 && IsLockSleepLogEvent(loggingEvent))
            {
                return false;
            }

            switch (mode.Value)
            {
                case TextBoxAppenderMode.Packets:
                    return IsPacketLogEvent(loggingEvent);
                case TextBoxAppenderMode.Network:
                    return IsNetworkLogEvent(loggingEvent);
                case TextBoxAppenderMode.NonNetwork:
                    return !IsNetworkLogEvent(loggingEvent);
                default:
                    return true;
            }
        }

        private static bool IsPacketLogEvent([NotNull] LoggingEvent loggingEvent)
        {
            return loggingEvent.RenderedMessage.IndexOf("=> RAW:", StringComparison.Ordinal) != -1 ||
                loggingEvent.RenderedMessage.IndexOf("<= RAW:", StringComparison.Ordinal) != -1;
        }

        private static bool IsNetworkLogEvent([NotNull] LoggingEvent loggingEvent)
        {
            return IsPacketLogEvent(loggingEvent) || loggingEvent.LoggerName.EndsWith(".SessionGuard", StringComparison.Ordinal) ||
                loggingEvent.LoggerName.EndsWith(".DeviceTracker", StringComparison.Ordinal) ||
                loggingEvent.LoggerName.EndsWith(".ActionQueue", StringComparison.Ordinal) ||
                loggingEvent.LoggerName.EndsWith(".CirceComConnection", StringComparison.Ordinal) ||
                loggingEvent.LoggerName.EndsWith(".ComPortRotator", StringComparison.Ordinal) ||
                loggingEvent.LoggerName.EndsWith(".NetworkHealthMonitor", StringComparison.Ordinal);
        }

        private static bool IsLockSleepLogEvent([NotNull] LoggingEvent loggingEvent)
        {
            return LockTracker.IsLockMessage(loggingEvent.RenderedMessage) ||
                loggingEvent.RenderedMessage.IndexOf("Starting sleep on wait handle.", StringComparison.Ordinal) != -1;
        }
    }
}
