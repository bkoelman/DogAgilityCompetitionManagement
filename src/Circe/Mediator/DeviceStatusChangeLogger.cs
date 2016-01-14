using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private readonly ConcurrentDictionary<WirelessNetworkAddress, DeviceStatus> deviceStatusMap =
            new ConcurrentDictionary<WirelessNetworkAddress, DeviceStatus>();

        public void ChangeAddress([NotNull] WirelessNetworkAddress oldAddress,
            [NotNull] WirelessNetworkAddress newAddress, DeviceCapabilities newCapabilities)
        {
            Guard.NotNull(oldAddress, nameof(oldAddress));
            Guard.NotNull(newAddress, nameof(newAddress));

            DeviceStatus deviceStatus;
            if (deviceStatusMap.TryRemove(oldAddress, out deviceStatus))
            {
                deviceStatusMap[newAddress] = deviceStatus;
                Log.Info(
                    $"Device address changed from {oldAddress} to {newAddress} with capabilities: {newCapabilities}.");
            }
        }

        public void Update([NotNull] DeviceStatus status)
        {
            Guard.NotNull(status, nameof(status));

            DeviceStatus previous = deviceStatusMap.ContainsKey(status.DeviceAddress)
                ? deviceStatusMap[status.DeviceAddress]
                : null;
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

        public void Remove([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            Log.Info($"Device off-line: {deviceAddress}");
            deviceStatusMap[deviceAddress] = null;
        }

        [NotNull]
        private static string FormatChanges([NotNull] DeviceStatus previous, [NotNull] DeviceStatus current)
        {
            var textBuilder = new StringBuilder();
            using (var formatter = new ObjectFormatter(textBuilder, null))
            {
                if (previous.IsInNetwork != current.IsInNetwork)
                {
                    formatter.AppendText(previous.IsInNetwork ? "left network" : "joined network");
                }

                formatter.AppendText(FormatFlagsEnumChanges(previous.Capabilities, current.Capabilities,
                    () => current.Capabilities));
                formatter.AppendText(FormatFlagsEnumChanges(previous.Roles, current.Roles, () => current.Roles));

                formatter.AppendText(FormatSimplePropertyChange(previous.SignalStrength, current.SignalStrength,
                    () => current.SignalStrength));
                formatter.AppendText(FormatSimplePropertyChange(previous.BatteryStatus, current.BatteryStatus,
                    () => current.BatteryStatus));
                formatter.AppendText(FormatSimplePropertyChange(previous.IsAligned, current.IsAligned,
                    () => current.IsAligned));
                formatter.AppendText(FormatSimplePropertyChange(previous.ClockSynchronization,
                    current.ClockSynchronization, () => current.ClockSynchronization));
                formatter.AppendText(FormatSimplePropertyChange(previous.HasVersionMismatch, current.HasVersionMismatch,
                    () => current.HasVersionMismatch));
            }

            return textBuilder.ToString();
        }

        [CanBeNull]
        private static string FormatFlagsEnumChanges<TEnum>(TEnum previousEnum, TEnum currentEnum,
            [NotNull] Expression<Func<object>> getNameExpression) where TEnum : struct
        {
            if (!EqualityComparer<TEnum>.Default.Equals(previousEnum, currentEnum))
            {
                var previous = (Enum) (object) previousEnum;
                var current = (Enum) (object) currentEnum;

                string name = getNameExpression.GetExpressionName();
                var textBuilder = new StringBuilder();
                using (var formatter = new ObjectFormatter(textBuilder, name))
                {
                    foreach (Enum value in Enum.GetValues(typeof (TEnum)))
                    {
                        bool inCurrent = current.HasFlag(value);
                        bool inPrevious = previous.HasFlag(value);

                        if (inCurrent != inPrevious)
                        {
                            string symbol = !inPrevious ? "+" : "-";
                            formatter.AppendText(symbol + Enum.GetName(typeof (TEnum), value));
                        }
                    }
                }
                return textBuilder.ToString();
            }
            return null;
        }

        [CanBeNull]
        private static string FormatSimplePropertyChange<T>([CanBeNull] T previous, [CanBeNull] T current,
            [NotNull] Expression<Func<object>> getNameExpression)
        {
            string name = getNameExpression.GetExpressionName();
            return !EqualityComparer<T>.Default.Equals(previous, current)
                ? $"{name} {ValueOrNullText(previous)} -> {ValueOrNullText(current)}"
                : null;
        }

        [NotNull]
        private static string ValueOrNullText([CanBeNull] object value)
        {
            return value?.ToString() ?? "(null)";
        }
    }
}