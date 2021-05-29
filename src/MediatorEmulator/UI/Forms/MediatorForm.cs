using System;
using System.Text;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Mediator;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// An MDI child that represents an emulated wireless mediator.
    /// </summary>
    public sealed partial class MediatorForm : FormWithWindowStateChangeEvent, IWirelessDevice
    {
        [NotNull]
        private readonly MediatorSettingsXml settings;

        private readonly bool initiallyMaximized;

        [NotNull]
        private readonly FreshNotNullableReference<CirceMediatorSessionManager> sessionManager;

        // Prevents endless recursion when updating controls that raise change events.
        private bool isUpdatingControlsFromSettings;

        private bool forceClose;

        bool IWirelessDevice.IsPoweredOn => powerStatus.ThreadSafeIsPoweredOn;

        WirelessNetworkAddress IWirelessDevice.Address => settings.DeviceAddressNotNull;

        public MediatorForm([NotNull] MediatorSettingsXml mediatorSettings, bool initiallyMaximized,
            [NotNull] CirceMediatorSessionManager mediatorSessionManager)
        {
            Guard.NotNull(mediatorSettings, nameof(mediatorSettings));
            Guard.NotNull(mediatorSessionManager, nameof(mediatorSessionManager));

            settings = mediatorSettings;
            this.initiallyMaximized = initiallyMaximized;
            sessionManager = new FreshNotNullableReference<CirceMediatorSessionManager>(mediatorSessionManager);

            sessionManager.Value.PacketSending += MediatorSessionManagerOnPacketSending;
            sessionManager.Value.PacketReceived += MediatorSessionManagerOnPacketReceived;
            sessionManager.Value.ConnectionStateChanged += MediatorSessionManagerOnConnectionStateChanged;
            sessionManager.Value.StatusCodeChanged += MediatorSessionManagerOnStatusCodeChanged;
            sessionManager.Value.Mediator = this;

            InitializeComponent();
            EnsureHandleCreated();
        }

        private void MediatorSessionManagerOnPacketSending([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            this.EnsureOnMainThread(() => packetOutputPulsingLed.On = !packetOutputPulsingLed.On);
        }

        private void MediatorSessionManagerOnPacketReceived([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            this.EnsureOnMainThread(() => packetInputPulsingLed.On = !packetInputPulsingLed.On);
        }

        private void MediatorSessionManagerOnConnectionStateChanged([CanBeNull] object sender, [NotNull] MediatorConnectionStateEventArgs e)
        {
            this.EnsureOnMainThread(() =>
            {
                switch (e.State)
                {
                    case MediatorConnectionState.WaitingForComPort:
                        stateLabel.Text = "Status: Waiting for available COM port.";
                        break;
                    case MediatorConnectionState.WaitingForLogin:
                        stateLabel.Text = $"Status: Waiting for incoming Login on {e.ComPort}.";
                        break;
                    case MediatorConnectionState.LoginReceived:
                        stateLabel.Text = $"Status: Received Login on {e.ComPort}.";
                        break;
                    case MediatorConnectionState.Connected:
                        stateLabel.Text = $"Status: Connected on {e.ComPort}.";
                        break;
                    case MediatorConnectionState.Disconnected:
                        stateLabel.Text = !string.IsNullOrEmpty(e.ComPort) ? $"Status: Disconnected from {e.ComPort}." : "Status: Disconnected.";
                        break;
                }
            });
        }

        private void MediatorSessionManagerOnStatusCodeChanged([CanBeNull] object sender, [NotNull] EventArgs eventArgs)
        {
            this.EnsureOnMainThread(() =>
            {
                if (!isUpdatingControlsFromSettings)
                {
                    settings.MediatorStatus = sessionManager.Value.StatusCode;
                    UpdateControlsFromSettings();
                }
            });
        }

        private void MediatorForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            MdiChildWindow.Register(this, settings, initiallyMaximized, ref components);

            UpdateControlsFromSettings();
            UpdateSessionManagerFromSettings();
        }

        private void UpdateControlsFromSettings()
        {
            try
            {
                isUpdatingControlsFromSettings = true;

                Text = "Mediator " + settings.DeviceAddressNotNull;

                powerStatus.IsPoweredOn = settings.IsPoweredOn;
                portLabel.Text = settings.ComPortName ?? ComPortSelectionForm.AutoText;

                portGroupBox.Enabled = !settings.IsPoweredOn;
                statusVersionGroupBox.Enabled = !settings.IsPoweredOn;
                logButton.Enabled = settings.IsPoweredOn;

                statusCodeLinkLabel.Text = MediatorStatusSelectionForm.GetTextFor(settings.MediatorStatus);

                Version version = settings.ProtocolVersionOrDefault;
                versionLinkLabel.Text = $"v{version.Major}.{version.Minor}.{version.Build}";
            }
            finally
            {
                isUpdatingControlsFromSettings = false;
            }
        }

        private void UpdateSessionManagerFromSettings()
        {
            sessionManager.Value.ProtocolVersion = settings.ProtocolVersionOrDefault;
            sessionManager.Value.StatusCode = settings.MediatorStatus;
            sessionManager.Value.ComPortName = settings.ComPortName;
        }

        private void PowerStatus_StatusChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.IsPoweredOn = powerStatus.IsPoweredOn;
                UpdateControlsFromSettings();
            }

            UpdateSessionManagerFromSettings();

            if (powerStatus.IsPoweredOn)
            {
                sessionManager.Value.PowerOn();
            }
            else
            {
                sessionManager.Value.PowerOff();
            }
        }

        private void ChangePortButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var form = new ComPortSelectionForm())
            {
                form.ComPortName = settings.ComPortName;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    settings.ComPortName = form.ComPortName;
                    UpdateControlsFromSettings();
                    UpdateSessionManagerFromSettings();
                }
            }
        }

        private void StatusCodeLinkLabel_LinkClicked([CanBeNull] object sender, [NotNull] LinkLabelLinkClickedEventArgs e)
        {
            using (var form = new MediatorStatusSelectionForm())
            {
                form.StatusCode = settings.MediatorStatus;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    settings.MediatorStatus = form.StatusCode;
                    UpdateControlsFromSettings();
                    UpdateSessionManagerFromSettings();
                }
            }
        }

        private void VersionLinkLabel_LinkClicked([CanBeNull] object sender, [NotNull] LinkLabelLinkClickedEventArgs e)
        {
            using (var form = new ProtocolVersionSelectionForm())
            {
                form.Version = settings.ProtocolVersionOrDefault;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    settings.ProtocolVersion = form.Version?.ToString();
                    UpdateControlsFromSettings();
                    UpdateSessionManagerFromSettings();
                }
            }
        }

        private void LogButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var form = new LogMessageForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    byte[] data = Encoding.UTF8.GetBytes(form.Message);
                    sessionManager.Value.LogData(data);
                }
            }
        }

        private void MediatorForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
        {
            if (!forceClose && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        public void CloseWithOverride()
        {
            forceClose = true;
            Close();
        }

        void IWirelessDevice.ChangeAddress(WirelessNetworkAddress newAddress)
        {
            this.EnsureOnMainThread(() =>
            {
                settings.DeviceAddress = newAddress;
                UpdateControlsFromSettings();
            });
        }

        void IWirelessDevice.Accept(AlertOperation operation)
        {
            // Not applicable for a mediator.
        }

        void IWirelessDevice.Accept(NetworkSetupOperation operation)
        {
            // Not applicable for a mediator.
        }

        void IWirelessDevice.Accept(SynchronizeClocksOperation operation)
        {
            // Not applicable for a mediator.
        }

        void IWirelessDevice.Accept(VisualizeOperation operation)
        {
            // Not applicable for a mediator.
        }
    }
}
