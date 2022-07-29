using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// In the process of input handling, tracks changes in the raw key flags from devices and raises up/down events for keys and key modifiers.
/// </summary>
public sealed class RemoteKeyTracker
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private static readonly Dictionary<RawDeviceKeys, RemoteKeyModifier> ModifierKeyTranslationTable = new()
    {
        { RawDeviceKeys.EnterNextCompetitor, RemoteKeyModifier.EnterNextCompetitor },
        { RawDeviceKeys.EnterCurrentCompetitor, RemoteKeyModifier.EnterCurrentCompetitor }
    };

    private static readonly Dictionary<RawDeviceKeys, RemoteKey> RegularKeyTranslationTable = new()
    {
        { RawDeviceKeys.Key1OrPlaySoundA, RemoteKey.Key1OrPlaySoundA },
        { RawDeviceKeys.Key2OrPassIntermediate, RemoteKey.Key2OrPassIntermediate },
        { RawDeviceKeys.Key3OrToggleElimination, RemoteKey.Key3OrToggleElimination },
        { RawDeviceKeys.Key4, RemoteKey.Key4 },
        { RawDeviceKeys.Key5OrDecreaseRefusals, RemoteKey.Key5OrDecreaseRefusals },
        { RawDeviceKeys.Key6OrIncreaseRefusals, RemoteKey.Key6OrIncreaseRefusals },
        { RawDeviceKeys.Key7, RemoteKey.Key7 },
        { RawDeviceKeys.Key8OrDecreaseFaults, RemoteKey.Key8OrDecreaseFaults },
        { RawDeviceKeys.Key9OrIncreaseFaults, RemoteKey.Key9OrIncreaseFaults },
        { RawDeviceKeys.Key0OrMuteSound, RemoteKey.Key0OrMuteSound },
        { RawDeviceKeys.PassFinish, RemoteKey.PassFinish },
        { RawDeviceKeys.PassStart, RemoteKey.PassStart },
        { RawDeviceKeys.ResetRun, RemoteKey.ResetRun },
        { RawDeviceKeys.Ready, RemoteKey.Ready }
    };

    private readonly Dictionary<WirelessNetworkAddress, RawDeviceKeys> precedingRawKeysDownPerDevice = new();

    public event EventHandler<RemoteKeyModifierEventArgs>? ModifierKeyDown;
    public event EventHandler<RemoteKeyEventArgs>? KeyDown;
    public event EventHandler<RemoteKeyEventArgs>? KeyUp;
    public event EventHandler<RemoteKeyModifierEventArgs>? ModifierKeyUp;
    public event EventHandler<DeviceTimeEventArgs>? MissingKey;

    public void ProcessDeviceAction(DeviceAction deviceAction)
    {
        Guard.NotNull(deviceAction, nameof(deviceAction));

        if (deviceAction.InputKeys == null)
        {
            var args = new DeviceTimeEventArgs(deviceAction.DeviceAddress, deviceAction.SensorTime);
            MissingKey?.Invoke(this, args);
        }
        else
        {
            ProcessRawKeysDown(deviceAction.DeviceAddress, deviceAction.InputKeys.Value, deviceAction.SensorTime);
        }
    }

    private void ProcessRawKeysDown(WirelessNetworkAddress source, RawDeviceKeys rawKeysDown, TimeSpan? sensorTime)
    {
        RawDeviceKeys precedingKeysDown = GetPrecedingKeysDownForDevice(source);

        RaiseModifierDownEvents(source, rawKeysDown, precedingKeysDown, sensorTime);

        RaiseKeyDownEvents(source, rawKeysDown, precedingKeysDown, sensorTime);

        RaiseKeyUpEvents(source, rawKeysDown, precedingKeysDown, sensorTime);

        RaiseModifierUpEvents(source, rawKeysDown, precedingKeysDown, sensorTime);

        precedingRawKeysDownPerDevice[source] = rawKeysDown;
    }

    private RawDeviceKeys GetPrecedingKeysDownForDevice(WirelessNetworkAddress source)
    {
        return precedingRawKeysDownPerDevice.ContainsKey(source) ? precedingRawKeysDownPerDevice[source] : RawDeviceKeys.None;
    }

    private void RaiseModifierDownEvents(WirelessNetworkAddress source, RawDeviceKeys rawKeysDown, RawDeviceKeys precedingKeysDown, TimeSpan? sensorTime)
    {
        foreach (KeyValuePair<RawDeviceKeys, RemoteKeyModifier> entry in ModifierKeyTranslationTable)
        {
            ChangeType changeType = GetChangeForKey(entry.Key, rawKeysDown, precedingKeysDown);

            if (changeType == ChangeType.Down)
            {
                Log.Debug($"ModifierKeyDown of {entry.Value} from {source}");

                var args = new RemoteKeyModifierEventArgs(source, entry.Value, sensorTime);
                ModifierKeyDown?.Invoke(this, args);
            }
        }
    }

    private void RaiseKeyDownEvents(WirelessNetworkAddress source, RawDeviceKeys rawKeysDown, RawDeviceKeys precedingKeysDown, TimeSpan? sensorTime)
    {
        foreach (KeyValuePair<RawDeviceKeys, RemoteKey> entry in RegularKeyTranslationTable)
        {
            ChangeType changeType = GetChangeForKey(entry.Key, rawKeysDown, precedingKeysDown);

            if (changeType == ChangeType.Down)
            {
                Log.Debug($"KeyDown of {entry.Value} from {source}");

                var args = new RemoteKeyEventArgs(source, entry.Value, sensorTime);
                KeyDown?.Invoke(this, args);
            }
        }
    }

    private void RaiseKeyUpEvents(WirelessNetworkAddress source, RawDeviceKeys rawKeysDown, RawDeviceKeys precedingKeysDown, TimeSpan? sensorTime)
    {
        foreach (KeyValuePair<RawDeviceKeys, RemoteKey> entry in RegularKeyTranslationTable)
        {
            ChangeType changeType = GetChangeForKey(entry.Key, rawKeysDown, precedingKeysDown);

            if (changeType == ChangeType.Up)
            {
                Log.Debug($"KeyUp of {entry.Value} from {source}");

                var args = new RemoteKeyEventArgs(source, entry.Value, sensorTime);
                KeyUp?.Invoke(this, args);
            }
        }
    }

    private void RaiseModifierUpEvents(WirelessNetworkAddress source, RawDeviceKeys rawKeysDown, RawDeviceKeys precedingKeysDown, TimeSpan? sensorTime)
    {
        foreach (KeyValuePair<RawDeviceKeys, RemoteKeyModifier> entry in ModifierKeyTranslationTable)
        {
            ChangeType changeType = GetChangeForKey(entry.Key, rawKeysDown, precedingKeysDown);

            if (changeType == ChangeType.Up)
            {
                Log.Debug($"ModifierKeyUp of {entry.Value} from {source}");

                var args = new RemoteKeyModifierEventArgs(source, entry.Value, sensorTime);
                ModifierKeyUp?.Invoke(this, args);
            }
        }
    }

    private static ChangeType GetChangeForKey(RawDeviceKeys targetKey, RawDeviceKeys rawKeysDown, RawDeviceKeys precedingKeysDown)
    {
        RawDeviceKeys lastKeyValue = precedingKeysDown & targetKey;
        RawDeviceKeys thisKeyValue = rawKeysDown & targetKey;

        bool hasChanged = (lastKeyValue ^ thisKeyValue) != 0;

        return !hasChanged ? ChangeType.None : thisKeyValue != 0 ? ChangeType.Down : ChangeType.Up;
    }

    private enum ChangeType
    {
        None,
        Down,
        Up
    }
}
