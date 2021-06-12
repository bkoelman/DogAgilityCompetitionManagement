using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A <see cref="DataGridView" /> that uses double buffering to prevent screen flicker.
    /// </summary>
    public sealed class NonFlickeringDataGridView : DataGridView
    {
        public bool PublicShowFocusCues => ShowFocusCues;

        public NonFlickeringDataGridView()
        {
            DoubleBuffered = true;
        }
    }
}
