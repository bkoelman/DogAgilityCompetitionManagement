using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A <see cref="DataGridView" /> that uses double buffering to prevent screen flicker.
    /// </summary>
    public sealed class NonFlickeringDataGridView : DataGridView
    {
        public NonFlickeringDataGridView()
        {
            DoubleBuffered = true;
        }

        public bool PublicShowFocusCues => ShowFocusCues;
    }
}