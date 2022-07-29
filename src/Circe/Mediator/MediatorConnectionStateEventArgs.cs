namespace DogAgilityCompetition.Circe.Mediator;

/// <summary />
public sealed class MediatorConnectionStateEventArgs : EventArgs
{
    /// <summary>
    /// Gets the new connection state.
    /// </summary>
    public MediatorConnectionState State { get; }

    /// <summary>
    /// Gets the name of the COM port, if available.
    /// </summary>
    public string? ComPort { get; }

    public MediatorConnectionStateEventArgs(MediatorConnectionState state, string? comPort)
    {
        State = state;
        ComPort = comPort;
    }
}
