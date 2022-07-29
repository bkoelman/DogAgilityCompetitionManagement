using System.Text;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.UI.Forms;

/// <summary>
/// Editor for competition run results.
/// </summary>
public sealed partial class ClassResultsForm : Form
{
    private CompetitionClassModel originalVersion;

    public ClassResultsForm()
    {
        InitializeComponent();

        originalVersion = CacheManager.DefaultInstance.ActiveModel;
    }

    private void ClassResultsForm_Load(object? sender, EventArgs e)
    {
        runResultsGrid.DataSource = originalVersion.Results;
    }

    private void ExportButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Select competitors file for export",
            Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = ProposeFileNameFor(originalVersion)
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var calculator = new ClassPlacementCalculator(originalVersion);
            IEnumerable<CompetitionRunResult> runResultsRecalculated = calculator.Recalculate(runResultsGrid.DataSource);

            RunResultsExporter.ExportTo(dialog.FileName, runResultsRecalculated);
        }
    }

    private static string ProposeFileNameFor(CompetitionClassModel model)
    {
        var textBuilder = new StringBuilder();

        AddToBuilder(model.ClassInfo.Grade, textBuilder);
        AddToBuilder(model.ClassInfo.Type, textBuilder);
        AddToBuilder(SystemContext.UtcNow().ToString("yyyyMMdd-HHmmss"), textBuilder);
        textBuilder.Append(".csv");

        return textBuilder.ToString();
    }

    private static void AddToBuilder(string? value, StringBuilder textBuilder)
    {
        string? safeValue = MakeSafeForFileName(value);

        if (!string.IsNullOrEmpty(safeValue))
        {
            if (textBuilder.Length > 0)
            {
                textBuilder.Append('-');
            }

            textBuilder.Append(safeValue);
        }
    }

    private static string? MakeSafeForFileName(string? text)
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

    private void RefreshButton_Click(object? sender, EventArgs e)
    {
        runResultsGrid.RecalculatePlacements();
    }

    private void OkButton_Click(object? sender, EventArgs e)
    {
        CompetitionClassModel newVersion = originalVersion.ChangeRunResults(runResultsGrid.DataSource).RecalculatePlacements();

        originalVersion = CacheManager.DefaultInstance.ReplaceModel(newVersion, originalVersion);
        DialogResult = DialogResult.OK;
    }

    private void GoToCompetitorButton_Click(object? sender, EventArgs e)
    {
        using var form = new GoToCompetitorForm();

        if (form.ShowDialog(this) == DialogResult.OK && form.SelectedCompetitorNumber != null)
        {
            runResultsGrid.GoToCompetitor(form.SelectedCompetitorNumber.Value);
        }
    }
}
