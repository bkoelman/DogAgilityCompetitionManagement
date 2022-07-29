using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.UI.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls;

/// <summary>
/// Selectable picture file path with browse button and preview.
/// </summary>
public sealed partial class CompetitionPictureAlert : UserControl
{
    [Category("Behavior")]
    [DefaultValue(typeof(ErrorProvider), null)]
    public ErrorProvider? ErrorProvider { get; set; }

    [Category("Appearance")]
    [DefaultValue(typeof(string), "")]
    public string? AlertName { get; set; }

    public AlertPictureSourceItem Item
    {
        get => new(enabledCheckBox.Checked, pathTextBox.Text);
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

    private void EnabledCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        picturePreviewButton.Enabled = enabledCheckBox.Checked;
        pathTextBox.Enabled = enabledCheckBox.Checked;
        browseButton.Enabled = enabledCheckBox.Checked;

        ValidateChildren();
    }

    private void PicturePreviewButton_Click(object? sender, EventArgs e)
    {
        Assertions.IsNotNull(ParentForm, nameof(ParentForm));
        PicturePreviewForm.ShowPreview(pathTextBox.Text, AlertName ?? string.Empty, ParentForm);
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
                // Enabled or not: selected file must exist and be valid.
                ValidatePictureExistsAndCanBeLoaded(ErrorProvider);
            }
        }
    }

    private void ValidatePictureExistsAndCanBeLoaded(ErrorProvider errorProvider)
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

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        BrowseForPicture();
    }

    private void BrowseForPicture()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Select picture file",
            Filter = "All pictures (*.bmp;*.gif;*.jpg;*.png;*.tiff)|*.bmp;*.gif;*.jpg;*.png;*.tiff|Bitmap (*.bmp)|*.bmp|" +
                "Graphics Interchange Format (*.gif)|*.gif|Joint Photographic Experts Group (*.jpg)|*.jpg" +
                "|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tiff)|*.tiff|All files (*.*)|*.*",
            FileName = pathTextBox.Text
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            pathTextBox.Text = dialog.FileName;
            ValidateChildren();
        }
    }
}
