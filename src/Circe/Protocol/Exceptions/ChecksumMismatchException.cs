using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Exceptions
{
    /// <summary>
    /// Represents the error that is thrown when the calculated checksum of a CIRCE packet differs from the checksum that is
    /// stored in the packet.
    /// </summary>
    [Serializable]
    public sealed class ChecksumMismatchException : PacketFormatException
    {
        /// <summary>
        /// Gets the checksum value that is stored in the packet.
        /// </summary>
        public int Stored { get; }

        /// <summary>
        /// Gets the calculated checksum value.
        /// </summary>
        public int Calculated { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumMismatchException" /> class.
        /// </summary>
        /// <param name="packet">
        /// The source packet buffer.
        /// </param>
        /// <param name="errorOffset">
        /// The offset in <paramref name="packet" /> where the error was found.
        /// </param>
        /// <param name="stored">
        /// The checksum value that is stored in the packet.
        /// </param>
        /// <param name="calculated">
        /// The calculated checksum value.
        /// </param>
        public ChecksumMismatchException([NotNull] byte[] packet, int errorOffset, int stored, int calculated)
            : base(packet, errorOffset, FormatMessage(stored, calculated))
        {
            Stored = stored;
            Calculated = calculated;
        }

        [NotNull]
        private static string FormatMessage(int stored, int calculated)
        {
            return $"Invalid checksum (stored=0x{stored:X2}, calculated=0x{calculated:X2}).";
        }

        private ChecksumMismatchException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Stored = info.GetInt32("Stored");
            Calculated = info.GetInt32("Calculated");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));

            info.AddValue("Stored", Stored);
            info.AddValue("Calculated", Calculated);

            base.GetObjectData(info, context);
        }
    }
}