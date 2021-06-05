using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// Keeps track of the hardware devices in the wireless network. It monitors periodic incoming Circe status messages from the mediator and provides
    /// events for addition, removal and changes of devices in the network.
    /// </summary>
    public sealed class DeviceTracker : IDisposable
    {
#if DEBUGGING_HACKS
        private const int DeviceLifetimeExpiredInMilliseconds = 10000; // Wait a bit longer for easier debugging
#else
        private const int DeviceLifetimeExpiredInMilliseconds = 3000;
#endif

        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

        private readonly Dictionary<WirelessNetworkAddress, DeviceMapEntry> deviceMap = new();
        private readonly object stateLock = new();

        private int lastMediatorStatus; // Protected by stateLock

        public event EventHandler<EventArgs<DeviceStatus>>? DeviceAdded;
        public event EventHandler<EventArgs<DeviceStatus>>? DeviceChanged;
        public event EventHandler<EventArgs<WirelessNetworkAddress>>? DeviceRemoved;
        public event EventHandler<EventArgs<int>>? MediatorStatusChanged;

        public void UpdateMediatorStatus(int mediatorStatus)
        {
            using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

            lock (stateLock)
            {
                lockTracker.Acquired();

                if (mediatorStatus != lastMediatorStatus)
                {
                    Log.Debug($"Mediator status changed from {lastMediatorStatus} to {mediatorStatus}.");

                    lastMediatorStatus = mediatorStatus;
                    MediatorStatusChanged?.Invoke(this, new EventArgs<int>(mediatorStatus));
                }
            }
        }

        public void SetDeviceStatus(DeviceStatus status)
        {
            Guard.NotNull(status, nameof(status));

            using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

            lock (stateLock)
            {
                lockTracker.Acquired();

                if (deviceMap.ContainsKey(status.DeviceAddress))
                {
                    DeviceMapEntry existingEntry = deviceMap[status.DeviceAddress];

                    bool hasChanges = existingEntry.ApplyChanges(status);
                    existingEntry.Extend();

                    if (hasChanges)
                    {
                        Log.Debug($"Device {status.DeviceAddress} changed.");
                        DeviceChanged?.Invoke(this, new EventArgs<DeviceStatus>(status));
                    }
                }
                else
                {
                    var newEntry = new DeviceMapEntry(status, RemoveTimerTick);
                    deviceMap[status.DeviceAddress] = newEntry;
                    newEntry.Extend();

                    Log.Debug($"Device {status.DeviceAddress} added.");
                    DeviceAdded?.Invoke(this, new EventArgs<DeviceStatus>(status));
                }
            }
        }

        public void NotifyDeviceIsAlive(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

            lock (stateLock)
            {
                lockTracker.Acquired();

                if (deviceMap.ContainsKey(deviceAddress))
                {
                    DeviceMapEntry existingEntry = deviceMap[deviceAddress];
                    existingEntry.Extend();
                }
            }
        }

        public void Dispose()
        {
            using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

            lock (stateLock)
            {
                lockTracker.Acquired();

                while (deviceMap.Count > 0)
                {
                    KeyValuePair<WirelessNetworkAddress, DeviceMapEntry> firstPair = deviceMap.First();
                    deviceMap.Remove(firstPair.Key);
                    firstPair.Value.Dispose();
                }
            }
        }

        private void RemoveTimerTick(object? state)
        {
            try
            {
                using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

                lock (stateLock)
                {
                    lockTracker.Acquired();

                    // Justification for nullable suppression: 'state' parameter is optional in TimerCallback delegate, but we always pass a value.
                    var entry = (DeviceMapEntry)state!;

                    WirelessNetworkAddress address = entry.LastStatus.DeviceAddress;

                    if (deviceMap.Remove(address))
                    {
                        Log.Debug($"Device {address} removed.");
                        DeviceRemoved?.Invoke(this, new EventArgs<WirelessNetworkAddress>(address));
                    }

                    entry.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error while processing removal of wireless device.", ex);
            }
        }

        private sealed class DeviceMapEntry : IDisposable
        {
            private readonly Timer expiryTimer;

            public DeviceStatus LastStatus { get; private set; }

            public DeviceMapEntry(DeviceStatus status, TimerCallback entryExpiredCallback)
            {
                LastStatus = status;
                expiryTimer = new Timer(entryExpiredCallback, this, Timeout.Infinite, Timeout.Infinite);
            }

            public bool ApplyChanges(DeviceStatus newStatus)
            {
                if (LastStatus == newStatus)
                {
                    return false;
                }

                LastStatus = newStatus;
                return true;
            }

            public void Extend()
            {
                expiryTimer.Change(DeviceLifetimeExpiredInMilliseconds, Timeout.Infinite);
            }

            public void Dispose()
            {
                expiryTimer.Dispose();
            }
        }
    }
}
