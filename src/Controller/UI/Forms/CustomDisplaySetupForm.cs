using System.Windows.Forms;
using DogAgilityCompetition.Controller.Properties;

namespace DogAgilityCompetition.Controller.UI.Forms;

/// <summary>
/// Enables configuration of text displayed on <see cref="CustomDisplayForm" />.
/// </summary>
public sealed partial class CustomDisplaySetupForm : Form
{
    public CustomDisplaySetupForm()
    {
        InitializeComponent();
    }

    private void CustomDisplaySetupForm_Load(object? sender, EventArgs e)
    {
        LoadSettings();
    }

    private void PictureRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        picturePathTextBox.Enabled = pictureRadioButton.Checked;
        browseButton.Enabled = pictureRadioButton.Checked;
        textGroupBox.Enabled = textRadioButton.Checked;
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            FileName = picturePathTextBox.Text.Trim(),
            Title = "Select picture file",
            Filter = "Image files (*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png)|*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png|All files|*.*"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            picturePathTextBox.Text = dialog.FileName;
        }
    }

    private void TopLineRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        firstLineTextBox.Enabled = firstLineRadioButton.Checked;
    }

    private void OkButton_Click(object? sender, EventArgs e)
    {
        SaveSettings();
        DialogResult = DialogResult.OK;
    }

    private void LoadSettings()
    {
        (Settings.Default.CustomDisplayModeIsText ? textRadioButton : pictureRadioButton).Checked = true;
        picturePathTextBox.Text = Settings.Default.CustomDisplayPicturePath;
        RadioButton? radioButton = Settings.Default.CustomDisplayModeFirstLineIsSystemTime ? systemTimeRadioButton : firstLineRadioButton;
        radioButton.Checked = true;
        firstLineTextBox.Text = Settings.Default.CustomDisplayFirstLine;
        secondLineTextBox.Text = Settings.Default.CustomDisplaySecondLine;
        thirdLineTextBox.Text = Settings.Default.CustomDisplayThirdLine;
    }

    private void SaveSettings()
    {
        Settings.Default.CustomDisplayModeIsText = textRadioButton.Checked;
        Settings.Default.CustomDisplayPicturePath = picturePathTextBox.Text.Trim();
        Settings.Default.CustomDisplayModeFirstLineIsSystemTime = systemTimeRadioButton.Checked;
        Settings.Default.CustomDisplayFirstLine = firstLineTextBox.Text.Trim();
        Settings.Default.CustomDisplaySecondLine = secondLineTextBox.Text.Trim();
        Settings.Default.CustomDisplayThirdLine = thirdLineTextBox.Text.Trim();
    }
}
