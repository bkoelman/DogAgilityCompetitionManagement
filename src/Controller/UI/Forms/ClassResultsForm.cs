using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Editor for competition run results.
    /// </summary>
    public sealed partial class ClassResultsForm : Form
    {
        [NotNull]
        private CompetitionClassModel originalVersion;

        public ClassResultsForm()
        {
            InitializeComponent();

            originalVersion = CacheManager.DefaultInstance.ActiveModel;
        }

        private void ClassResultsForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            runResultsGrid.DataSource = originalVersion.Results;
        }

        private void ExportButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = @"Select competitors file for export";
                dialog.Filter = @"Csv files (*.csv)|*.csv|All files (*.*)|*.*";

                dialog.FileName = ProposeFileNameFor(originalVersion);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var calculator = new ClassPlacementCalculator(originalVersion);
                    IEnumerable<CompetitionRunResult> runResultsRecalculated =
                        calculator.Recalculate(runResultsGrid.DataSource);

                    RunResultsExporter.ExportTo(dialog.FileName, runResultsRecalculated);
                }
            }
        }

        [NotNull]
        private static string ProposeFileNameFor([NotNull] CompetitionClassModel model)
        {
            return $"{model.ClassInfo.Grade}-{model.ClassInfo.Type}_" +
                $"SPT-{model.ClassInfo.StandardParcoursTime?.TotalSeconds.ToString(CultureInfo.InvariantCulture) ?? string.Empty}_" +
                $"MPT-{model.ClassInfo.MaximumParcoursTime?.TotalSeconds.ToString(CultureInfo.InvariantCulture) ?? string.Empty}_" +
                $"TL-{model.ClassInfo.TrackLengthInMeters}_" +
                $"DT-{SystemContext.UtcNow().ToString("yyyyMMdd-HHmmss")}.csv";
        }

        private void RefreshButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            runResultsGrid.RecalculatePlacements();
        }

        private void OkButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            CompetitionClassModel newVersion =
                originalVersion.ChangeRunResults(runResultsGrid.DataSource).RecalculatePlacements();

            originalVersion = CacheManager.DefaultInstance.ReplaceModel(newVersion, originalVersion);
            DialogResult = DialogResult.OK;
        }

        private void GoToCompetitorButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var form = new GoToCompetitorForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.SelectedCompetitorNumber != null)
                {
                    runResultsGrid.GoToCompetitor(form.SelectedCompetitorNumber.Value);
                }
            }
        }
    }
}