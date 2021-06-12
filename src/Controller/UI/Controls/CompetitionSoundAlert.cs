using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Selectable sound file path with browse button and preview.
    /// </summary>
    public sealed partial class CompetitionSoundAlert : UserControl
    {
        [Category("Behavior")]
        [DefaultValue(typeof(ErrorProvider), null)]
        public ErrorProvider? ErrorProvider { get; set; }

        public AlertSoundSourceItem Item
        {
            get => new(enabledCheckBox.Checked, pathTextBox.Text);
            set
            {
                Guard.NotNull(value, nameof(value));

                pathTextBox.Text = value.Path;
                enabledCheckBox.Checked = value.IsEnabled;
            }
        }

        public CompetitionSoundAlert()
        {
            InitializeComponent();
        }

        private void EnabledCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            soundPreviewButton.Enabled = enabledCheckBox.Checked;
            pathTextBox.Enabled = enabledCheckBox.Checked;
            browseButton.Enabled = enabledCheckBox.Checked;

            ValidateChildren();
        }

        private void SoundPreviewButton_Click(object? sender, EventArgs e)
        {
            SystemSound.PlayWaveFile(null);
            SystemSound.PlayWaveFile(pathTextBox.Text);
        }

        private void PathTextBox_Validating(object? sender, CancelEventArgs e)
        {
            if (ErrorProvider != null)
            {
                if (enabledCheckBox.Checked && pathTextBox.Text.Length == 0)
                {
                    ErrorProvider.SetError(pathTextBox, "No file selected.");
                }
                else
                {
                    // Enabled or not: selected file must exist.
                    ErrorProvider.SetError(pathTextBox, pathTextBox.Text.Length > 0 && !File.Exists(pathTextBox.Text) ? "File not found." : string.Empty);
                }
            }
        }

        private void BrowseButton_Click(object? sender, EventArgs e)
        {
            BrowseForSound();
        }

        private void BrowseForSound()
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select sound file",
                Filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*",
                FileName = pathTextBox.Text
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = dialog.FileName;
                ValidateChildren();
            }
        }
    }
}
