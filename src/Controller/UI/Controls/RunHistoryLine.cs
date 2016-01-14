using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A view of a single competitor run result on a large screen.
    /// </summary>
    public sealed partial class RunHistoryLine : UserControl
    {
        public static readonly Color EliminationColor = Color.FromArgb(255, 192, 0, 0);

        private const string DefaultColorInHex = "0xFFE0C2";

        [Category("Appearance")]
        [DefaultValue(typeof (Color), DefaultColorInHex)]
        public Color FillColor
        {
            get
            {
                return competitorNumberPanel.BackColor;
            }
            set
            {
                competitorNumberPanel.BackColor = value;
                countryCodePanel.BackColor = value;
                dogNamePanel.BackColor = value;
                competitorNamePanel.BackColor = value;
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
            competitorNameLabel.Text = string.Empty;
            finishTimeLabel.Text = string.Empty;
            faultsLabel.Text = string.Empty;
            refusalsLabel.Text = string.Empty;
            placementLabel.Text = string.Empty;
        }

        public void SetCompetitionRunResult([NotNull] CompetitionRunResult runResult)
        {
            Guard.NotNull(runResult, nameof(runResult));

            competitorNumberLabel.Text = TextFormatting.FormatCompetitorNumber(runResult.Competitor.Number);
            countryCodeLabel.Text = runResult.Competitor.CountryCode;
            dogNameLabel.Text = runResult.Competitor.DogName;
            competitorNameLabel.Text = runResult.Competitor.Name;
            finishTimeLabel.Text =
                TextFormatting.FormatTime(
                    runResult.Timings?.FinishTime?.ElapsedSince(runResult.Timings.StartTime).TimeValue);
            faultsLabel.Text = TextFormatting.FormatNumber(runResult.FaultCount, 2);
            refusalsLabel.Text = TextFormatting.FormatNumber(runResult.RefusalCount, 2);

            if (runResult.IsEliminated)
            {
                finishTimeLabel.ForeColor = EliminationColor;

                placementLabel.Text = @"X";
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