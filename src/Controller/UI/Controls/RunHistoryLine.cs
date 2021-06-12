using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A view of a single competitor run result on a large screen.
    /// </summary>
    public sealed partial class RunHistoryLine : UserControl
    {
        private const string DefaultColorInHex = "0xFFE0C2";

        public static readonly Color EliminationColor = Color.FromArgb(255, 192, 0, 0);

        [Category("Appearance")]
        [DefaultValue(typeof(Color), DefaultColorInHex)]
        public Color FillColor
        {
            get => competitorNumberPanel.BackColor;
            set
            {
                competitorNumberPanel.BackColor = value;
                countryCodePanel.BackColor = value;
                dogNamePanel.BackColor = value;
                handlerNamePanel.BackColor = value;
                finishTimePanel.BackColor = value;
                faultsPanel.BackColor = value;
                refusalsPanel.BackColor = value;
                placementPanel.BackColor = value;
            }
        }

        public RunHistoryLine()
        {
            InitializeComponent();
            FillColor = ColorTranslator.FromHtml(DefaultColorInHex);
        }

        public void ClearCompetitionRunResult()
        {
            competitorNumberLabel.Text = string.Empty;
            countryCodeLabel.Text = string.Empty;
            dogNameLabel.Text = string.Empty;
            handlerNameLabel.Text = string.Empty;
            finishTimeLabel.Text = string.Empty;
            faultsLabel.Text = string.Empty;
            refusalsLabel.Text = string.Empty;
            placementLabel.Text = string.Empty;
        }

        public void SetCompetitionRunResult(CompetitionRunResult runResult)
        {
            Guard.NotNull(runResult, nameof(runResult));

            competitorNumberLabel.Text = TextFormatting.FormatCompetitorNumber(runResult.Competitor.Number);
            countryCodeLabel.Text = runResult.Competitor.CountryCode;
            dogNameLabel.Text = runResult.Competitor.DogName;
            handlerNameLabel.Text = runResult.Competitor.HandlerName;
            finishTimeLabel.Text = TextFormatting.FormatTime(runResult.Timings?.FinishTime?.ElapsedSince(runResult.Timings.StartTime).TimeValue);
            faultsLabel.Text = TextFormatting.FormatNumber(runResult.FaultCount, 2);
            refusalsLabel.Text = TextFormatting.FormatNumber(runResult.RefusalCount, 2);

            if (runResult.IsEliminated)
            {
                finishTimeLabel.ForeColor = EliminationColor;

                placementLabel.Text = "X";
                placementLabel.ForeColor = EliminationColor;
            }
            else
            {
                finishTimeLabel.ForeColor = SystemColors.ControlText;

                placementLabel.Text = TextFormatting.FormatPlacement(runResult.Placement);
                placementLabel.ForeColor = SystemColors.ControlText;
            }
        }
    }
}
