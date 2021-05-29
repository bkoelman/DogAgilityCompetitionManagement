﻿using System;
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
    /// An MDI child that represents an emulated wireless passage gate.
    /// </summary>
    public sealed partial class GateForm : FormWithWindowStateChangeEvent, IWirelessDevice
    {
        [NotNull]
        private readonly GateSettingsXml settings;

        private readonly bool initiallyMaximized;

        [NotNull]
        private readonly FreshNotNullableReference<CirceMediatorSessionManager> sessionManager;

        [NotNull]
        private readonly FreshReference<DeviceStatus> lastStatus = new(null);

        // Prevents endless recursion when updating controls that raise change events.
        private bool isUpdatingControlsFromSettings;

        bool IWirelessDevice.IsPoweredOn => powerStatus.ThreadSafeIsPoweredOn;

        WirelessNetworkAddress IWirelessDevice.Address => settings.DeviceAddressNotNull;

        public event EventHandler<EventArgs<WirelessNetworkAddress>> DeviceRemoved;

        public GateForm([NotNull] GateSettingsXml gateSettings, bool initiallyMaximized, [NotNull] CirceMediatorSessionManager mediatorSessionManager)
        {
            Guard.NotNull(gateSettings, nameof(gateSettings));
            Guard.NotNull(mediatorSessionManager, nameof(mediatorSessionManager));

            settings = gateSettings;
            this.initiallyMaximized = initiallyMaximized;
            sessionManager = new FreshNotNullableReference<CirceMediatorSessionManager>(mediatorSessionManager);
            sessionManager.Value.Devices[settings.DeviceAddressNotNull] = this;

            InitializeComponent();
            EnsureHandleCreated();
        }

        private void GateForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
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

        private void SignalButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            var deviceAction = new DeviceAction(settings.DeviceAddressNotNull, null, hardwareStatus.ClockValue);
            sessionManager.Value.NotifyAction(deviceAction);
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
                settings.IsAligned = hardwareStatus.IsAligned == true;
                settings.SignalStrength = hardwareStatus.SignalStrength;
                settings.BatteryStatus = hardwareStatus.BatteryStatus.GetValueOrDefault(255);
                settings.HasVersionMismatch = hardwareStatus.HasVersionMismatch;
                UpdateLastStatusFromSettings();
            }
        }

        private void StatusUpdateTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (lastStatus.Value != null)
            {
                sessionManager.Value.NotifyStatus(lastStatus.Value);
            }
        }

        private void GateForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
        {
            IWirelessDevice unused;
            sessionManager.Value.Devices.TryRemove(settings.DeviceAddressNotNull, out unused);

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
            // Not applicable for a gate.
        }
    }
}
