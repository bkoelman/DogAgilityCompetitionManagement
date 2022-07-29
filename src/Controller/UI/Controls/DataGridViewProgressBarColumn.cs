using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls;

/// <summary>
/// A column in a <see cref="DataGridView" /> that contains a colored progress bar.
/// </summary>
public sealed class DataGridViewProgressBarColumn : DataGridViewTextBoxColumn
{
    private DataGridViewProgressBarCell ProgressBarCell
    {
        get
        {
            var result = CellTemplate as DataGridViewProgressBarCell;

            if (result == null)
            {
                throw new InvalidOperationException("CellTemplate is not properly set.");
            }

            return result;
        }
    }

    /// <summary>
    /// ProgressBar Max
    /// </summary>
    public int Maximum
    {
        get => ProgressBarCell.Maximum;
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
                        DataGridViewRow? r = DataGridView.Rows.SharedRow(i);
                        ((DataGridViewProgressBarCell)r.Cells[Index]).Maximum = value;
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
        get => ProgressBarCell.Minimum;
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
                        DataGridViewRow? r = DataGridView.Rows.SharedRow(i);
                        ((DataGridViewProgressBarCell)r.Cells[Index]).Minimum = value;
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
