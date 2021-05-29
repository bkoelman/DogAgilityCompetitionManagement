using System;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary>
    /// Lists the features that the remote emulator can support.
    /// </summary>
    [Flags]
    public enum RemoteEmulatorFeatures
    {
        None = 0,
        CoreKeys = 1,
        NumericKeys = 1 << 1,
        TimerKeys = 1 << 2,

        All = CoreKeys | NumericKeys | TimerKeys
    }

    /// <summary />
    public static class RemoteEmulatorFeaturesExtensions
    {
        public static DeviceCapabilities ToCapabilities(this RemoteEmulatorFeatures features)
        {
            var capabilities = DeviceCapabilities.None;

            if ((features & RemoteEmulatorFeatures.CoreKeys) != 0)
            {
                capabilities |= DeviceCapabilities.ControlKeypad;
            }

            if ((features & RemoteEmulatorFeatures.NumericKeys) != 0)
            {
                capabilities |= DeviceCapabilities.NumericKeypad;
            }

            if ((features & RemoteEmulatorFeatures.TimerKeys) != 0)
            {
                capabilities |= DeviceCapabilities.StartSensor | DeviceCapabilities.IntermediateSensor | DeviceCapabilities.FinishSensor;
            }

            return capabilities;
        }
    }
}
