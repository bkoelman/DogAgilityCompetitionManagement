using System;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation can be used by a controller to activate a presence indication on a wireless device.
    /// </summary>
    [Serializable]
    public sealed class AlertOperation : Operation
    {
        internal const int TypeCode = 3;

        [NotNull]
        private readonly NetworkAddressParameter destinationAddressParameter =
            ParameterFactory.Create(ParameterType.NetworkAddress.DestinationAddress, true);

        /// <summary>
        /// Required. Gets or sets the destination address of the device in the wireless network.
        /// </summary>
        [CanBeNull]
        public WirelessNetworkAddress DestinationAddress
        {
            get
            {
                string parameterValue = destinationAddressParameter.GetValueOrNull();
                return parameterValue != null ? new WirelessNetworkAddress(parameterValue) : null;
            }
            set
            {
                destinationAddressParameter.Value = value?.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertOperation" /> class.
        /// </summary>
        internal AlertOperation()
            : base(TypeCode)
        {
            Parameters.Add(destinationAddressParameter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertOperation" /> class with required parameters.
        /// </summary>
        /// <param name="destinationAddress">
        /// The destination address of the device in the wireless network.
        /// </param>
        public AlertOperation([NotNull] WirelessNetworkAddress destinationAddress)
            : this()
        {
            Guard.NotNull(destinationAddress, nameof(destinationAddress));

            DestinationAddress = destinationAddress;
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