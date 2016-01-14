using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Exceptions
{
    /// <summary>
    /// Represents the error that is thrown when the bytes of a CIRCE packet could not be parsed.
    /// </summary>
    [Serializable]
    public class PacketFormatException : Exception
    {
        /// <summary>
        /// Gets the zero-based offset in the source packet buffer where this error was found.
        /// </summary>
        public int ErrorOffset { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketFormatException" /> class.
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
        public PacketFormatException([NotNull] byte[] packet, int errorOffset, [NotNull] string message,
            [CanBeNull] Exception innerException = null)
            : base(FormatMessage(packet, errorOffset, message), innerException)
        {
            ErrorOffset = errorOffset;
        }

        [NotNull]
        private static string FormatMessage([NotNull] byte[] packet, int errorOffset, [NotNull] string message)
        {
            Guard.NotNull(packet, nameof(packet));

            int displayPosition = errorOffset + 1;
            return $"Error at position {displayPosition}: {message}{packet.FormatHexBuffer(4)}";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected PacketFormatException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorOffset = info.GetInt32("ErrorOffset");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));

            info.AddValue("ErrorOffset", ErrorOffset);

            base.GetObjectData(info, context);
        }
    }
}