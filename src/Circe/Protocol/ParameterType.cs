using System.Diagnostics.CodeAnalysis;
using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// Contains the set of predefined CIRCE parameters, grouped by parameter type.
/// </summary>
public static class ParameterType
{
    /// <summary>
    /// Lists the predefined <see cref="IntegerParameter" /> types.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
    public enum Integer
    {
        MediatorStatus,
        Capabilities,
        Roles,
        SignalStrength,
        BatteryStatus,
        InputKeys,
        SensorTime,
        ClockSynchronization,
        CurrentCompetitorNumber,
        NextCompetitorNumber,
        StartTimer,
        PrimaryTimerValue,
        SecondaryTimerValue,
        FaultCount,
        RefusalCount,
        PreviousPlacement
    }

    /// <summary>
    /// Lists the predefined <see cref="BooleanParameter" /> types.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
    public enum Boolean
    {
        GetMembership,
        SetMembership,
        IsAligned,
        Eliminated,
        HasVersionMismatch
    }

    /// <summary>
    /// Lists the predefined <see cref="NetworkAddressParameter" /> types.
    /// </summary>
    public enum NetworkAddress
    {
        DestinationAddress,
        OriginatingAddress,
        AssignAddress
    }

    /// <summary>
    /// Lists the predefined <see cref="VersionParameter" /> types.
    /// </summary>
    public enum Version
    {
        ProtocolVersion
    }

    /// <summary>
    /// Lists the predefined <see cref="BinaryParameter" /> types.
    /// </summary>
    [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
    public enum Binary
    {
        LogData
    }
}
