namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Defines delimiter characters used in a CIRCE packet.
    /// </summary>
    public static class PacketFormatDelimiters
    {
        /// <summary>
        /// Indicates the start of the packet.
        /// </summary>
        public const byte StartOfText = 0x02;

        /// <summary>
        /// Indicates the end of the packet.
        /// </summary>
        public const byte EndOfText = 0x03;

        /// <summary>
        /// Indicates the TAB character, which is used to separate packet header, parameters and trailer.
        /// </summary>
        public const byte Tab = 0x09;

        /// <summary>
        /// Indicates the COLON character, which is used to separate the Id and the value of a parameter.
        /// </summary>
        public const byte Colon = 0x3A;
    }
}
