using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Controller.UI.Controls;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Displays on-line wireless devices and enables composition of logical network.
    /// </summary>
    public sealed partial class NetworkSetupForm : FormWithHandleManagement
    {
        [NotNull]
        private readonly Form owner;

        [CanBeNull]
        private CirceControllerSessionManager sessionManager;

        [CanBeNull]
        public CirceControllerSessionManager SessionManager
        {
            get
            {
                return sessionManager;
            }
            set
            {
                if (value != sessionManager)
                {
                    DetachSessionHandlers();
                    sessionManager = value;
                    AttachSessionHandlers();
                }
            }
        }

        public NetworkSetupForm([NotNull] Form owner)
        {
            Guard.NotNull(owner, nameof(owner));
            this.owner = owner;

            InitializeComponent();

            devicesGrid.AlertRequested += DevicesGridOnAlertRequested;
            devicesGrid.NetworkSetupRequested += DevicesGridOnNetworkSetupRequested;
        }

        private void DevicesGridOnAlertRequested([CanBeNull] object sender, [NotNull] AlertEventArgs e)
        {
            if (sessionManager != null)
            {
                e.Task = sessionManager.AlertAsync(e.DestinationAddress, e.CancelToken);
            }
        }

        private void DevicesGridOnNetworkSetupRequested([CanBeNull] object sender, [NotNull] NetworkSetupEventArgs e)
        {
            if (sessionManager != null)
            {
                e.Task = sessionManager.NetworkSetupAsync(e.DestinationAddress, e.JoinNetwork, e.Roles, e.CancelToken);
            }
        }

        private void AttachSessionHandlers()
        {
            if (sessionManager != null)
            {
                sessionManager.ConnectionStateChanged += SessionManagerOnConnectionStateChanged;
                sessionManager.DeviceTracker.DeviceAdded += DeviceTrackerOnDeviceAdded;
                sessionManager.DeviceTracker.DeviceChanged += DeviceTrackerOnDeviceChanged;
                sessionManager.DeviceTracker.DeviceRemoved += DeviceTrackerOnDeviceRemoved;
            }
        }

        private void DetachSessionHandlers()
        {
            if (sessionManager != null)
            {
                sessionManager.DeviceTracker.DeviceRemoved -= DeviceTrackerOnDeviceRemoved;
                sessionManager.DeviceTracker.DeviceChanged -= DeviceTrackerOnDeviceAdded;
                sessionManager.DeviceTracker.DeviceAdded -= DeviceTrackerOnDeviceChanged;
                sessionManager.ConnectionStateChanged -= SessionManagerOnConnectionStateChanged;
            }
        }

        private void SessionManagerOnConnectionStateChanged([CanBeNull] object sender,
            [NotNull] ControllerConnectionStateEventArgs e)
        {
            // Chicken and egg problem: We need to marshal from a background thread to the UI thread,
            // but may need to create a Window handle before marshaling is possible. But we can only create
            // a window handle when running on the UI thread. 
            // To overcome this, we use MainForm for marshaling to the UI thread, then create our Window handle.

            owner.EnsureOnMainThread(() =>
            {
                EnsureHandleCreated();
                devicesGrid.IsConnected = e.State == ControllerConnectionState.Connected;
            });
        }

        private void DeviceTrackerOnDeviceAdded([CanBeNull] object sender, [NotNull] EventArgs<DeviceStatus> e)
        {
            if (Visible)
            {
                SystemSound.AsyncPlayDeviceConnect();
            }

            HandleDeviceAddedOrChanged(e);
        }

        private void DeviceTrackerOnDeviceChanged([CanBeNull] object sender, [NotNull] EventArgs<DeviceStatus> e)
        {
            HandleDeviceAddedOrChanged(e);
        }

        private void HandleDeviceAddedOrChanged([NotNull] EventArgs<DeviceStatus> e)
        {
            owner.EnsureOnMainThread(() =>
            {
                EnsureHandleCreated();
                devicesGrid.AddOrUpdate(e.Argument);
            });
        }

        private void DeviceTrackerOnDeviceRemoved([CanBeNull] object sender,
            [NotNull] EventArgs<WirelessNetworkAddress> e)
        {
            if (Visible)
            {
                SystemSound.AsyncPlayDeviceDisconnect();
            }

            owner.EnsureOnMainThread(() =>
            {
                EnsureHandleCreated();
                devicesGrid.Remove(e.Argument);
            });
        }
    }
}