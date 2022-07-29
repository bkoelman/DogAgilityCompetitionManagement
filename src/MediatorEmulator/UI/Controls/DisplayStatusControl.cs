using System.Text;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.MediatorEmulator.Engine;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls;

/// <summary>
/// The emulated LED segments of a wireless remote display.
/// </summary>
public sealed partial class DisplayStatusControl : UserControl, ISimpleVisualizationActor
{
    private const string MillisecDashes = "---";
    private const string EliminationText = "-E-";
    private static readonly TimeSpan FreezeSecondaryTimeDuration = TimeSpan.FromSeconds(2);

    private string? primaryTimeMillisecondsMeasured;
    private DateTime? primaryTimeStartedAt;
    private SecondaryTime? secondaryTime;

    private bool IsEliminated => primaryTimeMillisecondsLabel.Text == EliminationText;

    public DisplayStatusControl()
    {
        InitializeComponent();

        var timerFontContainer = new TimerFontContainer(ref components);

        timerFontContainer.ApplyTo(primaryTimeSecondsLabel, primaryTimeMillisecondsLabel, currentFaultsValueLabel, currentRefusalsValueLabel,
            currentCompetitorNumberLabel, previousCompetitorPlacementLabel, nextCompetitorNumberLabel);
    }

    private void DisplayStatusControl_Resize(object? sender, EventArgs e)
    {
        int centerX = displayGroupBox.ClientSize.Width / 2;
        int labelWidth = previousCompetitorPlacementLabel.Width;

        previousCompetitorPlacementLabel.Location = previousCompetitorPlacementLabel.Location with
        {
            X = centerX - labelWidth / 2
        };
    }

    private void DisplayRefreshTimer_Tick(object? sender, EventArgs e)
    {
        bool isSecondaryTimeVisible = secondaryTime is { IsVisible: true };

        if (primaryTimeStartedAt != null && !isSecondaryTimeVisible)
        {
            TimeSpan timePassed = SystemContext.UtcNow() - primaryTimeStartedAt.Value;
            UpdatePrimaryTime(timePassed, false);
        }
    }

    private void UpdatePrimaryTime(TimeSpan? primaryTime, bool showMilliseconds)
    {
        primaryTimeSecondsLabel.Text = TextFormatting.FormatSeconds(primaryTime);

        if (!IsEliminated)
        {
            primaryTimeMillisecondsLabel.Text = showMilliseconds ? primaryTimeMillisecondsMeasured : MillisecDashes;
        }
    }

    void ISimpleVisualizationActor.StartPrimaryTimer()
    {
        secondaryTime = null;

        primaryTimeStartedAt = SystemContext.UtcNow();
        DisplayRefreshTimer_Tick(this, EventArgs.Empty);
        displayRefreshTimer.Enabled = true;
    }

    void ISimpleVisualizationActor.StopAndSetOrClearPrimaryTime(TimeSpan? time)
    {
        StopTimers();

        primaryTimeMillisecondsMeasured = TextFormatting.FormatMilliseconds(time);
        UpdatePrimaryTime(time, true);
    }

    private void StopTimers()
    {
        displayRefreshTimer.Enabled = false;
        primaryTimeStartedAt = null;
        secondaryTime = null;
    }

    void ISimpleVisualizationActor.SetOrClearSecondaryTime(TimeSpan? time)
    {
        if (time == null)
        {
            secondaryTime = null;
        }
        else
        {
            secondaryTime = new SecondaryTime(time.Value);
            primaryTimeSecondsLabel.Text = TextFormatting.FormatSeconds(secondaryTime.Value.TimeValue);

            if (!IsEliminated)
            {
                primaryTimeMillisecondsLabel.Text = TextFormatting.FormatMilliseconds(secondaryTime.Value.TimeValue);
            }
        }
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
        bool isSecondaryTimeVisible = secondaryTime is { IsVisible: true };

        // @formatter:keep_existing_linebreaks true

        primaryTimeMillisecondsLabel.Text =
            isEliminated
                ? EliminationText
                : isSecondaryTimeVisible
                    ? TextFormatting.FormatMilliseconds(secondaryTime!.Value.TimeValue)
                    : primaryTimeStartedAt != null
                        ? MillisecDashes
                        : primaryTimeMillisecondsMeasured;

        // @formatter:keep_existing_linebreaks restore
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
        public static string FormatNumber(int? number, int digitCount)
        {
            if (number == null)
            {
                return string.Empty;
            }

            var formatBuilder = new StringBuilder();
            formatBuilder.Append("{0,");
            formatBuilder.Append(digitCount);
            formatBuilder.Append('}');

            return string.Format(formatBuilder.ToString(), number);
        }

        public static string FormatSeconds(TimeSpan? time)
        {
            if (time == null)
            {
                return string.Empty;
            }

            double seconds = Math.Truncate(time.Value.TotalSeconds);
            return $"{seconds,3}";
        }

        public static string FormatMilliseconds(TimeSpan? time)
        {
            return time == null ? string.Empty : $"{time.Value.Milliseconds:000}";
        }
    }

    private readonly struct SecondaryTime
    {
        private readonly DateTime startedAt;

        public TimeSpan TimeValue { get; }

        public bool IsVisible => startedAt.Add(FreezeSecondaryTimeDuration) >= SystemContext.UtcNow();

        public SecondaryTime(TimeSpan timeValue)
        {
            startedAt = SystemContext.UtcNow();
            TimeValue = timeValue;
        }
    }
}
