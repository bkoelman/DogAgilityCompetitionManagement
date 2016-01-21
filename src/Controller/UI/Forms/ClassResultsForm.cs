using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            var textBuilder = new StringBuilder();

            AddToBuilder(model.ClassInfo.Grade, textBuilder);
            AddToBuilder(model.ClassInfo.Type, textBuilder);
            AddToBuilder(SystemContext.UtcNow().ToString("yyyyMMdd-HHmmss"), textBuilder);
            textBuilder.Append(".csv");

            return textBuilder.ToString();
        }

        private static void AddToBuilder([CanBeNull] string value, [NotNull] StringBuilder textBuilder)
        {
            string safeValue = MakeSafeForFileName(value);
            if (!string.IsNullOrEmpty(safeValue))
            {
                if (textBuilder.Length > 0)
                {
                    textBuilder.Append("-");
                }
                textBuilder.Append(safeValue);
            }
        }

        [CanBeNull]
        private static string MakeSafeForFileName([CanBeNull] string text)
        {
            if (text != null)
            {
                foreach (char ch in Path.GetInvalidFileNameChars())
                {
                    text = text.Replace(ch.ToString(), string.Empty);
                }
            }
            return text;
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