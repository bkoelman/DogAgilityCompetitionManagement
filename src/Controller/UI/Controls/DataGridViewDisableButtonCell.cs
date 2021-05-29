using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    public sealed class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        public bool Enabled { get; set; }

        public DataGridViewDisableButtonCell()
        {
            Enabled = true;
        }

        public override object Clone()
        {
            var cell = (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = Enabled;
            return cell;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState,
            [CanBeNull] object value, [CanBeNull] object formattedValue, [CanBeNull] string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Guard.NotNull(graphics, nameof(graphics));
            Guard.NotNull(cellStyle, nameof(cellStyle));
            Guard.NotNull(advancedBorderStyle, nameof(advancedBorderStyle));

            if (!Enabled)
            {
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    using (var cellBackground = new SolidBrush(cellStyle.BackColor))
                    {
                        graphics.FillRectangle(cellBackground, cellBounds);
                    }
                }

                if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
                }

                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment = BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Bug workaround: designer-made changes are not propagated to the incoming advancedBorderStyle here.
                // So correct for padding manually.
                Rectangle contentRect = GetContentBounds(graphics, cellStyle, rowIndex);
                buttonArea.X += contentRect.X;
                buttonArea.Y += contentRect.Y;
                buttonArea.Height = contentRect.Height;
                buttonArea.Width = contentRect.Width;

                ButtonRenderer.DrawButton(graphics, buttonArea, PushButtonState.Disabled);

                string buttonText = FormattedValue as string;

                if (buttonText != null)
                {
                    TextRenderer.DrawText(graphics, buttonText, DataGridView.Font, buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
                    paintParts);
            }
        }
    }
}
