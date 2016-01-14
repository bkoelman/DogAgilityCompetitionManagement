using System;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.Properties;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Displays real-time application logging with some filtering options.
    /// </summary>
    public sealed partial class LogForm : FormWithHandleManagement
    {
        public LogForm()
        {
            InitializeComponent();

            EnsureHandleCreated();

            TextBoxAppender.Subscribe(logTextBox);
        }

        private void LogForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            LoadSettingsForDebug();
        }

        private void LoadSettingsForDebug()
        {
            TextBoxAppenderMode mode;
            if (Enum.TryParse(Settings.Default.DebugTextBoxMode, false, out mode))
            {
                switch (mode)
                {
                    case TextBoxAppenderMode.Packets:
                        packetsRadioButton.Checked = true;
                        break;
                    case TextBoxAppenderMode.Network:
                        networkRadioButton.Checked = true;
                        break;
                    case TextBoxAppenderMode.NonNetwork:
                        nonNetworkRadioButton.Checked = true;
                        break;
                    case TextBoxAppenderMode.All:
                        allRadioButton.Checked = true;
                        break;
                }
            }

            TextBoxAppenderSwitches switches;
            if (Enum.TryParse(Settings.Default.DebugTextBoxSwitches, false, out switches))
            {
                hideLockSleepCheckBox.Checked = (switches & TextBoxAppenderSwitches.HideLockSleep) != 0;
            }

            bool isFrozen;
            if (bool.TryParse(Settings.Default.DebugTextBoxIsFrozen, out isFrozen))
            {
                freezeLogTextBox.Checked = isFrozen;
            }
        }

        public void PulseInputLed()
        {
            packetInputPulsingLed.On = !packetInputPulsingLed.On;
        }

        public void PulseOutputLed()
        {
            packetOutputPulsingLed.On = !packetOutputPulsingLed.On;
        }

        private void PacketsRadioButton_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            TextBoxAppender.Mode = TextBoxAppenderMode.Packets;
        }

        private void NetworkRadioButton_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            TextBoxAppender.Mode = TextBoxAppenderMode.Network;
        }

        private void NonNetworkRadioButton_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            TextBoxAppender.Mode = TextBoxAppenderMode.NonNetwork;
        }

        private void AllRadioButton_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            TextBoxAppender.Mode = TextBoxAppenderMode.All;
        }

        private void HideLockSleepCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            TextBoxAppenderSwitches switches = TextBoxAppender.Switches;
            if (hideLockSleepCheckBox.Checked)
            {
                switches |= TextBoxAppenderSwitches.HideLockSleep;
            }
            else
            {
                switches &= ~TextBoxAppenderSwitches.HideLockSleep;
            }
            TextBoxAppender.Switches = switches;
        }

        private void FreezeLogTextBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            TextBoxAppender.IsFrozen = freezeLogTextBox.Checked;
        }

        private void CopyLogLinkLabel_LinkClicked([CanBeNull] object sender, [NotNull] LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(logTextBox.Text);
        }

        private void ClearLog_LinkClicked([CanBeNull] object sender, [NotNull] LinkLabelLinkClickedEventArgs e)
        {
            logTextBox.Text = string.Empty;
        }

        private void LogForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }
    }
}