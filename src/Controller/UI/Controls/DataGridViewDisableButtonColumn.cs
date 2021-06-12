using System;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A column in a <see cref="DataGridView" /> that contains a button which can be enabled/disabled.
    /// </summary>
    public sealed class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        private DataGridViewDisableButtonCell DisableButtonCell
        {
            get
            {
                var result = CellTemplate as DataGridViewDisableButtonCell;

                if (result == null)
                {
                    throw new InvalidOperationException("CellTemplate is not properly set.");
                }

                return result;
            }
        }

        public bool Enabled
        {
            get => DisableButtonCell.Enabled;
            set
            {
                if (Enabled != value)
                {
                    DisableButtonCell.Enabled = value;

                    if (DataGridView != null)
                    {
                        int rowCount = DataGridView.RowCount;

                        for (int i = 0; i < rowCount; i++)
                        {
                            DataGridViewRow? r = DataGridView.Rows.SharedRow(i);
                            ((DataGridViewDisableButtonCell)r.Cells[Index]).Enabled = value;
                        }
                    }
                }
            }
        }

        public DataGridViewDisableButtonColumn()
        {
            CellTemplate = new DataGridViewDisableButtonCell();
        }
    }
}
