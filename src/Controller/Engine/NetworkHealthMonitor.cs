using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Performs continuous monitoring of the on-line devices in the wireless network and reports status on changes.
    /// </summary>
    public sealed class NetworkHealthMonitor
    {
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);
        private static readonly object StateLock = new();

        private readonly Dictionary<WirelessNetworkAddress, DeviceStatus> devicesMap = new();
        private readonly FreshObjectReference<NetworkHealthReport> previousReport = new(NetworkHealthReport.Default);
        private readonly FreshObjectReference<NetworkComposition?> runComposition = new(null);
        private readonly FreshObjectReference<CompetitionClassRequirements?> nextRunRequirements = new(null);

        public event EventHandler<EventArgs<NetworkHealthReport>>? HealthChanged;

        public void ForceChanged()
        {
            ExclusiveUpdateWithRaiseEvent(true, previous => previous);
        }

        public void HandleConnectionStateChanged(ControllerConnectionState state)
        {
            ExclusiveUpdateWithRaiseEvent(false, previous => GetHealthReportAfterConnectionStateChanged(state, previous));
        }

        private static NetworkHealthReport GetHealthReportAfterConnectionStateChanged(ControllerConnectionState state, NetworkHealthReport previous)
        {
            previous = previous.ChangeIsConnected(state == ControllerConnectionState.Connected);

            if (previous.HasProtocolVersionMismatch)
            {
                if (state == ControllerConnectionState.Connected)
                {
                    previous = previous.ChangeHasProtocolVersionMismatch(false);
                }
            }
            else
            {
                if (state == ControllerConnectionState.ProtocolVersionMismatch)
                {
                    previous = previous.ChangeHasProtocolVersionMismatch(true);
                }
            }

            return previous;
        }

        public void HandleDeviceAdded(DeviceStatus deviceStatus)
        {
            Guard.NotNull(deviceStatus, nameof(deviceStatus));

            ExclusiveUpdateWithRaiseEvent(false, _ =>
            {
                devicesMap[deviceStatus.DeviceAddress] = deviceStatus;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void HandleDeviceChanged(DeviceStatus deviceStatus)
        {
            Guard.NotNull(deviceStatus, nameof(deviceStatus));

            ExclusiveUpdateWithRaiseEvent(false, _ =>
            {
                devicesMap[deviceStatus.DeviceAddress] = deviceStatus;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void HandleDeviceRemoved(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            ExclusiveUpdateWithRaiseEvent(false, _ =>
            {
                devicesMap.Remove(deviceAddress);
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void HandleMediatorStatusChanged(int mediatorStatus)
        {
            ExclusiveUpdateWithRaiseEvent(false, previous => previous.ChangeMediatorStatus(mediatorStatus));
        }

        private void ExclusiveUpdateWithRaiseEvent(bool forceChanged, Func<NetworkHealthReport, NetworkHealthReport> updateCallback)
        {
            EventArgs<NetworkHealthReport>? eventArgs;

            // Must lock despite of FreshReference, to prevent concurrent calls overwriting each others changes.
            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!))
            {
                lock (StateLock)
                {
                    lockTracker.Acquired();

                    NetworkHealthReport newReport = updateCallback(previousReport.Value);

                    bool hasChanged = forceChanged || newReport != previousReport.Value;

                    previousReport.Value = newReport;

                    eventArgs = hasChanged ? new EventArgs<NetworkHealthReport>(newReport) : null;
                }
            }

            if (eventArgs != null)
            {
                Log.Info($"Network health has changed - {eventArgs.Argument}");
                HealthChanged?.Invoke(this, eventArgs);
            }
        }

        public NetworkHealthReport GetLatest()
        {
            return previousReport.Value;
        }

        public void SetClassRequirements(CompetitionClassRequirements classRequirements)
        {
            Guard.NotNull(classRequirements, nameof(classRequirements));

            ExclusiveUpdateWithRaiseEvent(false, _ =>
            {
                nextRunRequirements.Value = classRequirements;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void SelectRunComposition(NetworkComposition? networkComposition)
        {
            ExclusiveUpdateWithRaiseEvent(false, _ =>
            {
                runComposition.Value = networkComposition;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        private NetworkHealthReport GetHealthReportAfterNetworkHasChanged()
        {
            NetworkComposition? runCompositionSnapshot = runComposition.Value;

            return runCompositionSnapshot != null
                ? GetHealthReportForActiveRunAfterNetworkHasChanged(runCompositionSnapshot)
                : GetHealthReportForInactiveRunAfterNetworkHasChanged();
        }

        private NetworkHealthReport GetHealthReportForInactiveRunAfterNetworkHasChanged()
        {
            List<DeviceStatus> devicesAliveAndInNetwork = GetDevicesAliveAndInNetwork();

            IEnumerable<WirelessNetworkAddress> misalignedSensors =
                from device in devicesAliveAndInNetwork where device.IsAligned == false select device.DeviceAddress;

            IEnumerable<WirelessNetworkAddress> versionMismatchingSensors =
                from device in devicesAliveAndInNetwork where device.HasVersionMismatch == true select device.DeviceAddress;

            IList<NetworkComplianceMismatch>? classCompliance = null;

            if (nextRunRequirements.Value != null)
            {
                // Create a temporary composition from all devices that are alive, then assert compliance.
                // This effectively determines if the current network is valid for starting a competition run.
                NetworkComposition tempComposition = CreateTemporaryCompositionFrom(devicesAliveAndInNetwork, nextRunRequirements.Value);

                classCompliance = tempComposition.AssertComplianceWithRequirements();
            }

            // @formatter:keep_existing_linebreaks true

            return previousReport.Value
                .ChangeMisalignedSensors(misalignedSensors)
                .ChangeVersionMismatchingSensors(versionMismatchingSensors)
                .ChangeRunComposition(null)
                .ChangeClassCompliance(classCompliance);

            // @formatter:keep_existing_linebreaks restore
        }

        private NetworkHealthReport GetHealthReportForActiveRunAfterNetworkHasChanged(NetworkComposition runCompositionSnapshot)
        {
            List<DeviceStatus> devicesAliveAndInNetwork = GetDevicesAliveAndInNetwork();

            IEnumerable<NetworkComplianceMismatch> mismatchesResultingFromOfflineDevices =
                from deviceAddress in runCompositionSnapshot.GetDeviceAddresses()
                where !devicesAliveAndInNetwork.Exists(d => d.DeviceAddress == deviceAddress)
                select NetworkComplianceMismatch.CreateForOfflineDevice(deviceAddress);

            List<DeviceStatus> devicesAliveAndInComposition =
                (from device in devicesAliveAndInNetwork where runCompositionSnapshot.ContainsDevice(device.DeviceAddress) select device).ToList();

            // We only care about misalignment for devices that are in runComposition.
            IEnumerable<WirelessNetworkAddress> misalignedSensors =
                from device in devicesAliveAndInComposition where device.IsAligned == false select device.DeviceAddress;

            // We only care about mismatching version for devices that are in runComposition.
            IEnumerable<WirelessNetworkAddress> versionMismatchingSensors =
                from device in devicesAliveAndInComposition where device.HasVersionMismatch == true select device.DeviceAddress;

            // We only care about devices requesting sync that are in runComposition.
            IEnumerable<WirelessNetworkAddress> unsyncedSensors =
                from device in devicesAliveAndInComposition
                where device.ClockSynchronization == ClockSynchronizationStatus.RequiresSync
                select device.DeviceAddress;

            // Create a temporary composition from the subset of devices that are both alive and were selected
            // for this run, then assert compliance.
            // This effectively determines if, due to off-line devices, the current network has become invalid.
            NetworkComposition tempComposition = CreateTemporaryCompositionFrom(devicesAliveAndInComposition, runCompositionSnapshot.Requirements);

            IEnumerable<NetworkComplianceMismatch> classCompliance =
                mismatchesResultingFromOfflineDevices.Concat(tempComposition.AssertComplianceWithRequirements());

            // @formatter:keep_existing_linebreaks true

            return previousReport.Value
                .ChangeMisalignedSensors(misalignedSensors)
                .ChangeUnsyncedSensors(unsyncedSensors)
                .ChangeVersionMismatchingSensors(versionMismatchingSensors)
                .ChangeRunComposition(runCompositionSnapshot)
                .ChangeClassCompliance(classCompliance);

            // @formatter:keep_existing_linebreaks restore
        }

        private List<DeviceStatus> GetDevicesAliveAndInNetwork()
        {
            IEnumerable<DeviceStatus> devices = from device in devicesMap.Values where device.IsInNetwork select device;

            return devices.ToList();
        }

        private static NetworkComposition CreateTemporaryCompositionFrom(IEnumerable<DeviceStatus> devicesAlive, CompetitionClassRequirements requirements)
        {
            var composition = NetworkComposition.Empty;
            composition = composition.ChangeRequirements(requirements);

            foreach (DeviceStatus device in devicesAlive)
            {
                composition = composition.ChangeRolesFor(device.DeviceAddress, device.Capabilities, device.Roles);
            }

            return composition;
        }
    }
}
