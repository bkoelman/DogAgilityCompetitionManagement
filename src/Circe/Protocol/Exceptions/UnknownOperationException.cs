namespace DogAgilityCompetition.Circe.Protocol.Exceptions;

/// <summary>
/// Represents the error when a packet with an unknown operation code is encountered.
/// </summary>
public sealed class UnknownOperationException : PacketFormatException
{
    /// <summary>
    /// Gets the code of the unknown operation.
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownOperationException" /> class.
    /// </summary>
    /// <param name="packet">
    /// The source packet buffer.
    /// </param>
    /// <param name="code">
    /// The code of the unknown operation.
    /// </param>
    public UnknownOperationException(byte[] packet, int code)
        : base(packet, 0, $"Unsupported operation code {code}.")
    {
        Code = code;
    }
}
