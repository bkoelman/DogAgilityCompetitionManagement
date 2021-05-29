using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using DogAgilityCompetition.Circe.Protocol.Exceptions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Reads a block of bytes and produces a CIRCE operation from it.
    /// </summary>
    public sealed class PacketReader
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [CanBeNull]
        private ISystemLogger customLogger;

        [NotNull]
        public ISystemLogger ActiveLogger
        {
            get => customLogger ?? Log;
            set
            {
                Guard.NotNull(value, nameof(value));
                customLogger = value;
            }
        }

        /// <summary>
        /// Converts the specified binary packet to an <see cref="Operation" />.
        /// </summary>
        /// <param name="buffer">
        /// The binary packet.
        /// </param>
        /// <returns>
        /// The operation constructed from the packet.
        /// </returns>
        /// <exception cref="PacketFormatException" />
        /// <exception cref="ChecksumMismatchException" />
        /// <exception cref="UnknownOperationException" />
        /// <exception cref="ParameterValueFormatException" />
        /// <exception cref="OperationValidationException" />
        [NotNull]
        public Operation Read([NotNull] byte[] buffer)
        {
            Guard.NotNull(buffer, nameof(buffer));

            var scanner = new PacketBufferScanner(buffer);

            int operationCode = ParseHeader(scanner.HeaderContext);
            ParseTrailer(scanner.TrailerContext, scanner.HasChecksum);

            Operation operation;

            try
            {
                operation = OperationFactory.Create(operationCode);
            }
            catch (NotSupportedException)
            {
                throw new UnknownOperationException(buffer, operationCode);
            }

            ParsePayload(scanner.PayloadContext, operation);

            operation.Validate();
            return operation;
        }

        private static int ParseHeader([NotNull] PacketParseContext context)
        {
            context.ConsumeByte(PacketFormatDelimiters.StartOfText);
            int operationCode = context.ConsumePositiveNumber(2);
            context.ConsumeByte(PacketFormatDelimiters.Tab);

            return operationCode;
        }

        private static void ParseTrailer([NotNull] PacketParseContext context, bool hasChecksum)
        {
            if (hasChecksum)
            {
                context.Consume();
                context.Consume();
            }

            context.ConsumeByte(PacketFormatDelimiters.EndOfText);
        }

        private void ParsePayload([NotNull] PacketParseContext context, [NotNull] Operation operation)
        {
            var seenParameters = new HashSet<int>();

            while (context.IsPositionBeforeEnd)
            {
                int parameterStartOffset = context.Position;
                int parameterId = context.ConsumePositiveNumber(3);
                context.ConsumeByte(PacketFormatDelimiters.Colon);

                int valueStartOffset = context.Position;
                var parameterValueBytes = new List<byte>();

                byte nextByte;

                while ((nextByte = context.Consume()) != PacketFormatDelimiters.Tab)
                {
                    parameterValueBytes.Add(nextByte);
                }

                if (seenParameters.Contains(parameterId) && !operation.AllowMultiple(parameterId))
                {
                    int displayPosition = parameterStartOffset + 1;

                    ActiveLogger.Warn($"Warning at packet position {displayPosition}: " +
                        $"Ignoring additional occurrence of parameter {parameterId} in packet " +
                        $"for operation {operation.Code}.{context.Buffer.Array.FormatHexBuffer(4)}");
                }
                else
                {
                    try
                    {
                        Parameter parameter = operation.GetParameterOrNull(parameterId);

                        if (parameter != null)
                        {
                            parameter.ImportValue(parameterValueBytes.ToArray());
                            seenParameters.Add(parameterId);
                        }
                        else
                        {
                            int displayPosition = parameterStartOffset + 1;

                            ActiveLogger.Warn($"Warning at packet position {displayPosition}: " +
                                $"Ignoring unexpected occurrence of parameter {parameterId} in packet " +
                                $"for operation {operation.Code}.{context.Buffer.Array.FormatHexBuffer(4)}");
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ParameterValueFormatException(context.Buffer.Array, valueStartOffset, ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Performs a pre-scan of a packet buffer. Verifies its checksum and breaks the packet into header, payload and trailer segments.
        /// </summary>
        private sealed class PacketBufferScanner
        {
            /// <summary>
            /// The buffer range that contains the header of the packet.
            /// </summary>
            [NotNull]
            public PacketParseContext HeaderContext { get; }

            /// <summary>
            /// The buffer range that contains the payload (parameters) of the packet.
            /// </summary>
            [NotNull]
            public PacketParseContext PayloadContext { get; }

            /// <summary>
            /// The buffer range that contains the trailer of the packet.
            /// </summary>
            [NotNull]
            public PacketParseContext TrailerContext { get; }

            /// <summary>
            /// Gets whether this packet contains a checksum.
            /// </summary>
            public bool HasChecksum { get; }

            public PacketBufferScanner([NotNull] byte[] buffer)
            {
                if (buffer.Length < PacketFormat.PacketHeaderLength + PacketFormat.PacketTrailerMinLength)
                {
                    throw new PacketFormatException(buffer, buffer.Length, "Insufficient bytes to form a valid packet.");
                }

                int lastTabPosition = buffer.Length - 1 - PacketFormat.PacketTrailerMinLength;
                HasChecksum = buffer[lastTabPosition] != PacketFormatDelimiters.Tab;

                if (HasChecksum)
                {
                    const int packetLength = PacketFormat.PacketHeaderLength + PacketFormat.OptionalChecksumLength + PacketFormat.PacketTrailerMinLength;

                    if (buffer.Length < packetLength)
                    {
                        throw new PacketFormatException(buffer, lastTabPosition, "Insufficient bytes to form a valid packet with checksum.");
                    }

                    VerifyChecksum(buffer);
                }

                HeaderContext = new PacketParseContext(buffer, 0, PacketFormat.PacketHeaderLength);

                int trailerLength = HasChecksum
                    ? PacketFormat.PacketTrailerMinLength + PacketFormat.OptionalChecksumLength
                    : PacketFormat.PacketTrailerMinLength;

                int trailerStartPosition = HasChecksum
                    ? buffer.Length - PacketFormat.PacketTrailerMinLength - PacketFormat.OptionalChecksumLength
                    : buffer.Length - PacketFormat.PacketTrailerMinLength;

                TrailerContext = new PacketParseContext(buffer, trailerStartPosition, trailerLength);

                int payloadSize = buffer.Length - PacketFormat.PacketHeaderLength - trailerLength;
                PayloadContext = new PacketParseContext(buffer, PacketFormat.PacketHeaderLength, payloadSize);
            }

            private static void VerifyChecksum([NotNull] byte[] buffer)
            {
                int storedChecksum = ExtractChecksum(buffer);
                int calculatedChecksum = CalculateChecksum(buffer);

                if (storedChecksum != calculatedChecksum)
                {
                    int checksumOffset = buffer.Length - PacketFormat.ChecksumOffsetFromEndOfPacket;
                    throw new ChecksumMismatchException(buffer, checksumOffset, storedChecksum, calculatedChecksum);
                }
            }

            private static int ExtractChecksum([NotNull] byte[] buffer)
            {
                int checksumOffset = buffer.Length - PacketFormat.ChecksumOffsetFromEndOfPacket;

                byte[] checksumBytes =
                {
                    buffer[checksumOffset],
                    buffer[checksumOffset + 1]
                };

                char[] chars = Encoding.ASCII.GetChars(checksumBytes);
                string checksumHex = new(chars);

                if (!int.TryParse(checksumHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int checksumValue))
                {
                    throw new PacketFormatException(buffer, checksumOffset, "Invalid encoding of checksum value.");
                }

                return checksumValue;
            }

            private static int CalculateChecksum([NotNull] byte[] buffer)
            {
                int checksumOffset = buffer.Length - PacketFormat.ChecksumOffsetFromEndOfPacket;

                int calculatedChecksum = 0;

                for (int index = 0; index < checksumOffset; index++)
                {
                    calculatedChecksum += buffer[index];
                    calculatedChecksum &= 0xFF;
                }

                return calculatedChecksum;
            }
        }

        /// <summary>
        /// Represents a subset of the bytes from a complete packet, along with utility methods.
        /// </summary>
        private sealed class PacketParseContext
        {
            /// <summary>
            /// Gets the bytes of the packet. The segment indicates which subrange is being parsed.
            /// </summary>
            public ArraySegment<byte> Buffer { get; }

            /// <summary>
            /// Gets the current position in <see cref="Buffer" />.
            /// </summary>
            public int Position { get; private set; }

            /// <summary>
            /// Gets a value indicating whether the internal cursor is positioned before the end of the segment being parsed.
            /// </summary>
            /// <value>
            /// <c>true</c> if another byte can be consumed; otherwise, <c>false</c>.
            /// </value>
            public bool IsPositionBeforeEnd => Position < Buffer.Offset + Buffer.Count;

            /// <summary>
            /// Initializes a new instance of the <see cref="PacketParseContext" /> class.
            /// </summary>
            /// <param name="buffer">
            /// The bytes of a complete packet.
            /// </param>
            /// <param name="offset">
            /// The offset at which to start parsing.
            /// </param>
            /// <param name="count">
            /// The number of bytes to parse.
            /// </param>
            public PacketParseContext([NotNull] byte[] buffer, int offset, int count)
            {
                Buffer = new ArraySegment<byte>(buffer, offset, count);
                Position = offset;
            }

            /// <summary>
            /// Consumes a positive number from the buffer and advances the position accordingly.
            /// </summary>
            /// <param name="digitCount">
            /// The number of digits to consume.
            /// </param>
            /// <returns>
            /// The numeric value of the parsed digits.
            /// </returns>
            public int ConsumePositiveNumber(int digitCount)
            {
                byte[] digits = new byte[digitCount];

                for (int index = 0; index < digitCount; index++)
                {
                    digits[index] = ConsumeDigit();
                }

                char[] chars = Encoding.ASCII.GetChars(digits);
                string numberString = new(chars);
                return int.Parse(numberString);
            }

            private byte ConsumeDigit()
            {
                return Consume(actual =>
                {
                    if (actual < '0' || actual > '9')
                    {
                        throw new PacketFormatException(Buffer.Array, Position, "Expected digit.");
                    }
                });
            }

            /// <summary>
            /// Consumes a single byte from the buffer and advances the position accordingly.
            /// </summary>
            /// <param name="expected">
            /// The expected byte value.
            /// </param>
            /// <returns>
            /// The consumed byte.
            /// </returns>
            public void ConsumeByte(byte expected)
            {
                Consume(actual =>
                {
                    if (actual != expected)
                    {
                        throw new PacketFormatException(Buffer.Array, Position, $"Expected 0x{expected:X2} instead of 0x{actual:X2}.");
                    }
                });
            }

            /// <summary>
            /// Consumes a single byte from the buffer and advances the position accordingly.
            /// </summary>
            /// <param name="validateValueCallback">
            /// An optional callback to validate the byte being consumed.
            /// </param>
            /// <returns>
            /// The consumed byte.
            /// </returns>
            public byte Consume([CanBeNull] Action<byte> validateValueCallback = null)
            {
                AssertOffsetNotAtEnd();
                byte actual = Buffer.Array[Position];

                validateValueCallback?.Invoke(actual);

                Position++;
                return actual;
            }

            [AssertionMethod]
            private void AssertOffsetNotAtEnd()
            {
                if (!IsPositionBeforeEnd)
                {
                    throw new PacketFormatException(Buffer.Array, Position, "Unexpected end of packet.");
                }
            }
        }
    }
}
