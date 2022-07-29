namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// Lists the various capabilities that can be supported by wireless devices.
/// </summary>
[Flags]
public enum DeviceCapabilities
{
    None = 0,

    // Indicates that a device contains buttons for Ready and Reset, and optionally
    // ToggleElimination, +/- Faults, +/- Refusals.
    ControlKeypad = 1,

    // Indicates that a remote control contains buttons for Current/Next and numeric keys 0-9.
    // This capability does not make a device eligible for any role.
    NumericKeypad = 1 << 1,

    // Indicates that a device can be used to signal Start time.
    StartSensor = 1 << 2,

    // Indicates that a device can be used to signal Finish time.
    FinishSensor = 1 << 3,

    // Indicates that a device can be used to signal Intermediate time.
    IntermediateSensor = 1 << 4,

    // Mutually exclusive. Indicates that a device can be used to signal time.
    TimeSensor = 1 << 5,

    // Mutually exclusive. Indicates that a device can visualize run status.
    Display = 1 << 6,

    All = ControlKeypad | NumericKeypad | StartSensor | FinishSensor | IntermediateSensor | TimeSensor | Display
}
