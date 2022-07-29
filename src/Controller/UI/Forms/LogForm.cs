using System.Windows.Forms;
using DogAgilityCompetition.Controller.Properties;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.Controller.UI.Forms;

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

    private void LogForm_Load(object? sender, EventArgs e)
    {
        LoadSettingsForDebug();
    }

    private void LoadSettingsForDebug()
    {
        if (Enum.TryParse(Settings.Default.DebugTextBoxMode, false, out TextBoxAppenderMode mode))
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

        if (Enum.TryParse(Settings.Default.DebugTextBoxSwitches, false, out TextBoxAppenderSwitches switches))
        {
            hideLockSleepCheckBox.Checked = (switches & TextBoxAppenderSwitches.HideLockSleep) != 0;
        }

        if (bool.TryParse(Settings.Default.DebugTextBoxIsFrozen, out bool isFrozen))
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

    private void PacketsRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        TextBoxAppender.Mode = TextBoxAppenderMode.Packets;
    }

    private void NetworkRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        TextBoxAppender.Mode = TextBoxAppenderMode.Network;
    }

    private void NonNetworkRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        TextBoxAppender.Mode = TextBoxAppenderMode.NonNetwork;
    }

    private void AllRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        TextBoxAppender.Mode = TextBoxAppenderMode.All;
    }

    private void HideLockSleepCheckBox_CheckedChanged(object? sender, EventArgs e)
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

    private void FreezeLogTextBox_CheckedChanged(object? sender, EventArgs e)
    {
        TextBoxAppender.IsFrozen = freezeLogTextBox.Checked;
    }

    private void CopyLogLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        Clipboard.SetText(logTextBox.Text);
    }

    private void ClearLogLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        logTextBox.Text = string.Empty;
    }

    private void LogForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        Visible = false;
        e.Cancel = true;
    }
}
