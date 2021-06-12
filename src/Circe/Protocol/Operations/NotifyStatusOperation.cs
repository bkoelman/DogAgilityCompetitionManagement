using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a mediator to describe the current status of a single device in the wireless network.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation is periodically sent by a mediator to describe the current status of a single device in the wireless network.
    /// </para>
    /// <para>
    /// Besides device status display, it can be used by a controller for device discovery. A device is considered off-line when more than three seconds have
    /// elapsed since its previous status notification was sent.
    /// </para>
    /// </remarks>
    public sealed class NotifyStatusOperation : Operation
    {
        internal const int TypeCode = 52;

        private readonly NetworkAddressParameter originatingAddressParameter = ParameterFactory.Create(ParameterType.NetworkAddress.OriginatingAddress, true);
        private readonly BooleanParameter getMembershipParameter = ParameterFactory.Create(ParameterType.Boolean.GetMembership, true);
        private readonly IntegerParameter capabilitiesParameter = ParameterFactory.Create(ParameterType.Integer.Capabilities, true);
        private readonly IntegerParameter rolesParameter = ParameterFactory.Create(ParameterType.Integer.Roles, true);
        private readonly IntegerParameter signalStrengthParameter = ParameterFactory.Create(ParameterType.Integer.SignalStrength, true);
        private readonly IntegerParameter batteryStatusParameter = ParameterFactory.Create(ParameterType.Integer.BatteryStatus, false);
        private readonly BooleanParameter isAlignedParameter = ParameterFactory.Create(ParameterType.Boolean.IsAligned, false);
        private readonly IntegerParameter clockSynchronizationParameter = ParameterFactory.Create(ParameterType.Integer.ClockSynchronization, false);
        private readonly BooleanParameter hasVersionMismatchParameter = ParameterFactory.Create(ParameterType.Boolean.HasVersionMismatch, false);

        /// <summary>
        /// Required. Gets or sets the originating address of the device in the wireless network.
        /// </summary>
        public WirelessNetworkAddress? OriginatingAddress
        {
            get
            {
                string? parameterValue = originatingAddressParameter.Value;
                return parameterValue != null ? new WirelessNetworkAddress(parameterValue) : null;
            }
            set => originatingAddressParameter.Value = value?.Value;
        }

        /// <summary>
        /// Required. Indicates whether the device is part of the logical network configuration.
        /// </summary>
        /// <value>
        /// <c>true</c> when the device is part of the network; <c>false</c> if it is not.
        /// </value>
        public bool? GetMembership
        {
            get => getMembershipParameter.Value;
            set => getMembershipParameter.Value = value;
        }

        /// <summary>
        /// Required. Gets or sets the capabilities that the device can perform.
        /// </summary>
        public DeviceCapabilities? Capabilities
        {
            get => (DeviceCapabilities?)capabilitiesParameter.Value;
            set => capabilitiesParameter.Value = (int?)value;
        }

        /// <summary>
        /// Required. Gets or sets the roles that the device currently performs in the logical network.
        /// </summary>
        public DeviceRoles? Roles
        {
            get => (DeviceRoles?)rolesParameter.Value;
            set => rolesParameter.Value = (int?)value;
        }

        /// <summary>
        /// Required. Gets or sets the wireless signal strength. Higher values indicate a better signal.
        /// </summary>
        /// <value>
        /// Range: 0 - 255 (inclusive)
        /// </value>
        public int? SignalStrength
        {
            get => signalStrengthParameter.Value;
            set => signalStrengthParameter.Value = value;
        }

        /// <summary>
        /// Optional. Gets or sets the battery status of the device. Higher values indicate longer battery lifetime.
        /// </summary>
        /// <value>
        /// Range: 0 - 255 (inclusive)
        /// </value>
        public int? BatteryStatus
        {
            get => batteryStatusParameter.Value;
            set => batteryStatusParameter.Value = value;
        }

        /// <summary>
        /// Optional. Applies to gates only. Indicates whether this passage detector is properly aligned.
        /// </summary>
        public bool? IsAligned
        {
            get => isAlignedParameter.Value;
            set => isAlignedParameter.Value = value;
        }

        /// <summary>
        /// Optional. Indicates the status of hardware clock synchronization for this device.
        /// </summary>
        public ClockSynchronizationStatus? ClockSynchronization
        {
            get => clockSynchronizationParameter.Value == null ? null : (ClockSynchronizationStatus?)clockSynchronizationParameter.Value;
            set => clockSynchronizationParameter.Value = value == null ? null : (int?)value;
        }

        /// <summary>
        /// Optional. Indicates whether the version of a device matches the mediator version.
        /// </summary>
        public bool? HasVersionMismatch
        {
            get => hasVersionMismatchParameter.Value;
            set => hasVersionMismatchParameter.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyStatusOperation" /> class with required parameters.
        /// </summary>
        /// <param name="originatingAddress">
        /// The originating address of the device in the wireless network.
        /// </param>
        /// <param name="getMembership">
        /// Indicates whether the device is part of the logical network configuration.
        /// </param>
        /// <param name="capabilities">
        /// The capabilities that the device can perform.
        /// </param>
        /// <param name="roles">
        /// The subset of <paramref name="capabilities" /> that the device currently performs in the logical network.
        /// </param>
        /// <param name="signalStrength">
        /// The wireless signal strength. Higher values indicate a better signal.
        /// </param>
        public NotifyStatusOperation(WirelessNetworkAddress originatingAddress, bool getMembership, DeviceCapabilities capabilities, DeviceRoles roles,
            int signalStrength)
            : this()
        {
            Guard.NotNull(originatingAddress, nameof(originatingAddress));

            OriginatingAddress = originatingAddress;
            GetMembership = getMembership;
            Capabilities = capabilities;
            Roles = roles;
            SignalStrength = signalStrength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyStatusOperation" /> class.
        /// </summary>
        internal NotifyStatusOperation()
            : base(TypeCode)
        {
            Parameters.Add(originatingAddressParameter);
            Parameters.Add(getMembershipParameter);
            Parameters.Add(capabilitiesParameter);
            Parameters.Add(rolesParameter);
            Parameters.Add(signalStrengthParameter);
            Parameters.Add(batteryStatusParameter);
            Parameters.Add(isAlignedParameter);
            Parameters.Add(clockSynchronizationParameter);
            Parameters.Add(hasVersionMismatchParameter);
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
