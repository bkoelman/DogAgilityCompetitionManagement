using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Dialog to quickly select a competitor by number.
    /// </summary>
    public sealed partial class GoToCompetitorForm : Form
    {
        [CanBeNull]
        public int? SelectedCompetitorNumber
        {
            get
            {
                int value;
                return int.TryParse(competitorNumberTextBox.Text.Trim(), out value) ? (int?) value : null;
            }
        }

        public GoToCompetitorForm()
        {
            InitializeComponent();
        }

        private void CompetitorNumberTextBox_TextChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            okButton.Enabled = SelectedCompetitorNumber != null;
        }
    }
}