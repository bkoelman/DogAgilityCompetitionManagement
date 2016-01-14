using System;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is periodically used by a controller to synchronize the hardware clocks of all wireless devices in the
    /// logical network.
    /// </summary>
    /// <remarks>
    /// The controller can determine whether the synchronization succeeded by waiting for three seconds on incoming Notify
    /// Status (52) operations from devices, which are expected to include the Clock Synchronization parameter with value Sync
    /// Succeeded.
    /// </remarks>
    [Serializable]
    public sealed class SynchronizeClocksOperation : Operation
    {
        internal const int TypeCode = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeClocksOperation" /> class.
        /// </summary>
        public SynchronizeClocksOperation()
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