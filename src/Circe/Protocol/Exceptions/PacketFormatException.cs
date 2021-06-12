using System;

namespace DogAgilityCompetition.Circe.Protocol.Exceptions
{
    /// <summary>
    /// Represents the error that is thrown when the bytes of a CIRCE packet could not be parsed.
    /// </summary>
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
        public PacketFormatException(byte[] packet, int errorOffset, string message, Exception? innerException = null)
            : base(FormatMessage(packet, errorOffset, message), innerException)
        {
            ErrorOffset = errorOffset;
        }

        private static string FormatMessage(byte[] packet, int errorOffset, string message)
        {
            Guard.NotNull(packet, nameof(packet));

            int displayPosition = errorOffset + 1;
            return $"Error at position {displayPosition}: {message}{packet.FormatHexBuffer(4)}";
        }
    }
}
