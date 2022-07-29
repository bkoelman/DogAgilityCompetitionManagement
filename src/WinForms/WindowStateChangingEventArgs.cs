using System.Windows.Forms;

namespace DogAgilityCompetition.WinForms;

public sealed class WindowStateChangingEventArgs : EventArgs
{
    public FormWindowState NewState { get; }
    public bool Cancel { get; set; }

    public WindowStateChangingEventArgs(FormWindowState newState)
    {
        NewState = newState;
    }
}
