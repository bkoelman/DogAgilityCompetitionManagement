using System;
using System.Globalization;
using System.Linq;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs
{
    /// <summary>
    /// Tests for correct position reporting in <see cref="DelimitedValuesReader" />.
    /// </summary>
    [TestFixture]
    public sealed class ReaderPositioning
    {
        [NotNull]
        private static readonly string DefaultTextQualifier =
            new DelimitedValuesReaderSettings().TextQualifier.ToString(CultureInfo.InvariantCulture);

        [Test]
        public void When_only_header_has_been_read_it_should_be_positioned_at_first_line()
        {
            // Arrange
            Func<DelimitedValuesReader> construction = () => new DelimitedValuesReaderBuilder().Build();

            // Act
            DelimitedValuesReader reader = construction();

            // Assert
            reader.LineNumber.Should().Be(1);
        }

        [Test]
        public void When_header_and_first_line_have_been_read_it_should_be_positioned_at_second_line()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();

            // Act
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            reader.First();

            // Assert
            reader.LineNumber.Should().Be(2);
        }

        [Test]
        public void When_lines_are_broken_using_carriage_returns_it_should_report_the_correct_starting_line_number()
        {
            // Arrange
            const string lineBreaker = "\r";
            const string cellValueWithLineBreak = "Cell with" + lineBreaker + "line break";
            string rowWithLineBreak = DefaultTextQualifier + cellValueWithLineBreak + DefaultTextQualifier;

            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithSingleColumnHeader()
                .WithoutRows()
                .WithDataLine(rowWithLineBreak)
                .WithDataLine(rowWithLineBreak)
                .Build();

            // Act
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            reader.Take(2).ToArray();

            // Assert
            reader.LineNumber.Should().Be(4);
        }

        [Test]
        public void When_lines_are_broken_using_line_feeds_it_should_report_the_correct_starting_line_number()
        {
            // Arrange
            const string lineBreaker = "\n";
            const string cellValueWithLineBreak = "Cell with" + lineBreaker + "line break";
            string rowWithLineBreak = DefaultTextQualifier + cellValueWithLineBreak + DefaultTextQualifier;

            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithSingleColumnHeader()
                .WithoutRows()
                .WithDataLine(rowWithLineBreak)
                .WithDataLine(rowWithLineBreak)
                .Build();

            // Act
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            reader.Take(2).ToArray();

            // Assert
            reader.LineNumber.Should().Be(4);
        }

        [Test]
        public void
            When_lines_are_broken_using_carriage_returns_followed_by_with_line_feeds_it_should_report_the_correct_starting_line_number()
        {
            // Arrange
            const string lineBreaker = "\r\n";
            const string cellValueWithLineBreak = "Cell with" + lineBreaker + "line break";
            string rowWithLineBreak = DefaultTextQualifier + cellValueWithLineBreak + DefaultTextQualifier;

            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithSingleColumnHeader()
                .WithoutRows()
                .WithDataLine(rowWithLineBreak)
                .WithDataLine(rowWithLineBreak)
                .Build();

            // Act
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            reader.Take(2).ToArray();

            // Assert
            reader.LineNumber.Should().Be(4);
        }

        [Test]
        public void When_source_contains_uneven_number_of_quotes_it_should_report_the_correct_starting_line_number()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithSingleColumnHeader()
                .WithoutRows()
                .WithDataLine(DefaultTextQualifier + "12345")
                .WithDataLine("A")
                .WithDataLine("B")
                .Build();

            try
            {
                // Act
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                reader.SkipWhile(_ => true).ToArray();
            }
            catch (DelimitedValuesParseException)
            {
                // Assert
                reader.LineNumber.Should().Be(2);
            }
        }
    }
}
