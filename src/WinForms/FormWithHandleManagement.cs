using System.Windows.Forms;

namespace DogAgilityCompetition.WinForms;

/// <summary>
/// A <see cref="Form" /> that provides a way to enforce creating its underlying windows handle.
/// </summary>
public class FormWithHandleManagement : Form
{
    protected void EnsureHandleCreated()
    {
        if (!IsHandleCreated)
        {
            CreateHandle();
        }
    }
}
