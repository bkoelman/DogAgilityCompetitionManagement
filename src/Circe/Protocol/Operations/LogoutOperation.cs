namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a controller to end communications with a mediator.
    /// </summary>
    /// <remarks>
    /// This operation is sent by a controller to terminate communications. After receiving this operation, the mediator should not send any more operations
    /// until another Login (01) operation has been received.
    /// </remarks>
    public sealed class LogoutOperation : Operation
    {
        internal const int TypeCode = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutOperation" /> class.
        /// </summary>
        public LogoutOperation()
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
