using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Performs continuous monitoring of the on-line devices in the wireless network and reports status on changes.
    /// </summary>
    public sealed class NetworkHealthMonitor
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private static readonly object StateLock = new();

        [NotNull]
        private readonly Dictionary<WirelessNetworkAddress, DeviceStatus> devicesMap = new();

        [NotNull]
        private readonly FreshNotNullableReference<NetworkHealthReport> previousReport = new(NetworkHealthReport.Default);

        [NotNull]
        private readonly FreshReference<NetworkComposition> runComposition = new(null);

        [NotNull]
        private readonly FreshReference<CompetitionClassRequirements> nextRunRequirements = new(null);

        public event EventHandler<EventArgs<NetworkHealthReport>> HealthChanged;

        public void ForceChanged()
        {
            ExclusiveUpdateWithRaiseEvent(true, previous => previous);
        }

        public void HandleConnectionStateChanged(ControllerConnectionState state)
        {
            ExclusiveUpdateWithRaiseEvent(false, previous => GetHealthReportAfterConnectionStateChanged(state, previous));
        }

        [NotNull]
        private static NetworkHealthReport GetHealthReportAfterConnectionStateChanged(ControllerConnectionState state, [NotNull] NetworkHealthReport previous)
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

        public void HandleDeviceAdded([NotNull] DeviceStatus deviceStatus)
        {
            Guard.NotNull(deviceStatus, nameof(deviceStatus));

            ExclusiveUpdateWithRaiseEvent(false, previous =>
            {
                devicesMap[deviceStatus.DeviceAddress] = deviceStatus;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void HandleDeviceChanged([NotNull] DeviceStatus deviceStatus)
        {
            Guard.NotNull(deviceStatus, nameof(deviceStatus));

            ExclusiveUpdateWithRaiseEvent(false, previous =>
            {
                devicesMap[deviceStatus.DeviceAddress] = deviceStatus;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void HandleDeviceRemoved([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            ExclusiveUpdateWithRaiseEvent(false, previous =>
            {
                devicesMap.Remove(deviceAddress);
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void HandleMediatorStatusChanged(int mediatorStatus)
        {
            ExclusiveUpdateWithRaiseEvent(false, previous => previous.ChangeMediatorStatus(mediatorStatus));
        }

        private void ExclusiveUpdateWithRaiseEvent(bool forceChanged, [NotNull] Func<NetworkHealthReport, NetworkHealthReport> updateCallback)
        {
            EventArgs<NetworkHealthReport> eventArgs;

            // Must lock despite of FreshReference, to prevent concurrent calls overwriting each others changes.
            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
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

        [NotNull]
        public NetworkHealthReport GetLatest()
        {
            return previousReport.Value;
        }

        public void SetClassRequirements([NotNull] CompetitionClassRequirements classRequirements)
        {
            Guard.NotNull(classRequirements, nameof(classRequirements));

            ExclusiveUpdateWithRaiseEvent(false, previous =>
            {
                nextRunRequirements.Value = classRequirements;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        public void SelectRunComposition([CanBeNull] NetworkComposition networkComposition)
        {
            ExclusiveUpdateWithRaiseEvent(false, previous =>
            {
                runComposition.Value = networkComposition;
                return GetHealthReportAfterNetworkHasChanged();
            });
        }

        [NotNull]
        private NetworkHealthReport GetHealthReportAfterNetworkHasChanged()
        {
            NetworkComposition runCompositionSnapshot = runComposition.Value;

            return runCompositionSnapshot != null
                ? GetHealthReportForActiveRunAfterNetworkHasChanged(runCompositionSnapshot)
                : GetHealthReportForInactiveRunAfterNetworkHasChanged();
        }

        [NotNull]
        private NetworkHealthReport GetHealthReportForInactiveRunAfterNetworkHasChanged()
        {
            List<DeviceStatus> devicesAliveAndInNetwork = GetDevicesAliveAndInNetwork();

            IEnumerable<WirelessNetworkAddress> misalignedSensors =
                from device in devicesAliveAndInNetwork where device.IsAligned == false select device.DeviceAddress;

            IEnumerable<WirelessNetworkAddress> versionMismatchingSensors =
                from device in devicesAliveAndInNetwork where device.HasVersionMismatch == true select device.DeviceAddress;

            IList<NetworkComplianceMismatch> classCompliance = null;

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

        [NotNull]
        private NetworkHealthReport GetHealthReportForActiveRunAfterNetworkHasChanged([NotNull] NetworkComposition runCompositionSnapshot)
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

        [NotNull]
        [ItemNotNull]
        private List<DeviceStatus> GetDevicesAliveAndInNetwork()
        {
            IEnumerable<DeviceStatus> devices = from device in devicesMap.Values where device.IsInNetwork select device;

            return devices.ToList();
        }

        [NotNull]
        private static NetworkComposition CreateTemporaryCompositionFrom([NotNull] [ItemNotNull] IEnumerable<DeviceStatus> devicesAlive,
            [NotNull] CompetitionClassRequirements requirements)
        {
            var composition = NetworkComposition.Empty;
            composition = composition.ChangeRequirements(requirements);

            // ReSharper disable once LoopCanBeConvertedToQuery
            // Reason: Procedural algorithm is more readable and easier to understand here.
            foreach (DeviceStatus device in devicesAlive)
            {
                composition = composition.ChangeRolesFor(device.DeviceAddress, device.Capabilities, device.Roles);
            }

            return composition;
        }
    }
}
