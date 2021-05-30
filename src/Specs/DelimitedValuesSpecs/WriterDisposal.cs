using System;
using System.Collections.Generic;
using System.IO;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using NUnit.Framework;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs
{
    /// <summary>
    /// Tests for lifetime management in <see cref="DelimitedValuesWriter" />.
    /// </summary>
    [TestFixture]
    public sealed class WriterDisposal
    {
        [Test]
        public void When_creating_row_after_disposal_it_should_fail()
        {
            // Arrange
            DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder().Build();
            writer.Dispose();

            // Act
            Action action = () =>
            {
                // ReSharper disable once UnusedVariable
                IDelimitedValuesWriterRow dummy = writer.CreateRow();
            };

            // Assert
            action.Should().Throw<ObjectDisposedException>();
        }

        [Test]
        public void When_completing_row_creation_after_disposal_it_should_fail()
        {
            // Arrange
            DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder().Build();
            IDelimitedValuesWriterRow row = writer.CreateRow();
            writer.Dispose();

            // Act
            Action action = row.Dispose;

            // Assert
            action.Should().Throw<ObjectDisposedException>();
        }

        [Test]
        public void When_disposing_row_multiple_times_it_should_write_the_row_only_once()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithoutColumnNamesOnFirstLine())
                .WithSingleColumnHeader("A")
                .Build())
            {
                IDelimitedValuesWriterRow row = writer.CreateRow();
                row.SetCell("A", "X");

                // Act
                row.Dispose();
                row.Dispose();
            }

            IEnumerable<string> lines = TextToLines(output.ToString());

            // Assert
            lines.Should().HaveCount(1);
        }

        private static IEnumerable<string> TextToLines(string text)
        {
            var lines = new List<string>();

            using var reader = new StringReader(text);
            string? nextLine;

            while ((nextLine = reader.ReadLine()) != null)
            {
                lines.Add(nextLine);
            }

            return lines;
        }

        [Test]
        public void When_not_closing_underlying_reader_it_should_flush_on_disposal()
        {
            // Arrange
            var outputStream = new MemoryStream();

            DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(new StreamWriter(new BufferedStream(outputStream)))
                .WithSingleColumnHeader("A")
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .KeepingReaderOpen())
                .Build();

            // Act
            writer.Dispose();

            // Assert
            outputStream.Length.Should().BeGreaterThan(0);
        }
    }
}
