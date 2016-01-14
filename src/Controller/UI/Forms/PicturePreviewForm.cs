using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Dialog to preview a picture during configuration.
    /// </summary>
    public sealed partial class PicturePreviewForm : Form
    {
        public PicturePreviewForm()
        {
            InitializeComponent();
        }

        public static void ShowPreview([NotNull] string path, [NotNull] string title, [NotNull] Form parent)
        {
            Guard.NotNullNorEmpty(path, nameof(path));
            Guard.NotNullNorEmpty(title, nameof(title));
            Guard.NotNull(parent, nameof(parent));

            using (var form = new PicturePreviewForm())
            {
                form.pictureBox.ImageLocation = path;
                form.Text = title + @" preview";
                form.ShowDialog(parent);
            }
        }

        private void PicturePreviewForm_KeyDown([CanBeNull] object sender, [NotNull] KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}