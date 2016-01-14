using System;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary />
    public sealed class IncomingOperationEventArgs : EventArgs
    {
        [NotNull]
        public Operation Operation { get; private set; }

        [NotNull]
        public CirceComConnection Connection { get; private set; }

        public IncomingOperationEventArgs([NotNull] Operation operation, [NotNull] CirceComConnection connection)
        {
            Guard.NotNull(operation, nameof(operation));
            Guard.NotNull(connection, nameof(connection));

            Operation = operation;
            Connection = connection;
        }
    }
}