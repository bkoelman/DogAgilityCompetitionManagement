﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// In the process of input handling, filters press/release of modifier and numeric keys, to build up a competitor number
    /// and raise related events.
    /// </summary>
    /// <remarks>
    /// Supports multiple remote controls.
    /// </remarks>
    public sealed class NumberEntryFilter
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const int MaxCompetitorNumberLength = 3;

        [NotNull]
        private readonly NumberEntryState numberEntryState = new NumberEntryState();

        [NotNull]
        private readonly DeviceModifiersState modifierStatePerDevice = new DeviceModifiersState();

        public event EventHandler<CompetitorSelectionEventArgs> NotifyCompetitorSelecting;
        public event EventHandler<CompetitorNumberSelectionEventArgs> NotifyDigitReceived;
        public event EventHandler<CompetitorSelectionEventArgs> NotifyCompetitorSelectCanceled;
        public event EventHandler<CompetitorNumberSelectionEventArgs> NotifyCompetitorSelected;
        public event EventHandler<UnknownDeviceActionEventArgs> NotifyUnknownAction;

        public void HandleModifierKeyDown([NotNull] WirelessNetworkAddress source, RemoteKeyModifier modifier)
        {
            Guard.NotNull(source, nameof(source));

            modifierStatePerDevice.EnsureDown(source, modifier);

            if (numberEntryState.Mode == NumberEntryMode.None)
            {
                NumberEntryMode newEntryMode = GetMatchingEntryModeForModifier(modifier);
                numberEntryState.ChangeMode(newEntryMode);

                bool isCurrentCompetitor = modifier == RemoteKeyModifier.EnterCurrentCompetitor;
                var args = new CompetitorSelectionEventArgs(isCurrentCompetitor);
                NotifyCompetitorSelecting?.Invoke(this, args);
            }
        }

        private static NumberEntryMode GetMatchingEntryModeForModifier(RemoteKeyModifier modifier)
        {
            switch (modifier)
            {
                case RemoteKeyModifier.EnterNextCompetitor:
                    return NumberEntryMode.EnteringNextCompetitor;
                case RemoteKeyModifier.EnterCurrentCompetitor:
                    return NumberEntryMode.EnteringCurrentCompetitor;
                default:
                    throw ExceptionFactory.CreateNotSupportedExceptionFor(modifier);
            }
        }

        public void HandleKeyDown([NotNull] WirelessNetworkAddress source, RemoteKey key,
            [CanBeNull] TimeSpan? sensorTime)
        {
            Guard.NotNull(source, nameof(source));

            KeyCategory keyCategory = GetCategoryForKey(key);

            if (keyCategory != KeyCategory.CommandOnly && HasModifierDownThatMatchesEntryState(source))
            {
                int digit = GetDigitForKey(key);
                if (numberEntryState.AppendToNumber(digit))
                {
                    // ReSharper disable once PossibleInvalidOperationException
                    // Reason: Call to AppendToNumber() above guarantees that value is not null.
                    int number = numberEntryState.Number.Value;

                    if (number > 0)
                    {
                        bool isCurrentCompetitor = numberEntryState.Mode == NumberEntryMode.EnteringCurrentCompetitor;
                        var args = new CompetitorNumberSelectionEventArgs(number, isCurrentCompetitor);
                        NotifyDigitReceived?.Invoke(this, args);
                    }
                }
            }

            if (keyCategory == KeyCategory.CommandOnly ||
                (keyCategory == KeyCategory.DigitAndCommand && modifierStatePerDevice.IsEmpty(source)))
            {
                NotifyUnknownAction?.Invoke(this, new UnknownDeviceActionEventArgs(source, sensorTime, key));
            }
        }

        private bool HasModifierDownThatMatchesEntryState([NotNull] WirelessNetworkAddress deviceAddress)
        {
            if (modifierStatePerDevice.IsDown(deviceAddress, RemoteKeyModifier.EnterNextCompetitor) &&
                numberEntryState.Mode == NumberEntryMode.EnteringNextCompetitor)
            {
                return true;
            }
            if (modifierStatePerDevice.IsDown(deviceAddress, RemoteKeyModifier.EnterCurrentCompetitor) &&
                numberEntryState.Mode == NumberEntryMode.EnteringCurrentCompetitor)
            {
                return true;
            }

            return false;
        }

        private static KeyCategory GetCategoryForKey(RemoteKey key)
        {
            switch (key)
            {
                case RemoteKey.Key4:
                case RemoteKey.Key7:
                    return KeyCategory.DigitOnly;
                case RemoteKey.Key1OrPlaySoundA:
                case RemoteKey.Key2OrPassIntermediate:
                case RemoteKey.Key3OrToggleElimination:
                case RemoteKey.Key5OrDecreaseRefusals:
                case RemoteKey.Key6OrIncreaseRefusals:
                case RemoteKey.Key8OrDecreaseFaults:
                case RemoteKey.Key9OrIncreaseFaults:
                case RemoteKey.Key0OrMuteSound:
                    return KeyCategory.DigitAndCommand;
                case RemoteKey.PassFinish:
                case RemoteKey.PassStart:
                case RemoteKey.ResetRun:
                case RemoteKey.Ready:
                    return KeyCategory.CommandOnly;
                default:
                    throw ExceptionFactory.CreateNotSupportedExceptionFor(key);
            }
        }

        private static int GetDigitForKey(RemoteKey key)
        {
            switch (key)
            {
                case RemoteKey.Key1OrPlaySoundA:
                    return 1;
                case RemoteKey.Key2OrPassIntermediate:
                    return 2;
                case RemoteKey.Key3OrToggleElimination:
                    return 3;
                case RemoteKey.Key4:
                    return 4;
                case RemoteKey.Key5OrDecreaseRefusals:
                    return 5;
                case RemoteKey.Key6OrIncreaseRefusals:
                    return 6;
                case RemoteKey.Key7:
                    return 7;
                case RemoteKey.Key8OrDecreaseFaults:
                    return 8;
                case RemoteKey.Key9OrIncreaseFaults:
                    return 9;
                case RemoteKey.Key0OrMuteSound:
                    return 0;
                default:
                    throw new InvalidOperationException($"Key {key} has no numeric value.");
            }
        }

        public void HandleModifierKeyUp([NotNull] WirelessNetworkAddress source, RemoteKeyModifier modifier)
        {
            Guard.NotNull(source, nameof(source));

            modifierStatePerDevice.EnsureNotDown(source, modifier);

            NumberEntryMode matchingEntryMode = GetMatchingEntryModeForModifier(modifier);

            if (numberEntryState.Mode == matchingEntryMode)
            {
                int? builtNumber = numberEntryState.Number;

                numberEntryState.ChangeMode(NumberEntryMode.None);

                bool isCurrentCompetitor = modifier == RemoteKeyModifier.EnterCurrentCompetitor;
                if (builtNumber == null || builtNumber == 0)
                {
                    var args = new CompetitorSelectionEventArgs(isCurrentCompetitor);
                    NotifyCompetitorSelectCanceled?.Invoke(this, args);
                }
                else
                {
                    var args = new CompetitorNumberSelectionEventArgs(builtNumber.Value, isCurrentCompetitor);
                    NotifyCompetitorSelected?.Invoke(this, args);
                }
            }
        }

        public void HandleMissingKey([NotNull] WirelessNetworkAddress source, [CanBeNull] TimeSpan? sensorTime)
        {
            Guard.NotNull(source, nameof(source));

            var args = new UnknownDeviceActionEventArgs(source, sensorTime, null);
            NotifyUnknownAction?.Invoke(this, args);
        }

        private sealed class NumberEntryState
        {
            [NotNull]
            private readonly StringBuilder numberBuilder = new StringBuilder();

            public NumberEntryMode Mode { get; private set; }

            [CanBeNull]
            public int? Number => numberBuilder.Length == 0 ? (int?) null : int.Parse(numberBuilder.ToString());

            public void ChangeMode(NumberEntryMode mode)
            {
                numberBuilder.Length = 0;
                Mode = mode;
            }

            public bool AppendToNumber(int digit)
            {
                if (digit < 0 || digit > 9)
                {
                    throw new ArgumentOutOfRangeException(nameof(digit), digit, @"digit must be in range [0-9].");
                }

                if (numberBuilder.Length < MaxCompetitorNumberLength)
                {
                    numberBuilder.Append(digit.ToString(CultureInfo.InvariantCulture));
                    return true;
                }

                Log.Debug(
                    $"Discarding incoming digit {digit} because maximum competitor number length has been reached.");
                return false;
            }

            [Pure]
            public override string ToString() => $"{GetType().Name}: Mode={Mode}, Number={numberBuilder}";
        }

        private sealed class DeviceModifiersState
        {
            [NotNull]
            private readonly Dictionary<WirelessNetworkAddress, ModifiersDownForDevice> statePerDevice =
                new Dictionary<WirelessNetworkAddress, ModifiersDownForDevice>();

            public bool IsEmpty([NotNull] WirelessNetworkAddress deviceAddress)
            {
                if (statePerDevice.ContainsKey(deviceAddress))
                {
                    return !statePerDevice[deviceAddress].ContainsEnterNextCompetitor &&
                        !statePerDevice[deviceAddress].ContainsEnterCurrentCompetitor;
                }
                return true;
            }

            public bool IsDown([NotNull] WirelessNetworkAddress deviceAddress, RemoteKeyModifier modifier)
            {
                if (statePerDevice.ContainsKey(deviceAddress))
                {
                    switch (modifier)
                    {
                        case RemoteKeyModifier.EnterNextCompetitor:
                            return statePerDevice[deviceAddress].ContainsEnterNextCompetitor;
                        case RemoteKeyModifier.EnterCurrentCompetitor:
                            return statePerDevice[deviceAddress].ContainsEnterCurrentCompetitor;
                    }
                }
                return false;
            }

            public void EnsureDown([NotNull] WirelessNetworkAddress deviceAddress, RemoteKeyModifier modifier)
            {
                ModifiersDownForDevice deviceState = !statePerDevice.ContainsKey(deviceAddress)
                    ? new ModifiersDownForDevice()
                    : statePerDevice[deviceAddress];

                switch (modifier)
                {
                    case RemoteKeyModifier.EnterNextCompetitor:
                        deviceState = deviceState.WithEnterNextCompetitor();
                        break;
                    case RemoteKeyModifier.EnterCurrentCompetitor:
                        deviceState = deviceState.WithEnterCurrentCompetitor();
                        break;
                    default:
                        throw ExceptionFactory.CreateNotSupportedExceptionFor(modifier);
                }

                statePerDevice[deviceAddress] = deviceState;
            }

            public void EnsureNotDown([NotNull] WirelessNetworkAddress deviceAddress, RemoteKeyModifier modifier)
            {
                if (statePerDevice.ContainsKey(deviceAddress))
                {
                    ModifiersDownForDevice deviceState = statePerDevice[deviceAddress];

                    switch (modifier)
                    {
                        case RemoteKeyModifier.EnterNextCompetitor:
                            deviceState = deviceState.WithoutEnterNextCompetitor();
                            break;
                        case RemoteKeyModifier.EnterCurrentCompetitor:
                            deviceState = deviceState.WithoutEnterCurrentCompetitor();
                            break;
                        default:
                            throw ExceptionFactory.CreateNotSupportedExceptionFor(modifier);
                    }

                    statePerDevice[deviceAddress] = deviceState;
                }
            }

            private struct ModifiersDownForDevice
            {
                public bool ContainsEnterCurrentCompetitor { get; }
                public bool ContainsEnterNextCompetitor { get; }

                private ModifiersDownForDevice(bool containsEnterCurrentCompetitor, bool containsEnterNextCompetitor)
                    : this()
                {
                    ContainsEnterCurrentCompetitor = containsEnterCurrentCompetitor;
                    ContainsEnterNextCompetitor = containsEnterNextCompetitor;
                }

                public ModifiersDownForDevice WithEnterCurrentCompetitor()
                {
                    return new ModifiersDownForDevice(true, ContainsEnterNextCompetitor);
                }

                public ModifiersDownForDevice WithoutEnterCurrentCompetitor()
                {
                    return new ModifiersDownForDevice(false, ContainsEnterNextCompetitor);
                }

                public ModifiersDownForDevice WithEnterNextCompetitor()
                {
                    return new ModifiersDownForDevice(ContainsEnterCurrentCompetitor, true);
                }

                public ModifiersDownForDevice WithoutEnterNextCompetitor()
                {
                    return new ModifiersDownForDevice(ContainsEnterCurrentCompetitor, false);
                }

                [Pure]
                public override string ToString()
                    => $"Current={ContainsEnterCurrentCompetitor}, Next={ContainsEnterNextCompetitor}";
            }
        }

        private enum NumberEntryMode
        {
            None,
            EnteringCurrentCompetitor,
            EnteringNextCompetitor
        }

        private enum KeyCategory
        {
            DigitOnly,
            CommandOnly,
            DigitAndCommand
        }
    }
}