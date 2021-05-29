using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Exceptions
{
    /// <summary>
    /// Represents the error that is thrown when the logical contents of a CIRCE packet is not compliant with the protocol specification.
    /// </summary>
    [Serializable]
    public sealed class OperationValidationException : Exception
    {
        [NotNull]
        public Operation Operation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationValidationException" /> class.
        /// </summary>
        /// <param name="operation">
        /// The operation whose contents is invalid.
        /// </param>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public OperationValidationException([NotNull] Operation operation, [NotNull] string message)
            : base(FormatMessage(operation, message))
        {
            Guard.NotNull(operation, nameof(operation));

            Operation = operation;
        }

        private OperationValidationException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Operation = (Operation)info.GetValue("Operation", typeof(Operation));
        }

        [NotNull]
        private static string FormatMessage([NotNull] Operation operation, [NotNull] string message)
        {
            return $"{operation.GetType().Name} ({operation.Code}): {message}";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));

            info.AddValue("Operation", Operation);

            base.GetObjectData(info, context);
        }
    }
}
