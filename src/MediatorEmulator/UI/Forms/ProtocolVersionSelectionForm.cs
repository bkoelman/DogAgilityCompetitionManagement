using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// Enables configuration of mediator protocol version.
    /// </summary>
    public sealed partial class ProtocolVersionSelectionForm : Form
    {
        [CanBeNull]
        public Version Version
        {
            get
            {
                int majorNumber = (int)majorUpDown.Value;
                int minorNumber = (int)minorUpDown.Value;
                int releaseNumber = (int)releaseUpDown.Value;

                return IsInRange(majorNumber) && IsInRange(minorNumber) && IsInRange(releaseNumber)
                    ? new Version(majorNumber, minorNumber, releaseNumber)
                    : null;
            }
            set
            {
                majorUpDown.Value = value?.Major ?? 0;
                minorUpDown.Value = value?.Minor ?? 0;
                releaseUpDown.Value = value?.Build ?? 0;
            }
        }

        public ProtocolVersionSelectionForm()
        {
            InitializeComponent();
        }

        private static bool IsInRange(int value)
        {
            return value is >= 0 and <= 999;
        }

        private void OkButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (Version == null)
            {
                MessageBox.Show("All values must be in range [0-999].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
