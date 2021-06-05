using System;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Exceptions;
using DogAgilityCompetition.Specs.Facilities;
using FluentAssertions;
using NUnit.Framework;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.CirceSpecs
{
    /// <summary>
    /// Tests for parsing CIRCE binary packets.
    /// </summary>
    [TestFixture]
    public sealed class CircePacketBinaryFormat
    {
        [Test]
        public void When_parameter_format_is_invalid_it_must_throw()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                05,
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('6'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Action action = () => reader.Read(buffer);

            // Assert
            action.Should().ThrowExactly<ParameterValueFormatException>()
                .WithMessage("Error at position 9: Value of NetworkAddressParameter DestinationAddress must consist of 6 characters in range 0-9 or A-F.*");
        }

        [Test]
        public void When_packet_contains_unexpected_parameter_it_must_warn()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('6'),
                ByteFor('\t'),
                3
            };

            var logger = new FakeSystemLogger();

            var reader = new PacketReader
            {
                ActiveLogger = logger
            };

            // Act
            reader.Read(buffer);

            // Assert
            logger.WarningMessages.Should().HaveCount(1);

            logger.WarningMessages[0].Should().StartWith(
                "Warning at packet position 5: Ignoring unexpected occurrence of parameter 14 in packet for operation 1.");
        }

        [Test]
        public void When_packet_contains_duplicate_parameter_it_must_warn()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('3'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('6'),
                ByteFor('\t'),
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('4'),
                ByteFor(':'),
                ByteFor('1'),
                ByteFor('2'),
                ByteFor('3'),
                ByteFor('4'),
                ByteFor('5'),
                ByteFor('6'),
                ByteFor('\t'),
                3
            };

            var logger = new FakeSystemLogger();

            var reader = new PacketReader
            {
                ActiveLogger = logger
            };

            // Act
            reader.Read(buffer);

            // Assert
            logger.WarningMessages.Should().HaveCount(1);

            logger.WarningMessages[0].Should().StartWith(
                "Warning at packet position 16: Ignoring additional occurrence of parameter 14 in packet for operation 3.");
        }

        [Test]
        public void When_checksum_is_invalid_it_must_throw()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('0'),
                ByteFor('1'),
                ByteFor('\t'),
                ByteFor('6'),
                68,
                3
            };

            var reader = new PacketReader();

            // Act
            Action action = () => reader.Read(buffer);

            // Assert
            action.Should().ThrowExactly<ChecksumMismatchException>()
                .WithMessage("Error at position 5: Invalid checksum (stored=0x6D, calculated=0x6C).*");
        }

        [Test]
        public void When_operation_code_is_unknown_it_must_throw()
        {
            // Arrange
            byte[] buffer =
            {
                2,
                ByteFor('1'),
                ByteFor('1'),
                ByteFor('\t'),
                3
            };

            var reader = new PacketReader();

            // Act
            Action action = () => reader.Read(buffer);

            // Assert
            action.Should().ThrowExactly<UnknownOperationException>()
                .WithMessage("Error at position 1: Unsupported operation code 11.*");
        }

        private static byte ByteFor(char ch)
        {
            return (byte)ch;
        }
    }
}
