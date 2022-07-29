using System.Runtime.Serialization;

namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// Lists the various roles that wireless devices can fulfill in a logical network configuration.
/// </summary>
[Flags]
[DataContract]
public enum DeviceRoles
{
    None = 0,

    [EnumMember]
    Keypad = 1,

    [EnumMember]
    StartTimer = 1 << 1,

    [EnumMember]
    FinishTimer = 1 << 2,

    [EnumMember]
    IntermediateTimer1 = 1 << 3,

    [EnumMember]
    IntermediateTimer2 = 1 << 4,

    [EnumMember]
    IntermediateTimer3 = 1 << 5,

    [EnumMember]
    Display = 1 << 6
}
