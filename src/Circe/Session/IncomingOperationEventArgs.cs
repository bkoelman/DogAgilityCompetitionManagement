using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Circe.Session;

/// <summary />
public sealed class IncomingOperationEventArgs : EventArgs
{
    public Operation Operation { get; }
    public CirceComConnection Connection { get; }

    public IncomingOperationEventArgs(Operation operation, CirceComConnection connection)
    {
        Guard.NotNull(operation, nameof(operation));
        Guard.NotNull(connection, nameof(connection));

        Operation = operation;
        Connection = connection;
    }
}
