namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Defines sizes and offsets used in a CIRCE packet.
    /// </summary>
    public static class PacketFormat
    {
        /// <summary>
        /// Gets the fixed length (in bytes) of the packet header.
        /// </summary>
        public const int PacketHeaderLength = 4;

        /// <summary>
        /// Gets the minimum length (in bytes) of the packet trailer.
        /// </summary>
        public const int PacketTrailerMinLength = 1;

        /// <summary>
        /// Gets the length (in bytes) of the optional checksum in the packet trailer.
        /// </summary>
        public const int OptionalChecksumLength = 2;

        /// <summary>
        /// Gets the byte offset where the optional checksum starts, measured from the end of the packet.
        /// </summary>
        public const int ChecksumOffsetFromEndOfPacket = 3;
    }
}