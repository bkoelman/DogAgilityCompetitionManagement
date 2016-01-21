using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// An animated panel, whose contents automatically scrolls up and down slowly.
    /// </summary>
    public sealed class AutoScrollingPanel : Panel
    {
        private const int InitialScrollStepSize = -1;
        private const int UpdateIntervalInMilliseconds = 10;

        [NotNull]
        private readonly Panel innerPanel = new Panel();

        [NotNull]
        private readonly Timer timer;

        [CanBeNull]
        private Bitmap innerControlsBitmap;

        private int scrollOffset;
        private int scrollStepSize = InitialScrollStepSize;
        private bool isUpdating;

        public AutoScrollingPanel()
        {
            DoubleBuffered = true;

            timer = new Timer { Interval = UpdateIntervalInMilliseconds };
            timer.Tick += TimerOnTick;
            timer.Enabled = true;
        }

        public void ClearInnerControls()
        {
            // This is a pretty classic WinForms bug, many programmers have been bitten by it. Disposing a control 
            // also removes it from the parent's Control collection. Most .NET collection classes trigger an 
            // InvalidOperationException when iterating them changes the collection but that wasn't done for the 
            // ControlCollection class. The effect is that your for-each loop skips elements, it only disposes the 
            // even-numbered controls.
            // This problem is extra-specially nasty because the garbage collector will not finalize the controls. 
            // After the native window handle for a control is created, it will stay referenced by an internal table 
            // that maps Window handles to controls. Only destroying the native window removes the reference from 
            // that table. That never happens in code like this, calling Dispose() is a hard requirement. 
            // Very unusual in .NET.
            // The solution is to iterate the Controls collection backwards so that disposing controls doesn't affect 
            // what you iterate.

            for (int index = innerPanel.Controls.Count - 1; index >= 0; index--)
            {
                innerPanel.Controls[index].Dispose();
            }
            innerPanel.Controls.Clear();
        }

        public void AddToInnerControls([NotNull] Control control)
        {
            innerPanel.Controls.Add(control);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                timer.Dispose();
                innerPanel.Dispose();

                if (innerControlsBitmap != null)
                {
                    innerControlsBitmap.Dispose();
                    innerControlsBitmap = null;
                }
            }
        }

        [CanBeNull]
        private DateTime? lastHitBoundaryTime;

        private void TimerOnTick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (isUpdating || !Visible)
            {
                return;
            }

            if (RequiresScrolling())
            {
                if (scrollOffset == 0 || HasScrolledToEnd())
                {
                    if (lastHitBoundaryTime == null)
                    {
                        lastHitBoundaryTime = SystemContext.UtcNow();
                        return;
                    }
                    if (lastHitBoundaryTime.Value.AddSeconds(1) > SystemContext.UtcNow())
                    {
                        return;
                    }

                    scrollStepSize *= -1;
                    lastHitBoundaryTime = null;
                }

                scrollOffset += scrollStepSize;
                Invalidate();
            }
        }

        private bool RequiresScrolling()
        {
            Bitmap bitmap = EnsureBitmap();
            return bitmap != null && innerPanel.Height < bitmap.Height;
        }

        private bool HasScrolledToEnd()
        {
            Bitmap bitmap = EnsureBitmap();
            return bitmap != null && scrollOffset + innerPanel.Height >= bitmap.Height;
        }

        protected override void OnSizeChanged([NotNull] EventArgs e)
        {
            base.OnSizeChanged(e);

            innerPanel.Width = Width;
            innerPanel.Height = Height;
            innerPanel.Anchor = Anchor;
            Reset();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Guard.NotNull(e, nameof(e));
            base.OnPaint(e);

            Bitmap bitmap = EnsureBitmap();
            if (bitmap != null)
            {
                e.Graphics.DrawImage(bitmap, 0, -scrollOffset);
            }
        }

        [CanBeNull]
        private Bitmap EnsureBitmap()
        {
            if (innerControlsBitmap == null)
            {
                // No idea why, but this appears needed for correct height.
                const int visualHeightCorrection = 4;

                int completeWidth = innerPanel.PreferredSize.Width;
                int completeHeight = innerPanel.PreferredSize.Height - visualHeightCorrection;

                if (completeWidth == 0 || completeHeight == 0)
                {
                    return null;
                }

                innerControlsBitmap = new Bitmap(completeWidth, completeHeight);

                using (new TemporarySizeChanger(innerPanel))
                {
                    innerPanel.Width = completeWidth;
                    innerPanel.Height = completeHeight;

                    innerPanel.DrawToBitmap(innerControlsBitmap, new Rectangle(0, 0, completeWidth, completeHeight));
                }

                DrawMissingBorderOnPanels(innerControlsBitmap);

                // Uncomment next line to dump to disk
                //innerControlsBitmap.Save(@"d:\test.png");
            }

            return innerControlsBitmap;
        }

        private void DrawMissingBorderOnPanels([NotNull] Bitmap controlsBitmap)
        {
            // Bug workaround: For some unknown reason the FixedSingle border style of Panels is not rendered
            // when using Panel.DrawToBitmap. So we draw them ourselves.
            using (Graphics graphics = Graphics.FromImage(controlsBitmap))
            {
                foreach (RunHistoryLine runHistoryLine in innerPanel.Controls.OfType<RunHistoryLine>())
                {
                    int offsetY = runHistoryLine.Location.Y;
                    int offsetX = runHistoryLine.Location.X;

                    foreach (Panel panel in runHistoryLine.Controls.OfType<Panel>())
                    {
                        graphics.DrawRectangle(SystemPens.ControlDarkDark, panel.Location.X + offsetX,
                            panel.Location.Y + offsetY, panel.ClientSize.Width + 1, panel.ClientSize.Height);
                    }
                }
            }
        }

        protected override void OnVisibleChanged([NotNull] EventArgs e)
        {
            base.OnVisibleChanged(e);

            timer.Enabled = Visible && !DesignMode;
        }

        public void BeginUpdate()
        {
            isUpdating = true;
        }

        public void EndUpdate()
        {
            isUpdating = false;
            Reset();
        }

        private void Reset()
        {
            scrollOffset = 0;
            scrollStepSize = InitialScrollStepSize;

            if (innerControlsBitmap != null)
            {
                innerControlsBitmap.Dispose();
                innerControlsBitmap = null;
            }

            Invalidate();
        }

        private sealed class TemporarySizeChanger : IDisposable
        {
            [NotNull]
            private readonly Control control;

            private readonly int backupWidth;
            private readonly int backupHeight;

            public TemporarySizeChanger([NotNull] Control control)
            {
                Guard.NotNull(control, nameof(control));

                this.control = control;
                backupWidth = control.Width;
                backupHeight = control.Height;
            }

            public void Dispose()
            {
                control.Width = backupWidth;
                control.Height = backupHeight;
            }
        }
    }
}