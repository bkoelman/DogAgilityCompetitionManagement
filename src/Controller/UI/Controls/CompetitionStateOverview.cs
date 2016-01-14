using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Controller.Engine.Visualization;
using DogAgilityCompetition.Controller.UI.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Simplified/compact version of <see cref="TimerDisplayForm" /> to show competition run progress.
    /// </summary>
    public sealed partial class CompetitionStateOverview : UserControl, IVisualizationActor
    {
        private const int WsExComposited = 0x02000000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WsExComposited;
                return cp;
            }
        }

        public CompetitionStateOverview()
        {
            InitializeComponent();
        }

        private void CompetitionStateOverview_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            foreach (VisualizationChange change in VisualizationChangeFactory.ClearAll())
            {
                change.ApplyTo(this);
            }
        }

        void IVisualizationActor.SetClass(CompetitionClassInfo classInfo)
        {
        }

        [CanBeNull]
        private DateTime? startTime;

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
            currentCompetitorNumberLabel.Text = competitor != null
                ? TextFormatting.FormatCompetitorNumber(competitor.Number)
                : string.Empty;
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
            nextCompetitorNumberLabel.Text = competitor != null
                ? TextFormatting.FormatCompetitorNumber(competitor.Number)
                : string.Empty;
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
                prevCompetitorNumberLabel.Text =
                    TextFormatting.FormatCompetitorNumber(competitorRunResult.Competitor.Number);
                prevTimeLabel.Text =
                    TextFormatting.FormatTime(
                        competitorRunResult.Timings?.FinishTime?.ElapsedSince(competitorRunResult.Timings.StartTime)
                            .TimeValue);
                prevFaultsValueLabel.Text = TextFormatting.FormatNumber(competitorRunResult.FaultCount, 2);
                prevRefusalsValueLabel.Text = TextFormatting.FormatNumber(competitorRunResult.RefusalCount, 2);

                Color foreColor = competitorRunResult.IsEliminated
                    ? RunHistoryLine.EliminationColor
                    : SystemColors.ControlText;
                prevTimeLabel.ForeColor = foreColor;
                prevPlacementLabel.ForeColor = foreColor;
                prevPlacementLabel.Text = competitorRunResult.IsEliminated
                    ? @"X"
                    : TextFormatting.FormatPlacement(competitorRunResult.Placement);
            }
            else
            {
                prevCompetitorNumberLabel.Text = string.Empty;
                prevTimeLabel.Text = string.Empty;
                prevFaultsValueLabel.Text = string.Empty;
                prevRefusalsValueLabel.Text = string.Empty;
                prevPlacementLabel.Text = string.Empty;
            }
        }

        void IVisualizationActor.SetOrClearRankings(IEnumerable<CompetitionRunResult> rankings)
        {
        }

        [CanBeNull]
        private Panel rankingsOverlayPanel;

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
            Point topLeft = nextPanel.Location;
            Point bottomRight = prevPlacementPanel.Location + prevPlacementPanel.Size;

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
        }

        void IVisualizationActor.PlaySound(string path)
        {
        }
    }
}