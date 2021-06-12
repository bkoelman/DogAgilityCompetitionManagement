using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Mediator
{
    /// <summary>
    /// Tracks wireless devices whose status change or go on-line/off-line and logs it.
    /// </summary>
    public sealed class DeviceStatusChangeLogger
    {
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

        private readonly ConcurrentDictionary<WirelessNetworkAddress, DeviceStatus?> deviceStatusMap = new();

        public void ChangeAddress(WirelessNetworkAddress oldAddress, WirelessNetworkAddress newAddress, DeviceCapabilities newCapabilities)
        {
            Guard.NotNull(oldAddress, nameof(oldAddress));
            Guard.NotNull(newAddress, nameof(newAddress));

            if (deviceStatusMap.TryRemove(oldAddress, out DeviceStatus? deviceStatus))
            {
                deviceStatusMap[newAddress] = deviceStatus;
                Log.Info($"Device address changed from {oldAddress} to {newAddress} with capabilities: {newCapabilities}.");
            }
        }

        public void Update(DeviceStatus status)
        {
            Guard.NotNull(status, nameof(status));

            DeviceStatus? previous = deviceStatusMap.ContainsKey(status.DeviceAddress) ? deviceStatusMap[status.DeviceAddress] : null;

            if (previous == null)
            {
                Log.Info($"Device on-line: {status}");
            }
            else
            {
                string changes = FormatChanges(previous, status);

                if (!string.IsNullOrEmpty(changes))
                {
                    Log.Info($"Status update for {status.DeviceAddress}: {changes}");
                }
            }

            deviceStatusMap[status.DeviceAddress] = status;
        }

        public void Remove(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            Log.Info($"Device off-line: {deviceAddress}");
            deviceStatusMap[deviceAddress] = null;
        }

        private static string FormatChanges(DeviceStatus previous, DeviceStatus current)
        {
            var textBuilder = new StringBuilder();

            using (var formatter = new ObjectFormatter(textBuilder, null))
            {
                if (previous.IsInNetwork != current.IsInNetwork)
                {
                    formatter.AppendText(previous.IsInNetwork ? "left network" : "joined network");
                }

                formatter.AppendText(FormatFlagsEnumChanges(previous.Capabilities, current.Capabilities, nameof(current.Capabilities)));
                formatter.AppendText(FormatFlagsEnumChanges(previous.Roles, current.Roles, nameof(current.Roles)));
                formatter.AppendText(FormatSimplePropertyChange(previous.SignalStrength, current.SignalStrength, nameof(current.SignalStrength)));
                formatter.AppendText(FormatSimplePropertyChange(previous.BatteryStatus, current.BatteryStatus, nameof(current.BatteryStatus)));
                formatter.AppendText(FormatSimplePropertyChange(previous.IsAligned, current.IsAligned, nameof(current.IsAligned)));

                formatter.AppendText(FormatSimplePropertyChange(previous.ClockSynchronization, current.ClockSynchronization,
                    nameof(current.ClockSynchronization)));

                formatter.AppendText(FormatSimplePropertyChange(previous.HasVersionMismatch, current.HasVersionMismatch, nameof(current.HasVersionMismatch)));
            }

            return textBuilder.ToString();
        }

        private static string? FormatFlagsEnumChanges<TEnum>(TEnum previousEnum, TEnum currentEnum, [InvokerParameterName] string name)
            where TEnum : struct
        {
            if (!EqualityComparer<TEnum>.Default.Equals(previousEnum, currentEnum))
            {
                var previous = (Enum)(object)previousEnum;
                var current = (Enum)(object)currentEnum;

                var textBuilder = new StringBuilder();

                using (var formatter = new ObjectFormatter(textBuilder, name))
                {
                    foreach (Enum value in Enum.GetValues(typeof(TEnum)))
                    {
                        bool inCurrent = current.HasFlag(value);
                        bool inPrevious = previous.HasFlag(value);

                        if (inCurrent != inPrevious)
                        {
                            string symbol = !inPrevious ? "+" : "-";
                            formatter.AppendText(symbol + Enum.GetName(typeof(TEnum), value));
                        }
                    }
                }

                return textBuilder.ToString();
            }

            return null;
        }

        private static string? FormatSimplePropertyChange<T>(T? previous, T? current, [InvokerParameterName] string name)
        {
            return !EqualityComparer<T>.Default.Equals(previous, current) ? $"{name} {ValueOrNullText(previous)} -> {ValueOrNullText(current)}" : null;
        }

        private static string ValueOrNullText(object? value)
        {
            return value?.ToString() ?? "(null)";
        }
    }
}
