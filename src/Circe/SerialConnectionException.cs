namespace DogAgilityCompetition.Circe;

/// <summary>
/// The error that occurs while attempting to open an available COM port.
/// </summary>
public sealed class SerialConnectionException : Exception
{
    public SerialConnectionException(string? message)
        : base(message)
    {
    }
}
