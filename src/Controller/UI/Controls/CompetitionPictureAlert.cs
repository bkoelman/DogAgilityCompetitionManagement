using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.UI.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Selectable picture file path with browse button and preview.
    /// </summary>
    public sealed partial class CompetitionPictureAlert : UserControl
    {
        [Category("Behavior")]
        [DefaultValue(typeof (ErrorProvider), null)]
        [CanBeNull]
        public ErrorProvider ErrorProvider { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof (string), "")]
        [CanBeNull]
        public string AlertName { get; set; }

        [NotNull]
        public AlertPictureSourceItem Item
        {
            get
            {
                return new AlertPictureSourceItem(enabledCheckBox.Checked, pathTextBox.Text);
            }
            set
            {
                Guard.NotNull(value, nameof(value));

                pathTextBox.Text = value.Path;
                enabledCheckBox.Checked = value.IsEnabled;
            }
        }

        public CompetitionPictureAlert()
        {
            InitializeComponent();

            AlertName = string.Empty;
        }

        private void EnabledCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            picturePreviewButton.Enabled = enabledCheckBox.Checked;
            pathTextBox.Enabled = enabledCheckBox.Checked;
            browseButton.Enabled = enabledCheckBox.Checked;

            ValidateChildren();
        }

        private void PicturePreviewButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            Form parentFormNotNull = Assertions.InternalValueIsNotNull(() => ParentForm, () => ParentForm);
            PicturePreviewForm.ShowPreview(pathTextBox.Text, AlertName ?? string.Empty, parentFormNotNull);
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
                    // Enabled or not: selected file must exist and be valid.
                    ValidatePictureExistsAndCanBeLoaded(ErrorProvider);
                }
            }
        }

        private void ValidatePictureExistsAndCanBeLoaded([NotNull] ErrorProvider errorProvider)
        {
            if (pathTextBox.Text.Length > 0)
            {
                if (!File.Exists(pathTextBox.Text))
                {
                    errorProvider.SetError(pathTextBox, "File not found.");
                    return;
                }

                try
                {
                    using (new Bitmap(pathTextBox.Text))
                    {
                    }
                }
                catch (Exception)
                {
                    errorProvider.SetError(pathTextBox, "Invalid picture file.");
                    return;
                }
            }

            errorProvider.SetError(pathTextBox, string.Empty);
        }

        private void BrowseButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            BrowseForPicture();
        }

        private void BrowseForPicture()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = @"Select picture file";
                dialog.Filter =
                    @"All pictures (*.bmp;*.gif;*.jpg;*.png;*.tiff)|*.bmp;*.gif;*.jpg;*.png;*.tiff|Bitmap (*.bmp)|*.bmp|" +
                        @"Graphics Interchange Format (*.gif)|*.gif|Joint Photographic Experts Group (*.jpg)|*.jpg" +
                        @"|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tiff)|*.tiff|All files (*.*)|*.*";
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