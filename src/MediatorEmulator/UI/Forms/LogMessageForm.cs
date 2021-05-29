using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// Enables setting a log message for transmission over the CIRCE connection.
    /// </summary>
    public sealed partial class LogMessageForm : Form
    {
        [NotNull]
        public string Message
        {
            get => messageTextBox.Text;
            set => messageTextBox.Text = value;
        }

        public LogMessageForm()
        {
            InitializeComponent();
        }

        private void LogMessageForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            messageTextBox.SelectAll();
            messageTextBox.Select();
        }

        private void OkButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
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
}
