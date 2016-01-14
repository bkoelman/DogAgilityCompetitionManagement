using System;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a mediator to describe the current status of a single device in the wireless network.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation is periodically sent by a mediator to describe the current status of a single device in the wireless
    /// network.
    /// </para>
    /// <para>
    /// Besides device status display, it can be used by a controller for device discovery. A device is considered off-line
    /// when more than three seconds have elapsed since its previous status notification was sent.
    /// </para>
    /// </remarks>
    [Serializable]
    public sealed class NotifyStatusOperation : Operation
    {
        internal const int TypeCode = 52;

        [NotNull]
        private readonly NetworkAddressParameter originatingAddressParameter =
            ParameterFactory.Create(ParameterType.NetworkAddress.OriginatingAddress, true);

        [NotNull]
        private readonly BooleanParameter getMembershipParameter =
            ParameterFactory.Create(ParameterType.Boolean.GetMembership, true);

        [NotNull]
        private readonly IntegerParameter capabilitiesParameter =
            ParameterFactory.Create(ParameterType.Integer.Capabilities, true);

        [NotNull]
        private readonly IntegerParameter rolesParameter = ParameterFactory.Create(ParameterType.Integer.Roles, true);

        [NotNull]
        private readonly IntegerParameter signalStrengthParameter =
            ParameterFactory.Create(ParameterType.Integer.SignalStrength, true);

        [NotNull]
        private readonly IntegerParameter batteryStatusParameter =
            ParameterFactory.Create(ParameterType.Integer.BatteryStatus, false);

        [NotNull]
        private readonly BooleanParameter isAlignedParameter = ParameterFactory.Create(ParameterType.Boolean.IsAligned,
            false);

        [NotNull]
        private readonly IntegerParameter clockSynchronizationParameter =
            ParameterFactory.Create(ParameterType.Integer.ClockSynchronization, false);

        [NotNull]
        private readonly BooleanParameter hasVersionMismatchParameter =
            ParameterFactory.Create(ParameterType.Boolean.HasVersionMismatch, false);

        /// <summary>
        /// Required. Gets or sets the originating address of the device in the wireless network.
        /// </summary>
        [CanBeNull]
        public WirelessNetworkAddress OriginatingAddress
        {
            get
            {
                string parameterValue = originatingAddressParameter.GetValueOrNull();
                return parameterValue != null ? new WirelessNetworkAddress(parameterValue) : null;
            }
            set
            {
                originatingAddressParameter.Value = value?.Value;
            }
        }

        /// <summary>
        /// Required. Indicates whether the device is part of the logical network configuration.
        /// </summary>
        /// <value>
        /// <c>true</c> when the device is part of the network; <c>false</c> if it is not.
        /// </value>
        [CanBeNull]
        public bool? GetMembership
        {
            get
            {
                return getMembershipParameter.Value;
            }
            set
            {
                getMembershipParameter.Value = value;
            }
        }

        /// <summary>
        /// Required. Gets or sets the capabilities that the device can perform.
        /// </summary>
        [CanBeNull]
        public DeviceCapabilities? Capabilities
        {
            get
            {
                return (DeviceCapabilities?) capabilitiesParameter.Value;
            }
            set
            {
                capabilitiesParameter.Value = (int?) value;
            }
        }

        /// <summary>
        /// Required. Gets or sets the roles that the device currently performs in the logical network.
        /// </summary>
        [CanBeNull]
        public DeviceRoles? Roles
        {
            get
            {
                return (DeviceRoles?) rolesParameter.Value;
            }
            set
            {
                rolesParameter.Value = (int?) value;
            }
        }

        /// <summary>
        /// Required. Gets or sets the wireless signal strength. Higher values indicate a better signal.
        /// </summary>
        /// <value>
        /// Range: 0 - 255 (inclusive)
        /// </value>
        [CanBeNull]
        public int? SignalStrength
        {
            get
            {
                return signalStrengthParameter.Value;
            }
            set
            {
                signalStrengthParameter.Value = value;
            }
        }

        /// <summary>
        /// Optional. Gets or sets the battery status of the device. Higher values indicate longer battery lifetime.
        /// </summary>
        /// <value>
        /// Range: 0 - 255 (inclusive)
        /// </value>
        [CanBeNull]
        public int? BatteryStatus
        {
            get
            {
                return batteryStatusParameter.Value;
            }
            set
            {
                batteryStatusParameter.Value = value;
            }
        }

        /// <summary>
        /// Optional. Applies to gates only. Indicates whether this passage detector is properly aligned.
        /// </summary>
        [CanBeNull]
        public bool? IsAligned
        {
            get
            {
                return isAlignedParameter.Value;
            }
            set
            {
                isAlignedParameter.Value = value;
            }
        }

        /// <summary>
        /// Optional. Indicates the status of hardware clock synchronization for this device.
        /// </summary>
        [CanBeNull]
        public ClockSynchronizationStatus? ClockSynchronization
        {
            get
            {
                return clockSynchronizationParameter.Value == null
                    ? null
                    : (ClockSynchronizationStatus?) clockSynchronizationParameter.Value;
            }
            set
            {
                clockSynchronizationParameter.Value = value == null ? null : (int?) value;
            }
        }

        /// <summary>
        /// Optional. Indicates whether the version of a device matches the mediator version.
        /// </summary>
        [CanBeNull]
        public bool? HasVersionMismatch
        {
            get
            {
                return hasVersionMismatchParameter.Value;
            }
            set
            {
                hasVersionMismatchParameter.Value = value;
            }
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
        public NotifyStatusOperation([NotNull] WirelessNetworkAddress originatingAddress, bool getMembership,
            DeviceCapabilities capabilities, DeviceRoles roles, int signalStrength)
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