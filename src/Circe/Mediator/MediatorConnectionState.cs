namespace DogAgilityCompetition.Circe.Mediator
{
    /// <summary>
    /// Lists the stages when a mediator attempts to connect with a CIRCE controller.
    /// </summary>
    public enum MediatorConnectionState
    {
        WaitingForComPort,
        WaitingForLogin,
        LoginReceived,
        Connected,
        Disconnected
    }
}
