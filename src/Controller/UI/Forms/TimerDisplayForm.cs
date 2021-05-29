using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Controller.Engine.Visualization;
using DogAgilityCompetition.Controller.UI.Controls;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Displays competition run progress on a large screen.
    /// </summary>
    public sealed partial class TimerDisplayForm : Form, IVisualizationActor
    {
        private const int HistoryLineCount = 5;
        private const int WsExComposited = 0x02000000;

        [NotNull]
        [ItemNotNull]
        private readonly RunHistoryLine[] historyLines;

        [CanBeNull]
        private DateTime? startTime;

        private int secondaryTimeHighlightCount;

        [CanBeNull]
        private Panel rankingsOverlayPanel;

        [CanBeNull]
        private NotifyPictureForm lastPlayedAnimationForm;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WsExComposited;
                return cp;
            }
        }

        public TimerDisplayForm()
        {
            InitializeComponent();

            // ReSharper disable once RedundantExplicitArraySize
            // Reason: Extra compiler check when number of lines changes.
            historyLines = new RunHistoryLine[HistoryLineCount]
            {
                runHistoryLine1,
                runHistoryLine2,
                runHistoryLine3,
                runHistoryLine4,
                runHistoryLine5
            };

            var timerFontContainer = new TimerFontContainer(ref components);
            timerFontContainer.ApplyTo(primaryTimeLabel, secondaryTimeLabel);
        }

        private void TimerDisplayForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            foreach (VisualizationChange change in VisualizationChangeFactory.ClearAll())
            {
                change.ApplyTo(this);
            }
        }

        private void TimerDisplayForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        void IVisualizationActor.SetClass(CompetitionClassInfo classInfo)
        {
            gradeLabel.Text = classInfo != null ? classInfo.Grade : string.Empty;
            classTypeLabel.Text = classInfo != null ? classInfo.Type : string.Empty;
            standardCourseTimeValueLabel.Text = classInfo?.StandardCourseTime != null ? $"{classInfo.StandardCourseTime.Value.TotalSeconds:0}" : string.Empty;
        }

        void IVisualizationActor.StartPrimaryTimer()
        {
            startTime = SystemContext.UtcNow();
            displayRefreshTimer.Enabled = true;
        }

        private void DisplayRefreshTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (startTime != null)
            {
                TimeSpan timePassed = SystemContext.UtcNow() - startTime.Value;
                primaryTimeLabel.Text = TextFormatting.FormatTime(timePassed);
            }
        }

        void IVisualizationActor.StopAndSetOrClearPrimaryTime(TimeSpan? time)
        {
            StopPrimaryTimer();
            primaryTimeLabel.Text = time != null ? TextFormatting.FormatTime(time.Value) : string.Empty;
        }

        private void StopPrimaryTimer()
        {
            displayRefreshTimer.Enabled = false;
            startTime = null;
        }

        void IVisualizationActor.SetOrClearSecondaryTime(TimeSpan? time, bool doBlink)
        {
            secondaryTimeLabel.Text = time != null ? TextFormatting.FormatTime(time) : string.Empty;
            secondaryTimeHighlighter.IsHighlightEnabled = doBlink && time != null;

            if (secondaryTimeHighlighter.IsHighlightEnabled)
            {
                secondaryTimeHighlightCount = 0;
            }
        }

        private void SecondaryTimeHighlighter_HighlightCycleFinished([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            secondaryTimeHighlightCount++;

            if (secondaryTimeHighlightCount == 3)
            {
                secondaryTimeHighlighter.IsHighlightEnabled = false;
            }
        }

        void IVisualizationActor.SetOrClearFaultCount(int? count)
        {
            currentFaultsValueLabel.Text = TextFormatting.FormatNumber(count, 2);
        }

        void IVisualizationActor.SetOrClearRefusalCount(int? count)
        {
            currentRefusalsValueLabel.Text = TextFormatting.FormatNumber(count, 2);
        }

        void IVisualizationActor.SetElimination(bool isEliminated)
        {
            eliminationCaptionLabel.Visible = isEliminated;
            primaryTimeLabel.ForeColor = isEliminated ? RunHistoryLine.EliminationColor : SystemColors.ControlText;
        }

        void IVisualizationActor.SetOrClearCurrentCompetitor(Competitor competitor)
        {
            currentCompetitorNumberLabel.Text = competitor != null ? TextFormatting.FormatCompetitorNumber(competitor.Number) : string.Empty;
            currentHandlerNameLabel.Text = competitor?.HandlerName ?? string.Empty;
            currentDogNameLabel.Text = competitor?.DogName ?? string.Empty;
        }

        void IVisualizationActor.SetCurrentCompetitorNumber(int number)
        {
            currentCompetitorNumberLabel.Text = TextFormatting.FormatCompetitorNumber(number);
        }

        void IVisualizationActor.BlinkCurrentCompetitorNumber(bool isEnabled)
        {
            currentCompetitorNumberHighlighter.IsHighlightEnabled = isEnabled;
        }

        void IVisualizationActor.SetOrClearNextCompetitor(Competitor competitor)
        {
            nextCompetitorNumberLabel.Text = competitor != null ? TextFormatting.FormatCompetitorNumber(competitor.Number) : string.Empty;
            nextHandlerNameLabel.Text = competitor?.HandlerName ?? string.Empty;
            nextDogNameLabel.Text = competitor?.DogName ?? string.Empty;
        }

        void IVisualizationActor.SetNextCompetitorNumber(int number)
        {
            nextCompetitorNumberLabel.Text = TextFormatting.FormatCompetitorNumber(number);
        }

        void IVisualizationActor.BlinkNextCompetitorNumber(bool isEnabled)
        {
            nextCompetitorNumberHighlighter.IsHighlightEnabled = isEnabled;
        }

        void IVisualizationActor.SetOrClearPreviousCompetitorRun(CompetitionRunResult competitorRunResult)
        {
            if (competitorRunResult != null)
            {
                prevCompetitorNumberLabel.Text = TextFormatting.FormatCompetitorNumber(competitorRunResult.Competitor.Number);
                prevHandlerNameLabel.Text = competitorRunResult.Competitor.HandlerName;
                prevDogNameLabel.Text = competitorRunResult.Competitor.DogName;

                prevTimeLabel.Text =
                    TextFormatting.FormatTime(competitorRunResult.Timings?.FinishTime?.ElapsedSince(competitorRunResult.Timings.StartTime).TimeValue);

                prevFaultsValueLabel.Text = TextFormatting.FormatNumber(competitorRunResult.FaultCount, 2);
                prevRefusalsValueLabel.Text = TextFormatting.FormatNumber(competitorRunResult.RefusalCount, 2);

                Color foreColor = competitorRunResult.IsEliminated ? RunHistoryLine.EliminationColor : SystemColors.ControlText;
                prevTimeLabel.ForeColor = foreColor;
                prevPlacementLabel.ForeColor = foreColor;
                prevPlacementLabel.Text = competitorRunResult.IsEliminated ? "X" : TextFormatting.FormatPlacement(competitorRunResult.Placement);
            }
            else
            {
                prevCompetitorNumberLabel.Text = string.Empty;
                prevHandlerNameLabel.Text = string.Empty;
                prevDogNameLabel.Text = string.Empty;
                prevTimeLabel.Text = string.Empty;
                prevFaultsValueLabel.Text = string.Empty;
                prevRefusalsValueLabel.Text = string.Empty;
                prevPlacementLabel.Text = string.Empty;
            }
        }

        void IVisualizationActor.SetOrClearRankings(IEnumerable<CompetitionRunResult> rankings)
        {
            int index = 0;

            foreach (CompetitionRunResult runResult in rankings.Take(historyLines.Length))
            {
                historyLines[index].SetCompetitionRunResult(runResult);
                index++;
            }

            for (; index < historyLines.Length; index++)
            {
                historyLines[index].ClearCompetitionRunResult();
            }
        }

        void IVisualizationActor.SetClockSynchronizationMode(ClockSynchronizationMode mode)
        {
            string message;

            switch (mode)
            {
                case ClockSynchronizationMode.RecommendSynchronization:
                    message = "Entering standby\nin a few minutes...";
                    break;
                case ClockSynchronizationMode.RequireSynchronization:
                    message = "Press [Ready] to\nwake from standby";
                    break;
                default:
                    message = null;
                    break;
            }

            RemoveRankingsOverlay();

            if (message != null)
            {
                rankingsOverlayPanel = CreateRankingsOverlay(message);
                Controls.Add(rankingsOverlayPanel);
                rankingsOverlayPanel.BringToFront();
            }
        }

        private void RemoveRankingsOverlay()
        {
            if (rankingsOverlayPanel != null)
            {
                Controls.Remove(rankingsOverlayPanel);
            }
        }

        [NotNull]
        private Panel CreateRankingsOverlay([NotNull] string text)
        {
            RunHistoryLine lastHistoryLine = historyLines[HistoryLineCount - 1];

            Point topLeft = competitorNumberCaptionPanel.Location;
            Point bottomRight = lastHistoryLine.Location + lastHistoryLine.Size;

            return new Panel
            {
                Location = topLeft,
                Size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y),
                BackColor = Color.DarkRed,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Controls =
                {
                    new AutoScalingLabel
                    {
                        Text = text,
                        ForeColor = Color.White,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    }
                }
            };
        }

        void IVisualizationActor.StartAnimation(Bitmap bitmap)
        {
            lastPlayedAnimationForm?.Cancel();

            lastPlayedAnimationForm = new NotifyPictureForm();
            lastPlayedAnimationForm.AnimationCompleted += LastPlayedAnimationFormOnAnimationCompleted;
            lastPlayedAnimationForm.ShowAnimated(this, bitmap);
        }

        private void LastPlayedAnimationFormOnAnimationCompleted([CanBeNull] object sender, [NotNull] EventArgs eventArgs)
        {
            if (sender is NotifyPictureForm source)
            {
                source.AnimationCompleted -= LastPlayedAnimationFormOnAnimationCompleted;
            }
        }

        void IVisualizationActor.PlaySound(string path)
        {
            SystemSound.PlayWaveFile(path);
        }
    }
}
