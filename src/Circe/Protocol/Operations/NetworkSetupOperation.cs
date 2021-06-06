using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a controller to form a logical network configuration of devices.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A controller sends this operation to instruct a wireless device to join or leave the current logical network configuration, and/or to change assigned
    /// roles.
    /// </para>
    /// <para>
    /// Although a controller should validate its logical network configuration before it allows starting a competition run, nothing prevents another
    /// mediator to "take over" a device in the middle of a run.
    /// </para>
    /// <para>
    /// A mediator must discard any changes in assigned roles when the Set Membership parameter indicates to leave the network, to prevent altering another
    /// logical network configuration.
    /// </para>
    /// </remarks>
    public sealed class NetworkSetupOperation : Operation
    {
        internal const int TypeCode = 4;

        private readonly NetworkAddressParameter destinationAddressParameter = ParameterFactory.Create(ParameterType.NetworkAddress.DestinationAddress, true);
        private readonly BooleanParameter setMembershipParameter = ParameterFactory.Create(ParameterType.Boolean.SetMembership, true);
        private readonly IntegerParameter rolesParameter = ParameterFactory.Create(ParameterType.Integer.Roles, true);

        /// <summary>
        /// Required. Gets or sets the destination address of the device in the wireless network.
        /// </summary>
        public WirelessNetworkAddress? DestinationAddress
        {
            get
            {
                string? parameterValue = destinationAddressParameter.Value;
                return parameterValue != null ? new WirelessNetworkAddress(parameterValue) : null;
            }
            set => destinationAddressParameter.Value = value?.Value;
        }

        /// <summary>
        /// Required. Instructs whether the destination device must be part of the logical network configuration or not.
        /// </summary>
        /// <value>
        /// <c>true</c> to join network; <c>false</c> to leave network.
        /// </value>
        public bool? SetMembership
        {
            get => setMembershipParameter.Value;
            set => setMembershipParameter.Value = value;
        }

        /// <summary>
        /// Required. Gets or sets the roles that the device is going to perform in the logical network.
        /// </summary>
        public DeviceRoles? Roles
        {
            get => (DeviceRoles?)rolesParameter.Value;
            set => rolesParameter.Value = (int?)value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkSetupOperation" /> class with required parameters.
        /// </summary>
        /// <param name="destinationAddress">
        /// The destination address of the device in the wireless network.
        /// </param>
        /// <param name="setMembership">
        /// Instructs whether the destination device must be part of the logical network configuration or not. Set to <c>true</c> to join network or <c>false</c>
        /// to leave network.
        /// </param>
        /// <param name="roles">
        /// The subset of capabilities that a device performs or is going to perform in the logical network.
        /// </param>
        public NetworkSetupOperation(WirelessNetworkAddress destinationAddress, bool setMembership, DeviceRoles roles)
            : this()
        {
            Guard.NotNull(destinationAddress, nameof(destinationAddress));

            DestinationAddress = destinationAddress;
            SetMembership = setMembership;
            Roles = roles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkSetupOperation" /> class.
        /// </summary>
        internal NetworkSetupOperation()
            : base(TypeCode)
        {
            Parameters.Add(destinationAddressParameter);
            Parameters.Add(setMembershipParameter);
            Parameters.Add(rolesParameter);
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
