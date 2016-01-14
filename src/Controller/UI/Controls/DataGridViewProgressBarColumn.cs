using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A column in a <see cref="DataGridView" /> that contains a colored progress bar.
    /// </summary>
    public sealed class DataGridViewProgressBarColumn : DataGridViewTextBoxColumn
    {
        [CanBeNull]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (!(value is DataGridViewProgressBarCell))
                {
                    throw new InvalidOperationException(
                        $"CellTemplate must be set to a {typeof (DataGridViewProgressBarCell).Name} object.");
                }
                base.CellTemplate = value;
            }
        }

        [NotNull]
        private DataGridViewProgressBarCell ProgressBarCell
        {
            get
            {
                var result = (DataGridViewProgressBarCell) CellTemplate;
                if (result == null)
                {
                    throw new InvalidOperationException("CellTemplate is not set.");
                }
                return result;
            }
        }

        /// <summary>
        /// ProgressBar Max
        /// </summary>
        public int Maximum
        {
            get
            {
                return ProgressBarCell.Maximum;
            }
            set
            {
                if (Maximum != value)
                {
                    ProgressBarCell.Maximum = value;
                    if (DataGridView != null)
                    {
                        int rowCount = DataGridView.RowCount;
                        for (int i = 0; i < rowCount; i++)
                        {
                            DataGridViewRow r = DataGridView.Rows.SharedRow(i);
                            ((DataGridViewProgressBarCell) r.Cells[Index]).Maximum = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ProgressBar Min
        /// </summary>
        public int Minimum
        {
            get
            {
                return ProgressBarCell.Minimum;
            }
            set
            {
                if (Minimum != value)
                {
                    ProgressBarCell.Minimum = value;

                    if (DataGridView != null)
                    {
                        int rowCount = DataGridView.RowCount;
                        for (int i = 0; i < rowCount; i++)
                        {
                            DataGridViewRow r = DataGridView.Rows.SharedRow(i);
                            ((DataGridViewProgressBarCell) r.Cells[Index]).Minimum = value;
                        }
                    }
                }
            }
        }

        public DataGridViewProgressBarColumn()
        {
            CellTemplate = new DataGridViewProgressBarCell();
        }
    }
}