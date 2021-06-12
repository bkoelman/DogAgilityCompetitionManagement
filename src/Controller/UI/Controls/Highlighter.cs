using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Applies fluent foreground color transition on another control.
    /// </summary>
    public sealed class Highlighter : Component
    {
        private static readonly Color DefaultHighlightColor = Color.White;

        private readonly Timer timer = new();

        private Color? nonHighlightingForeColor;
        private bool isHighlightEnabled;
        private int highlightSpeed = 50;
        private int indexInColorTable;
        private IReadOnlyCollection<Color>? colorTable;

        public Control? TargetControl { get; set; }
        public Color HighlightColor { get; set; }

        [DefaultValue(false)]
        public bool IsHighlightEnabled
        {
            get => isHighlightEnabled;
            set
            {
                if (value != isHighlightEnabled)
                {
                    if (!DesignMode)
                    {
                        if (value && highlightSpeed > 0)
                        {
                            StartHighlighting();
                        }
                        else
                        {
                            StopHighlighting();
                        }
                    }

                    isHighlightEnabled = value;
                }
            }
        }

        [DefaultValue(50)]
        public int HighlightSpeed
        {
            get => highlightSpeed;
            set
            {
                if (value != highlightSpeed)
                {
                    if (value < 0 || value > 500)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), value, "value must be in range [0-500].");
                    }

                    if (!DesignMode)
                    {
                        if (value > 0 && isHighlightEnabled)
                        {
                            StartHighlighting();
                        }
                        else
                        {
                            StopHighlighting();
                        }
                    }

                    highlightSpeed = value;
                }
            }
        }

        public event EventHandler? HighlightCycleFinished;

        public Highlighter()
        {
            timer.Interval = 100;
            timer.Tick += TimerOnTick;
            HighlightColor = DefaultHighlightColor;
        }

        private void StartHighlighting()
        {
            indexInColorTable = -1;
            timer.Enabled = true;
        }

        private void StopHighlighting()
        {
            timer.Enabled = false;

            if (nonHighlightingForeColor != null && TargetControl != null)
            {
                TargetControl.ForeColor = nonHighlightingForeColor.Value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            timer.Tick -= TimerOnTick;
            timer.Dispose();
            base.Dispose(disposing);
        }

        private void TimerOnTick(object? sender, EventArgs e)
        {
            if (timer.Enabled && TargetControl != null)
            {
                nonHighlightingForeColor ??= TargetControl.ForeColor;

                Color nextHighlightColor = GetNextHighlightColor(out bool isAtEndOfCycle);
                TargetControl.ForeColor = nextHighlightColor;

                if (isAtEndOfCycle)
                {
                    HighlightCycleFinished?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private Color GetNextHighlightColor(out bool isAtEndOfCycle)
        {
            colorTable ??= CreateColorTable();

            isAtEndOfCycle = false;

            indexInColorTable++;

            if (indexInColorTable == colorTable.Count)
            {
                indexInColorTable = 0;
                isAtEndOfCycle = true;
            }

            return colorTable.ElementAt(indexInColorTable);
        }

        private IReadOnlyCollection<Color> CreateColorTable()
        {
            int tableSize = 1000 / highlightSpeed;
            var builder = new ColorTableBuilder(HighlightColor, tableSize);
            indexInColorTable = -1;
            return builder.ColorTable;
        }

        private sealed class ColorTableBuilder
        {
            private readonly Lazy<IReadOnlyCollection<Color>> colorTableLazy;
            private readonly byte alpha;
            private readonly double hue;
            private readonly double lightness;
            private readonly double startSaturation;
            private readonly int tableSize;
            private readonly double stepSize;

            private bool directionIsUp;

            public IReadOnlyCollection<Color> ColorTable => colorTableLazy.Value;

            public ColorTableBuilder(Color startColor, int tableSize)
            {
                alpha = startColor.A;
                hue = startColor.GetHue();
                lightness = startColor.GetBrightness();
                startSaturation = startColor.GetSaturation();

                this.tableSize = tableSize;
                stepSize = 2 / (double)tableSize;

                colorTableLazy = new Lazy<IReadOnlyCollection<Color>>(CreateColorTable);
            }

            private IReadOnlyCollection<Color> CreateColorTable()
            {
                var table = new List<Color>();

                double currentSaturation = startSaturation;

                for (int i = 0; i < tableSize; i++)
                {
                    currentSaturation = GetNextValue(currentSaturation);
                    Color nextColor = ColorConverter.FromAhsl(alpha, hue, currentSaturation, lightness);
                    table.Add(nextColor);
                }

                return new ReadOnlyCollection<Color>(table);
            }

            private double GetNextValue(double value)
            {
                if (value - stepSize < 0.0)
                {
                    directionIsUp = true;
                }
                else if (value + stepSize > 1.0)
                {
                    directionIsUp = false;
                }

                value = directionIsUp ? value + stepSize : value - stepSize;
                return value;
            }
        }
    }
}
