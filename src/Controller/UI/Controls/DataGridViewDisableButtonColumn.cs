using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A column in a <see cref="DataGridView" /> that contains a button which can be enabled/disabled.
    /// </summary>
    public sealed class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
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
                if (!(value is DataGridViewDisableButtonCell))
                {
                    throw new InvalidOperationException(
                        $"CellTemplate must be set to a {typeof (DataGridViewDisableButtonCell).Name} object.");
                }
                base.CellTemplate = value;
            }
        }

        [NotNull]
        private DataGridViewDisableButtonCell DisableButtonCell
        {
            get
            {
                var result = (DataGridViewDisableButtonCell) CellTemplate;
                if (result == null)
                {
                    throw new InvalidOperationException("CellTemplate is not set.");
                }
                return result;
            }
        }

        public bool Enabled
        {
            get
            {
                return DisableButtonCell.Enabled;
            }
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
                            DataGridViewRow r = DataGridView.Rows.SharedRow(i);
                            ((DataGridViewDisableButtonCell) r.Cells[Index]).Enabled = value;
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