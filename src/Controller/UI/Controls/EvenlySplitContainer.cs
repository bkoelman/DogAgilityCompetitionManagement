using System;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A <see cref="SplitContainer" /> whose contents is distributed evenly.
    /// </summary>
    public sealed class EvenlySplitContainer : SplitContainer
    {
        public EvenlySplitContainer()
        {
            IsSplitterFixed = true;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ResizePanels();
        }

        private void ResizePanels()
        {
            int distance = Orientation == Orientation.Vertical ? ClientSize.Width : ClientSize.Height;
            SplitterDistance = distance / 2;
        }

        protected override void OnResize(EventArgs e)
        {
            ResizePanels();
            base.OnResize(e);
        }
    }
}
