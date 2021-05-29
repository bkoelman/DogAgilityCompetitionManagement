using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A readonly or editable grid control that contains competition run results.
    /// </summary>
    public sealed partial class CompetitionRunResultsGrid : UserControl
    {
        [CanBeNull]
        private IButtonControl parentFormCancelButton;

        public bool IsEditable
        {
            get => !runResultsGrid.ReadOnly;
            set
            {
                runResultsGrid.ReadOnly = !value;
                runResultsGrid.RowHeadersVisible = value;
            }
        }

        public bool ShowPlacement
        {
            get => PlacementColumn.Visible;
            set => PlacementColumn.Visible = value;
        }

        public bool HideBackColors { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [NotNull]
        [ItemNotNull]
        public IEnumerable<CompetitionRunResult> DataSource
        {
            get
            {
                IEnumerable<CompetitionRunResultRowInGrid> source = competitionRunResultRowBindingSource.List.Cast<CompetitionRunResultRowInGrid>();
                return source.Select(r => r.ToCompetitionRunResult()).ToList();
            }
            set
            {
                Guard.NotNull(value, nameof(value));
                competitionRunResultRowBindingSource.DataSource = value.Select(CompetitionRunResultRowInGrid.FromCompetitionRunResult).ToList();
            }
        }

        public CompetitionRunResultsGrid()
        {
            InitializeComponent();
        }

        public void GoToCompetitor(int competitorNumber)
        {
            int index = 0;

            foreach (CompetitionRunResultRowInGrid rowInGrid in competitionRunResultRowBindingSource)
            {
                if (rowInGrid.CompetitorNumber == competitorNumber)
                {
                    competitionRunResultRowBindingSource.Position = index;
                    break;
                }

                index++;
            }
        }

        public void RecalculatePlacements()
        {
            CompetitionClassModel model = CacheManager.DefaultInstance.ActiveModel.ChangeRunResults(DataSource);
            model = model.RecalculatePlacements();
            DataSource = model.Results;
        }

        private void RunResultsGrid_CellFormatting([CanBeNull] object sender, [NotNull] DataGridViewCellFormattingEventArgs e)
        {
            if (!HideBackColors)
            {
                CompetitionRunResult runResult = DataSource.ElementAtOrDefault(e.RowIndex);

                if (runResult is { HasCompleted: true })
                {
                    e.CellStyle.BackColor = runResult.IsEliminated ? Color.FromArgb(255, 224, 194) : Color.FromArgb(224, 255, 194);
                }
            }
        }

        private void RunResultsGrid_CellBeginEdit([CanBeNull] object sender, [NotNull] DataGridViewCellCancelEventArgs e)
        {
            // Prevent that pressing ESC closes the parent form when user wants to cancel cell editing.

            if (Parent is Form parentForm)
            {
                parentFormCancelButton = parentForm.CancelButton;
                parentForm.CancelButton = null;
            }
        }

        private void RunResultsGrid_CellEndEdit([CanBeNull] object sender, [NotNull] DataGridViewCellEventArgs e)
        {
            if (Parent is Form parentForm)
            {
                parentForm.CancelButton = parentFormCancelButton;
            }

            runResultsGrid.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void RunResultsGrid_CellValidating([CanBeNull] object sender, [NotNull] DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewColumn column = runResultsGrid.Columns[e.ColumnIndex];

            if (IsTimeColumn(column))
            {
                try
                {
                    TimeSpanWithAccuracy.FromString(e.FormattedValue.ToString());
                }
                catch (FormatException ex)
                {
                    runResultsGrid.Rows[e.RowIndex].ErrorText = ex.Message;
                    e.Cancel = true;
                }
            }
            else if (column == FaultCountColumn)
            {
                try
                {
                    int value = int.Parse(e.FormattedValue.ToString());
                    CompetitionRunResult.AssertFaultCountIsValid(value);
                }
                catch (Exception ex)
                {
                    runResultsGrid.Rows[e.RowIndex].ErrorText = ex.Message;
                    e.Cancel = true;
                }
            }
            else if (column == RefusalCountColumn)
            {
                try
                {
                    int value = int.Parse(e.FormattedValue.ToString());
                    CompetitionRunResult.AssertRefusalCountIsValid(value);
                }
                catch (Exception ex)
                {
                    runResultsGrid.Rows[e.RowIndex].ErrorText = ex.Message;
                    e.Cancel = true;
                }
            }
        }

        private bool IsTimeColumn([NotNull] DataGridViewColumn column)
        {
            return column == IntermediateTime1Column || column == IntermediateTime2Column || column == IntermediateTime3Column || column == FinishTimeColumn;
        }
    }
}
