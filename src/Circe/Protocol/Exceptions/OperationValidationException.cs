namespace DogAgilityCompetition.Circe.Protocol.Exceptions;

/// <summary>
/// Represents the error that is thrown when the logical contents of a CIRCE packet is not compliant with the protocol specification.
/// </summary>
public sealed class OperationValidationException : Exception
{
    public Operation Operation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OperationValidationException" /> class.
    /// </summary>
    /// <param name="operation">
    /// The operation whose contents is invalid.
    /// </param>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public OperationValidationException(Operation operation, string message)
        : base(FormatMessage(operation, message))
    {
        Guard.NotNull(operation, nameof(operation));

        Operation = operation;
    }

    private static string FormatMessage(Operation operation, string message)
    {
        return $"{operation.GetType().Name} ({operation.Code}): {message}";
    }
}
