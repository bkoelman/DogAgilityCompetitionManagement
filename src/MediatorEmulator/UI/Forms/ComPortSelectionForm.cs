using System.Linq;
using System.Windows.Forms;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// Enables configuration of the COM port to use.
    /// </summary>
    public sealed partial class ComPortSelectionForm : Form
    {
        public const string AutoText = "(Auto)";

        public string? ComPortName
        {
            get => portNameComboBox.Text == AutoText ? null : portNameComboBox.Text;
            set => portNameComboBox.Text = value == null || portNameComboBox.Items.IndexOf(value) == -1 ? AutoText : value;
        }

        public ComPortSelectionForm()
        {
            InitializeComponent();

            portNameComboBox.Items.Add(AutoText);
            portNameComboBox.Items.AddRange(SystemPortProvider.GetAllComPorts().Cast<object>().ToArray());
            portNameComboBox.SelectedIndex = 0;
        }
    }
}
