using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a controller during the manufacturing process of a new mediator or other new wireless hardware device to assign its unique
    /// network address.
    /// </summary>
    /// <remarks>
    /// A controller may send this operation in response to a Keep Alive (51) operation from a mediator. Upon receipt of this operation, the mediator assigns
    /// the included address to itself when capabilities are omitted. Otherwise, the included address is assigned to the wireless device that matches the
    /// destination address. When destination address is missing, the mediator assumes address 000000.
    /// </remarks>
    public sealed class DeviceSetupOperation : Operation
    {
        internal const int TypeCode = 5;

        private readonly NetworkAddressParameter destinationAddressParameter = ParameterFactory.Create(ParameterType.NetworkAddress.DestinationAddress, false);
        private readonly NetworkAddressParameter assignAddressParameter = ParameterFactory.Create(ParameterType.NetworkAddress.AssignAddress, true);
        private readonly IntegerParameter capabilitiesParameter = ParameterFactory.Create(ParameterType.Integer.Capabilities, false);

        /// <summary>
        /// Optional. Gets or sets the destination address of the device in the wireless network.
        /// </summary>
        public WirelessNetworkAddress? DestinationAddress
        {
            get
            {
                string? parameterValue = destinationAddressParameter.GetValueOrNull();
                return parameterValue != null ? new WirelessNetworkAddress(parameterValue) : null;
            }
            set => destinationAddressParameter.Value = value?.Value;
        }

        public WirelessNetworkAddress DestinationAddressOrDefault => DestinationAddress ?? WirelessNetworkAddress.Default;

        /// <summary>
        /// Required. Gets or sets the address to assign to the new hardware device in the wireless network.
        /// </summary>
        public WirelessNetworkAddress? AssignAddress
        {
            get
            {
                string? parameterValue = assignAddressParameter.GetValueOrNull();
                return parameterValue != null ? new WirelessNetworkAddress(parameterValue) : null;
            }
            set => assignAddressParameter.Value = value?.Value;
        }

        /// <summary>
        /// Optional. Gets or sets the capabilities that the device can perform.
        /// </summary>
        public DeviceCapabilities? Capabilities
        {
            get => (DeviceCapabilities?)capabilitiesParameter.Value;
            set => capabilitiesParameter.Value = (int?)value;
        }

        public DeviceCapabilities CapabilitiesOrNone => Capabilities ?? DeviceCapabilities.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSetupOperation" /> class with required parameters.
        /// </summary>
        /// <param name="assignAddress">
        /// The address to assign to the new hardware device in the wireless network.
        /// </param>
        public DeviceSetupOperation(WirelessNetworkAddress assignAddress)
            : this()
        {
            Guard.NotNull(assignAddress, nameof(assignAddress));

            AssignAddress = assignAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSetupOperation" /> class.
        /// </summary>
        internal DeviceSetupOperation()
            : base(TypeCode)
        {
            Parameters.Add(destinationAddressParameter);
            Parameters.Add(assignAddressParameter);
            Parameters.Add(capabilitiesParameter);
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
