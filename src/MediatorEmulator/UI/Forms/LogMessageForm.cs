using System.Windows.Forms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms;

/// <summary>
/// Enables setting a log message for transmission over the CIRCE connection.
/// </summary>
public sealed partial class LogMessageForm : Form
{
    public string Message
    {
        get => messageTextBox.Text;
        set => messageTextBox.Text = value;
    }

    public LogMessageForm()
    {
        InitializeComponent();
    }

    private void LogMessageForm_Load(object? sender, EventArgs e)
    {
        messageTextBox.SelectAll();
        messageTextBox.Select();
    }

    private void OkButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Message))
        {
            MessageBox.Show("No message specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            DialogResult = DialogResult.OK;
        }
    }
}
