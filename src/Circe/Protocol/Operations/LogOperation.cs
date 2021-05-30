using System.Collections.Generic;
using System.Linq;
using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol.Operations
{
    /// <summary>
    /// This operation can be used by a mediator to communicate its internal state and is intended for technical troubleshooting.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation enables a mediator to communicate its internal state to the controller, which in turn facilitates logging to disk. This operation is
    /// intended for diagnostic purposes and can be sent at any time, also before login. The single parameter of this operation contains the (variable
    /// length) binary data to log.
    /// </para>
    /// <para>
    /// Note that this operation may be sent conditionally from a mediator, depending on compilation flags or configuration. This operation type must
    /// therefore not be used by a controller as an indication that the connection is still alive.
    /// </para>
    /// </remarks>
    public sealed class LogOperation : Operation
    {
        internal const int TypeCode = 54;

        private readonly BinaryParameter logDataParameter = ParameterFactory.Create(ParameterType.Binary.LogData, true);

        /// <summary>
        /// Required. Gets or sets the data to log.
        /// </summary>
        public IList<byte> LogData
        {
            get => logDataParameter.Value;
            set => logDataParameter.ReplaceValueWith(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOperation" /> class with required parameters.
        /// </summary>
        /// <param name="logData">
        /// The log data.
        /// </param>
        public LogOperation(IList<byte> logData)
            : this()
        {
            Guard.NotNull(logData, nameof(logData));
            LogData = logData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOperation" /> class.
        /// </summary>
        internal LogOperation()
            : base(TypeCode)
        {
            Parameters.Add(logDataParameter);
        }

        public string FormatLogData()
        {
            return LogData.ToArray().FormatHexBuffer();
        }

        /// <summary>
        /// Implements the Visitor design pattern.
        /// </summary>
        /// <param name="acceptor">
        /// The object accepting this operation.
        /// </param>
        public override void Visit(IOperationAcceptor acceptor)
        {
        }
    }
}
