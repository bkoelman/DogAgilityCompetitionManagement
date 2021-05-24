using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Exceptions
{
    /// <summary>
    /// Represents the error that is thrown when the bytes of a parameter value inside a CIRCE packet could not be parsed.
    /// </summary>
    [Serializable]
    public sealed class ParameterValueFormatException : PacketFormatException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterValueFormatException" /> class.
        /// </summary>
        /// <param name="packet">
        /// The source packet buffer.
        /// </param>
        /// <param name="errorOffset">
        /// The offset in <paramref name="packet" /> where the error was found.
        /// </param>
        /// <param name="message">
        /// The error message text.
        /// </param>
        /// <param name="innerException">
        /// Optional. The exception that caused the current exception.
        /// </param>
        public ParameterValueFormatException([NotNull] byte[] packet, int errorOffset, [NotNull] string message,
            [CanBeNull] Exception innerException)
            : base(packet, errorOffset, message, innerException)
        {
        }

        private ParameterValueFormatException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}