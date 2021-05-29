using System;
using System.Collections.Generic;
using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// In the process of input handling, translates incoming keys/times into commands and gate passages.
    /// </summary>
    public sealed class DeviceActionAdapter
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private static readonly Dictionary<RemoteKey, DeviceCommand> KeyToCommandLookupTable = new()
        {
            { RemoteKey.Key1OrPlaySoundA, DeviceCommand.PlaySoundA },
            { RemoteKey.Key3OrToggleElimination, DeviceCommand.ToggleElimination },
            { RemoteKey.Key5OrDecreaseRefusals, DeviceCommand.DecreaseRefusals },
            { RemoteKey.Key6OrIncreaseRefusals, DeviceCommand.IncreaseRefusals },
            { RemoteKey.Key8OrDecreaseFaults, DeviceCommand.DecreaseFaults },
            { RemoteKey.Key9OrIncreaseFaults, DeviceCommand.IncreaseFaults },
            { RemoteKey.Key0OrMuteSound, DeviceCommand.MuteSound },
            { RemoteKey.ResetRun, DeviceCommand.ResetRun },
            { RemoteKey.Ready, DeviceCommand.Ready }
        };

        [NotNull]
        private readonly FreshNotNullableReference<NetworkComposition> runComposition = new(NetworkComposition.Empty);

        [NotNull]
        public NetworkComposition RunComposition
        {
            get => runComposition.Value;
            set
            {
                Guard.NotNull(value, nameof(value));
                runComposition.Value = value;
            }
        }

        public event EventHandler<DeviceCommandEventArgs> CommandReceived;
        public event EventHandler<GatePassageEventArgs> GatePassed;

        public void Adapt([NotNull] WirelessNetworkAddress source, [CanBeNull] RemoteKey? key, [CanBeNull] TimeSpan? sensorTime)
        {
            Guard.NotNull(source, nameof(source));

            if (key != null)
            {
                AdaptForKeyWithOptionalSensorTime(source, key.Value, sensorTime);
            }
            else if (sensorTime != null)
            {
                AdaptForSensorTimeWithoutKey(source, sensorTime.Value);
            }
            else
            {
                Log.Warn($"Discarding {typeof(DeviceAction).Name} from {source} because keys and time are both missing.");
            }
        }

        private void AdaptForKeyWithOptionalSensorTime([NotNull] WirelessNetworkAddress source, RemoteKey key, [CanBeNull] TimeSpan? sensorTime)
        {
            if (KeyToCommandLookupTable.ContainsKey(key))
            {
                DeviceCommand command = KeyToCommandLookupTable[key];

                if (RunComposition.IsInRoleKeypad(source))
                {
                    CommandReceived?.Invoke(this, new DeviceCommandEventArgs(source, command));
                }
            }
            else
            {
                if (sensorTime != null)
                {
                    if (key == RemoteKey.PassStart && RunComposition.IsInRoleStartTimer(source))
                    {
                        GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassStart));
                    }
                    else if (key == RemoteKey.Key2OrPassIntermediate)
                    {
                        if (RunComposition.IsInRoleIntermediateTimer1(source))
                        {
                            GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassIntermediate1));
                        }
                        else if (RunComposition.IsInRoleIntermediateTimer2(source))
                        {
                            GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassIntermediate2));
                        }
                        else if (RunComposition.IsInRoleIntermediateTimer3(source))
                        {
                            GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassIntermediate3));
                        }
                    }
                    else if (key == RemoteKey.PassFinish && RunComposition.IsInRoleFinishTimer(source))
                    {
                        GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassFinish));
                    }
                }
            }
        }

        private void AdaptForSensorTimeWithoutKey([NotNull] WirelessNetworkAddress source, TimeSpan sensorTime)
        {
            if (RunComposition.IsStartFinishGate(source))
            {
                GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassStartFinish));
            }
            else if (RunComposition.IsInRoleStartTimer(source))
            {
                GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassStart));
            }
            else if (RunComposition.IsInRoleIntermediateTimer1(source))
            {
                GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassIntermediate1));
            }
            else if (RunComposition.IsInRoleIntermediateTimer2(source))
            {
                GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassIntermediate2));
            }
            else if (RunComposition.IsInRoleIntermediateTimer3(source))
            {
                GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassIntermediate3));
            }
            else if (RunComposition.IsInRoleFinishTimer(source))
            {
                GatePassed?.Invoke(this, new GatePassageEventArgs(source, sensorTime, GatePassage.PassFinish));
            }
        }
    }
}
