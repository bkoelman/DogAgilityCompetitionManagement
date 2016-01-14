using System;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Lists the various input keys as defined in the CIRCE protocol specification.
    /// </summary>
    [Flags]
    public enum RawDeviceKeys
    {
        None = 0,
        Key1OrPlaySoundA = 1,
        Key2OrPassIntermediate = 1 << 1,
        Key3OrToggleElimination = 1 << 2,
        Key4 = 1 << 3,
        Key5OrDecreaseRefusals = 1 << 4,
        Key6OrIncreaseRefusals = 1 << 5,
        Key7 = 1 << 6,
        Key8OrDecreaseFaults = 1 << 7,
        Key9OrIncreaseFaults = 1 << 8,
        EnterCurrentCompetitor = 1 << 9,
        EnterNextCompetitor = 1 << 10,
        Key0OrMuteSound = 1 << 11,
        PassFinish = 1 << 12,
        PassStart = 1 << 13,
        ResetRun = 1 << 14,
        Ready = 1 << 15
    }
}