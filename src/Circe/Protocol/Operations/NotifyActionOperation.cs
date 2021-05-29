using System;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a mediator to notify about an activity that occurred in the logical network configuration. For instance, when a gate is
    /// signaled or a key on a remote control has been pressed.
    /// </summary>
    [Serializable]
    public sealed class NotifyActionOperation : Operation
    {
        internal const int TypeCode = 53;

        [NotNull]
        private readonly NetworkAddressParameter originatingAddressParameter = ParameterFactory.Create(ParameterType.NetworkAddress.OriginatingAddress, true);

        [NotNull]
        private readonly IntegerParameter inputKeysParameter = ParameterFactory.Create(ParameterType.Integer.InputKeys, false);

        [NotNull]
        private readonly IntegerParameter sensorTimeParameter = ParameterFactory.Create(ParameterType.Integer.SensorTime, false);

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
            set => originatingAddressParameter.Value = value?.Value;
        }

        /// <summary>
        /// Optional. Gets or sets the input keys on a remote control that are currently pushed down.
        /// </summary>
        [CanBeNull]
        public RawDeviceKeys? InputKeys
        {
            get => inputKeysParameter.Value == null ? null : (RawDeviceKeys?)inputKeysParameter.Value;
            set => inputKeysParameter.Value = value == null ? null : (int?)value;
        }

        /// <summary>
        /// Optional. Gets or sets the time (in whole milliseconds precision) at which a time sensor detected motion.
        /// </summary>
        [CanBeNull]
        public TimeSpan? SensorTime
        {
            get
            {
                if (sensorTimeParameter.Value == null)
                {
                    return null;
                }

                double milliseconds = (double)sensorTimeParameter.Value;

                // TimeSpan.FromMilliseconds() accepts a double as input, but it internally 
                // rounds the input value to whole milliseconds.
                return TimeSpan.FromMilliseconds(milliseconds);
            }
            set
            {
                if (value == null)
                {
                    sensorTimeParameter.Value = null;
                }
                else
                {
                    int milliseconds = (int)Math.Round(value.Value.TotalMilliseconds, MidpointRounding.AwayFromZero);
                    sensorTimeParameter.Value = milliseconds;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyActionOperation" /> class with required parameters.
        /// </summary>
        /// <param name="originatingAddress">
        /// The originating address of the device in the wireless network.
        /// </param>
        public NotifyActionOperation([NotNull] WirelessNetworkAddress originatingAddress)
            : this()
        {
            Guard.NotNull(originatingAddress, nameof(originatingAddress));

            OriginatingAddress = originatingAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyActionOperation" /> class.
        /// </summary>
        internal NotifyActionOperation()
            : base(TypeCode)
        {
            Parameters.Add(originatingAddressParameter);
            Parameters.Add(inputKeysParameter);
            Parameters.Add(sensorTimeParameter);
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
