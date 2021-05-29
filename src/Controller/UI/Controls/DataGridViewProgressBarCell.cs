using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// ProgressBar in DataGridView
    /// </summary>
    public sealed class DataGridViewProgressBarCell : DataGridViewTextBoxCell
    {
        private static readonly Color GreenColor = Color.FromArgb(255, 0, 192, 0);
        private static readonly Color RedColor = Color.FromArgb(255, 192, 0, 0);
        private static readonly Color YellowColor = Color.FromArgb(255, 192, 192, 0);

        /// <summary>
        /// ProgressBar Max
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// ProgressBar Min
        /// </summary>
        public int Minimum { get; set; }

        [NotNull]
        public override Type ValueType => typeof(int);

        public override object DefaultNewRowValue => 0;

        public DataGridViewProgressBarCell()
        {
            Maximum = 100;
            Minimum = 0;
        }

        public override object Clone()
        {
            var cell = (DataGridViewProgressBarCell)base.Clone();
            cell.Maximum = Maximum;
            cell.Minimum = Minimum;
            return cell;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
            [CanBeNull] object value, [CanBeNull] object formattedValue, [CanBeNull] string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Guard.NotNull(graphics, nameof(graphics));
            Guard.NotNull(cellStyle, nameof(cellStyle));
            Guard.NotNull(advancedBorderStyle, nameof(advancedBorderStyle));

            double rate = CalculateRate(value);

            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }

            Rectangle borderRect = BorderWidths(advancedBorderStyle);

            var paintRect = new Rectangle(cellBounds.Left + borderRect.Left, cellBounds.Top + borderRect.Top, cellBounds.Width - borderRect.Right,
                cellBounds.Height - borderRect.Bottom);

            bool isSelected = (cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;

            Color backColor;

            if (isSelected && (paintParts & DataGridViewPaintParts.SelectionBackground) == DataGridViewPaintParts.SelectionBackground)
            {
                backColor = cellStyle.SelectionBackColor;
            }
            else
            {
                backColor = cellStyle.BackColor;
            }

            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                using (var backBrush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(backBrush, paintRect);
                }
            }

            paintRect.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
            paintRect.Width -= cellStyle.Padding.Horizontal;
            paintRect.Height -= cellStyle.Padding.Vertical;

            if ((paintParts & DataGridViewPaintParts.ContentForeground) == DataGridViewPaintParts.ContentForeground)
            {
                graphics.FillRectangle(Brushes.White, paintRect);
                graphics.DrawRectangle(Pens.Black, paintRect);
                var barBounds = new Rectangle(paintRect.Left + 1, paintRect.Top + 1, paintRect.Width - 1, paintRect.Height - 1);
                barBounds.Width = (int)Math.Round(barBounds.Width * rate);

                using (var fillBrush = new SolidBrush(rate <= 0.25 ? RedColor : rate <= 0.5 ? YellowColor : GreenColor))
                {
                    graphics.FillRectangle(fillBrush, barBounds);
                }
            }

            if (DataGridView.CurrentCellAddress.X == ColumnIndex && DataGridView.CurrentCellAddress.Y == RowIndex &&
                (paintParts & DataGridViewPaintParts.Focus) == DataGridViewPaintParts.Focus && DataGridView.Focused && ReflectShowFocusCues())
            {
                Rectangle focusRect = paintRect;
                focusRect.Inflate(-3, -3);
                ControlPaint.DrawFocusRectangle(graphics, focusRect);
            }

            if ((paintParts & DataGridViewPaintParts.ContentForeground) == DataGridViewPaintParts.ContentForeground)
            {
                string text = $"{100.0 * rate:#,0}%";
                paintRect.Inflate(-2, -2);

                TextRenderer.DrawText(graphics, text, cellStyle.Font, paintRect, cellStyle.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            if ((paintParts & DataGridViewPaintParts.ErrorIcon) == DataGridViewPaintParts.ErrorIcon && DataGridView.ShowCellErrors &&
                !string.IsNullOrEmpty(errorText))
            {
                Rectangle iconBounds = GetErrorIconBounds(graphics, cellStyle, rowIndex);
                iconBounds.Offset(cellBounds.X, cellBounds.Y);
                PaintErrorIcon(graphics, iconBounds, cellBounds, errorText);
            }
        }

        private double CalculateRate([CanBeNull] object cellValue)
        {
            int value = 0;

            if (cellValue is int)
            {
                value = (int)cellValue;
            }

            if (value < Minimum)
            {
                value = Minimum;
            }

            if (value > Maximum)
            {
                value = Maximum;
            }

            double rate = (double)(value - Minimum) / (Maximum - Minimum);
            return rate;
        }

        private bool ReflectShowFocusCues()
        {
            var customGridView = DataGridView as NonFlickeringDataGridView;

            if (customGridView != null)
            {
                return customGridView.PublicShowFocusCues;
            }

            // Note: The cell should only show the focus rectangle when this.DataGridView.ShowFocusCues is true. 
            // However that property is protected internal and can't be accessed directly.

            PropertyInfo propertyInfo = DataGridView.GetType().GetProperty("ShowFocusCues", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            bool showFocusCues = (bool)getMethodInfo.Invoke(DataGridView, new object[0]);
            return showFocusCues;
        }
    }
}
