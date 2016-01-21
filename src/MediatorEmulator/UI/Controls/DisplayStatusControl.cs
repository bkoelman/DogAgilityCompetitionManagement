using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.MediatorEmulator.Engine;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    /// <summary>
    /// The emulated LED segments of a wireless remote display.
    /// </summary>
    public sealed partial class DisplayStatusControl : UserControl, ISimpleVisualizationActor
    {
        private const string MillisecDashes = "---";
        private const string EliminationText = "-E-";

        [CanBeNull]
        private string lastMillisecondsValue;

        [CanBeNull]
        private DateTime? startTime;

        private bool IsEliminated => primaryTimeMillisecondsLabel.Text == EliminationText;

        public DisplayStatusControl()
        {
            InitializeComponent();

            var timerFontContainer = new TimerFontContainer(ref components);
            timerFontContainer.ApplyTo(primaryTimeSecondsLabel, primaryTimeMillisecondsLabel, currentFaultsValueLabel,
                currentRefusalsValueLabel, currentCompetitorNumberLabel, previousCompetitorPlacementLabel,
                nextCompetitorNumberLabel);
        }

        private void DisplayStatusControl_Resize([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            int centerX = displayGroupBox.ClientSize.Width / 2;
            int labelWidth = previousCompetitorPlacementLabel.Width;
            previousCompetitorPlacementLabel.Location = new Point(centerX - labelWidth / 2,
                previousCompetitorPlacementLabel.Location.Y);
        }

        private void DisplayRefreshTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (startTime != null)
            {
                TimeSpan timePassed = SystemContext.UtcNow() - startTime.Value;
                UpdatePrimaryTime(timePassed, false);
            }
        }

        private void UpdatePrimaryTime([CanBeNull] TimeSpan? time, bool showMilliseconds)
        {
            primaryTimeSecondsLabel.Text = TextFormatting.FormatSeconds(time);

            if (!IsEliminated)
            {
                primaryTimeMillisecondsLabel.Text = showMilliseconds ? lastMillisecondsValue : MillisecDashes;
            }
        }

        void ISimpleVisualizationActor.StartPrimaryTimer()
        {
            startTime = SystemContext.UtcNow();
            DisplayRefreshTimer_Tick(this, EventArgs.Empty);
            displayRefreshTimer.Enabled = true;
        }

        void ISimpleVisualizationActor.StopAndSetOrClearPrimaryTime(TimeSpan? time)
        {
            StopPrimaryTimer();

            lastMillisecondsValue = TextFormatting.FormatMilliseconds(time);
            UpdatePrimaryTime(time, true);
        }

        private void StopPrimaryTimer()
        {
            displayRefreshTimer.Enabled = false;
            startTime = null;
        }

        void ISimpleVisualizationActor.SetOrClearFaultCount(int? count)
        {
            currentFaultsValueLabel.Text = TextFormatting.FormatNumber(count, 2);
        }

        void ISimpleVisualizationActor.SetOrClearRefusalCount(int? count)
        {
            currentRefusalsValueLabel.Text = TextFormatting.FormatNumber(count, 2);
        }

        void ISimpleVisualizationActor.SetElimination(bool isEliminated)
        {
            primaryTimeMillisecondsLabel.Text = isEliminated
                ? EliminationText
                : (startTime != null ? MillisecDashes : lastMillisecondsValue);
        }

        void ISimpleVisualizationActor.SetOrClearCurrentCompetitorNumber(int? number)
        {
            currentCompetitorNumberLabel.Text = TextFormatting.FormatNumber(number, 3);
        }

        void ISimpleVisualizationActor.SetOrClearNextCompetitorNumber(int? number)
        {
            nextCompetitorNumberLabel.Text = TextFormatting.FormatNumber(number, 3);
        }

        void ISimpleVisualizationActor.SetOrClearPreviousCompetitorPlacement(int? placement)
        {
            previousCompetitorPlacementLabel.Text = TextFormatting.FormatNumber(placement, 3);
        }

        private static class TextFormatting
        {
            [NotNull]
            public static string FormatNumber([CanBeNull] int? number, int digitCount)
            {
                if (number == null)
                {
                    return string.Empty;
                }

                var formatBuilder = new StringBuilder();
                formatBuilder.Append("{0,");
                formatBuilder.Append(digitCount);
                formatBuilder.Append("}");

                return string.Format(formatBuilder.ToString(), number);
            }

            [NotNull]
            public static string FormatSeconds([CanBeNull] TimeSpan? time)
            {
                if (time == null)
                {
                    return string.Empty;
                }

                double seconds = Math.Truncate(time.Value.TotalSeconds);
                return $"{seconds,3}";
            }

            [NotNull]
            public static string FormatMilliseconds([CanBeNull] TimeSpan? time)
            {
                return time == null ? string.Empty : $"{time.Value.Milliseconds:000}";
            }
        }
    }
}