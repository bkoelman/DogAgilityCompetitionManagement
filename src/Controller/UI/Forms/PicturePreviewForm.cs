using System.Windows.Forms;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.UI.Forms;

/// <summary>
/// Dialog to preview a picture during configuration.
/// </summary>
public sealed partial class PicturePreviewForm : Form
{
    public PicturePreviewForm()
    {
        InitializeComponent();
    }

    public static void ShowPreview(string path, string title, Form parent)
    {
        Guard.NotNullNorEmpty(path, nameof(path));
        Guard.NotNullNorEmpty(title, nameof(title));
        Guard.NotNull(parent, nameof(parent));

        using var form = new PicturePreviewForm
        {
            pictureBox =
            {
                ImageLocation = path
            },
            Text = $"{title} preview"
        };

        form.ShowDialog(parent);
    }

    private void PicturePreviewForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }
}
