namespace DogAgilityCompetition.Circe.Controller;

/// <summary>
/// Lists the stages when a controller attempts to connect with a CIRCE mediator.
/// </summary>
public enum ControllerConnectionState
{
    WaitingForComPort,
    Connecting,
    Connected,
    Disconnected,
    ProtocolVersionMismatch,
    MediatorUnconfigured
}
