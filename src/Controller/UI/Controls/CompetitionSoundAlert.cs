using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Selectable sound file path with browse button and preview.
    /// </summary>
    public sealed partial class CompetitionSoundAlert : UserControl
    {
        [Category("Behavior")]
        [DefaultValue(typeof (ErrorProvider), null)]
        [CanBeNull]
        public ErrorProvider ErrorProvider { get; set; }

        [NotNull]
        public AlertSoundSourceItem Item
        {
            get
            {
                return new AlertSoundSourceItem(enabledCheckBox.Checked, pathTextBox.Text);
            }
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

        private void EnabledCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            soundPreviewButton.Enabled = enabledCheckBox.Checked;
            pathTextBox.Enabled = enabledCheckBox.Checked;
            browseButton.Enabled = enabledCheckBox.Checked;

            ValidateChildren();
        }

        private void SoundPreviewButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            SystemSound.PlayWaveFile(null);
            SystemSound.PlayWaveFile(pathTextBox.Text);
        }

        private void PathTextBox_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
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
                    ErrorProvider.SetError(pathTextBox,
                        pathTextBox.Text.Length > 0 && !File.Exists(pathTextBox.Text) ? "File not found." : string.Empty);
                }
            }
        }

        private void BrowseButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            BrowseForSound();
        }

        private void BrowseForSound()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = @"Select sound file";
                dialog.Filter = @"Wave files (*.wav)|*.wav|All files (*.*)|*.*";
                dialog.FileName = pathTextBox.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = dialog.FileName;
                    ValidateChildren();
                }
            }
        }
    }
}