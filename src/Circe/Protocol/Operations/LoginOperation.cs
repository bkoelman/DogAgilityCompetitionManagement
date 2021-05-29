using System;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a controller to start communications with a mediator. It is sent before any operations.
    /// </summary>
    /// <remarks>
    /// Login must be the first operation sent by a controller to initialize communications with a mediator. The mediator responds by sending a Keep Alive
    /// (51) operation. This enables a controller to detect at which physical port a mediator is connected and rescan ports when the connection has been
    /// interrupted.
    /// </remarks>
    [Serializable]
    public sealed class LoginOperation : Operation
    {
        internal const int TypeCode = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginOperation" /> class.
        /// </summary>
        public LoginOperation()
            : base(TypeCode)
        {
        }

        /// <summary>
        /// Implements the Visitor design pattern.
        /// </summary>
        /// <param name="acceptor">
        /// The object accepting this operation.
        /// </param>
        public override void Visit(IOperationAcceptor acceptor)
        {
            Guard.NotNull(acceptor, nameof(acceptor));

            acceptor.Accept(this);
        }
    }
}
