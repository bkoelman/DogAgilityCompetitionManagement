using System;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Mediator;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.MediatorEmulator.Engine;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// An MDI child that represents an emulated wireless remote control.
    /// </summary>
    public sealed partial class RemoteForm : FormWithWindowStateChangeEvent, IWirelessDevice
    {
        [NotNull]
        private readonly RemoteSettingsXml settings;

        private readonly bool initiallyMaximized;

        [NotNull]
        private readonly FreshNotNullableReference<CirceMediatorSessionManager> sessionManager;

        [NotNull]
        private readonly FreshReference<DeviceStatus> lastStatus = new FreshReference<DeviceStatus>(null);

        // Prevents endless recursion when updating controls that raise change events.
        private bool isUpdatingControlsFromSettings;

        public event EventHandler<EventArgs<WirelessNetworkAddress>> DeviceRemoved;

        public RemoteForm([NotNull] RemoteSettingsXml remoteSettings, bool initiallyMaximized,
            [NotNull] CirceMediatorSessionManager mediatorSessionManager)
        {
            Guard.NotNull(remoteSettings, nameof(remoteSettings));
            Guard.NotNull(mediatorSessionManager, nameof(mediatorSessionManager));

            settings = remoteSettings;
            this.initiallyMaximized = initiallyMaximized;
            sessionManager = new FreshNotNullableReference<CirceMediatorSessionManager>(mediatorSessionManager);
            sessionManager.Value.Devices[settings.DeviceAddressNotNull] = this;

            InitializeComponent();
            EnsureHandleCreated();

            keypad.KeysDownChanged += KeypadOnKeysDownChanged;
        }

        private void RemoteForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            MdiChildWindow.Register(this, settings, initiallyMaximized, ref components);

            UpdateControlsFromSettings();
            UpdateLastStatusFromSettings();
        }

        private void UpdateControlsFromSettings()
        {
            try
            {
                isUpdatingControlsFromSettings = true;

                Text = @"Remote " + settings.DeviceAddressNotNull;

                powerStatus.IsPoweredOn = settings.IsPoweredOn;
                statusUpdateTimer.Enabled = settings.IsPoweredOn;

                networkStatus.Enabled = settings.IsPoweredOn;
                networkStatus.IsInNetwork = settings.IsInNetwork;
                networkStatus.RolesAssigned = settings.RolesAssigned;

                hardwareStatus.Enabled = settings.IsPoweredOn;
                hardwareStatus.SignalStrength = settings.SignalStrength;
                hardwareStatus.BatteryStatus = settings.BatteryStatus;
                hardwareStatus.HasVersionMismatch = settings.HasVersionMismatch;

                keypad.Enabled = settings.IsPoweredOn;
                keypad.Features = settings.Features;
            }
            finally
            {
                isUpdatingControlsFromSettings = false;
            }
        }

        private void UpdateLastStatusFromSettings()
        {
            if (lastStatus.Value != null && !settings.IsPoweredOn)
            {
                sessionManager.Value.NotifyOffline(settings.DeviceAddressNotNull);
            }

            lastStatus.Value = settings.IsPoweredOn
                ? new DeviceStatus(settings.DeviceAddressNotNull, settings.IsInNetwork,
                    settings.Features.ToCapabilities(), settings.RolesAssigned, settings.SignalStrength,
                    settings.BatteryStatus, null, hardwareStatus.SynchronizationStatus,
                    settings.HasVersionMismatch.TrueOrNull())
                : null;
        }

        private void PowerStatus_StatusChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.IsPoweredOn = powerStatus.IsPoweredOn;
                UpdateControlsFromSettings();
                UpdateLastStatusFromSettings();
            }
        }

        private void NetworkStatus_StatusChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.IsInNetwork = networkStatus.IsInNetwork;
                settings.RolesAssigned = networkStatus.RolesAssigned;
                UpdateLastStatusFromSettings();
            }
        }

        private void HardwareStatus_StatusChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.SignalStrength = hardwareStatus.SignalStrength;
                settings.BatteryStatus = hardwareStatus.BatteryStatus.GetValueOrDefault(255);
                settings.HasVersionMismatch = hardwareStatus.HasVersionMismatch;
                UpdateLastStatusFromSettings();
            }
        }

        private void Keypad_StatusChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.Features = keypad.Features;

                hardwareStatus.SupportsClock = (keypad.Features & RemoteEmulatorFeatures.TimerKeys) != 0;

                UpdateLastStatusFromSettings();
            }
        }

        private void KeypadOnKeysDownChanged([CanBeNull] object sender, [NotNull] EventArgs<RawDeviceKeys> e)
        {
            TimeSpan? clockValueOrNull = hardwareStatus.SupportsClock ? hardwareStatus.ClockValue : (TimeSpan?) null;
            var deviceAction = new DeviceAction(settings.DeviceAddressNotNull, e.Argument, clockValueOrNull);
            sessionManager.Value.NotifyAction(deviceAction);
        }

        private void StatusUpdateTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (lastStatus.Value != null)
            {
                sessionManager.Value.NotifyStatus(lastStatus.Value);
            }
        }

        private void RemoteForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
        {
            IWirelessDevice unused;
            sessionManager.Value.Devices.TryRemove(settings.DeviceAddressNotNull, out unused);

            DeviceRemoved?.Invoke(this, new EventArgs<WirelessNetworkAddress>(settings.DeviceAddressNotNull));
        }

        bool IWirelessDevice.IsPoweredOn => powerStatus.ThreadSafeIsPoweredOn;

        WirelessNetworkAddress IWirelessDevice.Address => settings.DeviceAddressNotNull;

        void IWirelessDevice.ChangeAddress(WirelessNetworkAddress newAddress)
        {
            this.EnsureOnMainThread(() =>
            {
                settings.DeviceAddress = newAddress;
                UpdateControlsFromSettings();
                UpdateLastStatusFromSettings();
            });
        }

        void IWirelessDevice.Accept(AlertOperation operation)
        {
            this.EnsureOnMainThread(() => powerStatus.BlinkAsync());
        }

        void IWirelessDevice.Accept(NetworkSetupOperation operation)
        {
            this.EnsureOnMainThread(() =>
            {
                // ReSharper disable PossibleInvalidOperationException
                // Reason: Operation has been validated for required parameters when this code is reached.
                settings.IsInNetwork = operation.SetMembership.Value;
                settings.RolesAssigned = operation.Roles.Value;
                // ReSharper restore PossibleInvalidOperationException

                UpdateControlsFromSettings();
                UpdateLastStatusFromSettings();
            });
        }

        void IWirelessDevice.Accept(SynchronizeClocksOperation operation)
        {
            this.EnsureOnMainThread(() => hardwareStatus.StartClockSynchronization());
        }

        void IWirelessDevice.Accept(VisualizeOperation operation)
        {
            // Not applicable for a remote.
        }
    }
}