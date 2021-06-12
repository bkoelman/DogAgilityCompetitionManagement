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

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// An MDI child that represents an emulated wireless passage gate.
    /// </summary>
    public sealed partial class GateForm : FormWithWindowStateChangeEvent, IWirelessDevice
    {
        private readonly GateSettingsXml settings;
        private readonly bool initiallyMaximized;
        private readonly FreshObjectReference<CirceMediatorSessionManager> sessionManager;
        private readonly FreshObjectReference<DeviceStatus?> lastStatus = new(null);

        // Prevents endless recursion when updating controls that raise change events.
        private bool isUpdatingControlsFromSettings;

        bool IWirelessDevice.IsPoweredOn => powerStatus.ThreadSafeIsPoweredOn;

        WirelessNetworkAddress IWirelessDevice.Address => settings.DeviceAddressNotNull;

        public event EventHandler<EventArgs<WirelessNetworkAddress>>? DeviceRemoved;

        public GateForm(GateSettingsXml gateSettings, bool initiallyMaximized, CirceMediatorSessionManager mediatorSessionManager)
        {
            Guard.NotNull(gateSettings, nameof(gateSettings));
            Guard.NotNull(mediatorSessionManager, nameof(mediatorSessionManager));

            settings = gateSettings;
            this.initiallyMaximized = initiallyMaximized;
            sessionManager = new FreshObjectReference<CirceMediatorSessionManager>(mediatorSessionManager);
            sessionManager.Value.Devices[settings.DeviceAddressNotNull] = this;

            InitializeComponent();
            EnsureHandleCreated();
        }

        private void GateForm_Load(object? sender, EventArgs e)
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

                Text = "Gate " + settings.DeviceAddressNotNull;

                powerStatus.IsPoweredOn = settings.IsPoweredOn;
                signalButton.Enabled = settings.IsPoweredOn;
                statusUpdateTimer.Enabled = settings.IsPoweredOn;

                networkStatus.Enabled = settings.IsPoweredOn;
                networkStatus.IsInNetwork = settings.IsInNetwork;
                networkStatus.RolesAssigned = settings.RolesAssigned;

                hardwareStatus.Enabled = settings.IsPoweredOn;
                hardwareStatus.IsAligned = settings.IsAligned;
                hardwareStatus.SignalStrength = settings.SignalStrength;
                hardwareStatus.BatteryStatus = settings.BatteryStatus;
                hardwareStatus.HasVersionMismatch = settings.HasVersionMismatch;
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
                ? new DeviceStatus(settings.DeviceAddressNotNull, settings.IsInNetwork, DeviceCapabilities.TimeSensor, settings.RolesAssigned,
                    settings.SignalStrength, settings.BatteryStatus, settings.IsAligned, hardwareStatus.SynchronizationStatus,
                    settings.HasVersionMismatch.TrueOrNull())
                : null;
        }

        private void SignalButton_Click(object? sender, EventArgs e)
        {
            var deviceAction = new DeviceAction(settings.DeviceAddressNotNull, null, hardwareStatus.ClockValue);
            sessionManager.Value.NotifyAction(deviceAction);
        }

        private void PowerStatus_StatusChanged(object? sender, EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.IsPoweredOn = powerStatus.IsPoweredOn;
                UpdateControlsFromSettings();
                UpdateLastStatusFromSettings();
            }
        }

        private void NetworkStatus_StatusChanged(object? sender, EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.IsInNetwork = networkStatus.IsInNetwork;
                settings.RolesAssigned = networkStatus.RolesAssigned;
                UpdateLastStatusFromSettings();
            }
        }

        private void HardwareStatus_StatusChanged(object? sender, EventArgs e)
        {
            if (!isUpdatingControlsFromSettings)
            {
                settings.IsAligned = hardwareStatus.IsAligned == true;
                settings.SignalStrength = hardwareStatus.SignalStrength;
                settings.BatteryStatus = hardwareStatus.BatteryStatus.GetValueOrDefault(255);
                settings.HasVersionMismatch = hardwareStatus.HasVersionMismatch;
                UpdateLastStatusFromSettings();
            }
        }

        private void StatusUpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (lastStatus.Value != null)
            {
                NotifyStatus(lastStatus.Value);
            }
        }

        private void NotifyStatus(DeviceStatus status)
        {
            if (lastStatus.Value != null)
            {
                sessionManager.Value.NotifyStatus(status);
            }
        }

        private void GateForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            sessionManager.Value.Devices.TryRemove(settings.DeviceAddressNotNull, out _);

            DeviceRemoved?.Invoke(this, new EventArgs<WirelessNetworkAddress>(settings.DeviceAddressNotNull));
        }

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
                // Justification for nullable suppression: Operation has been validated for required parameters when this code is reached.
                settings.IsInNetwork = operation.SetMembership!.Value;
                settings.RolesAssigned = operation.Roles!.Value;

                UpdateControlsFromSettings();
                UpdateLastStatusFromSettings();
            });
        }

        void IWirelessDevice.Accept(SynchronizeClocksOperation operation)
        {
            this.EnsureOnMainThread(() =>
            {
                HardwareStatus_StatusChanged(this, EventArgs.Empty);
                DeviceStatus resetSyncStatus = lastStatus.Value!.ChangeClockSynchronization(null);
                NotifyStatus(resetSyncStatus);

                hardwareStatus.StartClockSynchronization();
            });
        }

        void IWirelessDevice.Accept(VisualizeOperation operation)
        {
            // Not applicable for a gate.
        }
    }
}
