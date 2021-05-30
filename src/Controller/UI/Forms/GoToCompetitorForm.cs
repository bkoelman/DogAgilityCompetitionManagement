using System;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Dialog to quickly select a competitor by number.
    /// </summary>
    public sealed partial class GoToCompetitorForm : Form
    {
        public int? SelectedCompetitorNumber => int.TryParse(competitorNumberTextBox.Text.Trim(), out int value) ? value : null;

        public GoToCompetitorForm()
        {
            InitializeComponent();
        }

        private void CompetitorNumberTextBox_TextChanged(object? sender, EventArgs e)
        {
            okButton.Enabled = SelectedCompetitorNumber != null;
        }
    }
}
