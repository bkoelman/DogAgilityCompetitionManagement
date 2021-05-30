using System;
using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation is used by a mediator to respond to login and to keep the connection alive.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation is sent by the mediator each time more than a whole second has elapsed since it sent any previous operation. It is also sent by the
    /// mediator as a response to the Login (01) operation.
    /// </para>
    /// <para>
    /// The controller may choose to end communications by sending a Logout (02) operation when the protocol version does not match expectations or when the
    /// connection has become idle.
    /// </para>
    /// </remarks>
    [Serializable]
    public sealed class KeepAliveOperation : Operation
    {
        internal const int TypeCode = 51;

        public static readonly Version CurrentProtocolVersion = new(0, 2, 2);

        private readonly VersionParameter protocolVersionParameter = ParameterFactory.Create(ParameterType.Version.ProtocolVersion, true);
        private readonly IntegerParameter mediatorStatusParameter = ParameterFactory.Create(ParameterType.Integer.MediatorStatus, true);

        /// <summary>
        /// Required. Gets or sets the version number of the CIRCE protocol that is in use.
        /// </summary>
        /// <remarks>
        /// Only <see cref="Version.Major" />, <see cref="Version.Minor" /> and <see cref="Version.Build" /> are used, which map to Major, Minor and Release as
        /// described in the CIRCE spec. This means that <see cref="Version.Revision" /> is discarded.
        /// </remarks>
        public Version? ProtocolVersion
        {
            get => protocolVersionParameter.Value;
            set => protocolVersionParameter.Value = value;
        }

        /// <summary>
        /// Required. Indicates internal status of the mediator device. See <see cref="KnownMediatorStatusCode" /> for some predefined values.
        /// </summary>
        public int? MediatorStatus
        {
            get => mediatorStatusParameter.Value;
            set => mediatorStatusParameter.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveOperation" /> class with required parameters.
        /// </summary>
        /// <param name="protocolVersion">
        /// The version number of the CIRCE protocol that is in use.
        /// </param>
        /// <param name="mediatorStatus">
        /// Indicates internal status of the mediator device.
        /// </param>
        public KeepAliveOperation(Version protocolVersion, int mediatorStatus)
            : this()
        {
            Guard.NotNull(protocolVersion, nameof(protocolVersion));

            ProtocolVersion = protocolVersion;
            MediatorStatus = mediatorStatus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveOperation" /> class.
        /// </summary>
        internal KeepAliveOperation()
            : base(TypeCode)
        {
            Parameters.Add(protocolVersionParameter);
            Parameters.Add(mediatorStatusParameter);
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
