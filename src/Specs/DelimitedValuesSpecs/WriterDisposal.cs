using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

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
            // ReSharper disable once UnusedVariable
            Action action = () => { IDelimitedValuesWriterRow dummy = writer.CreateRow(); };

            // Assert
            action.ShouldThrow<ObjectDisposedException>();
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
            action.ShouldThrow<ObjectDisposedException>();
        }

        [Test]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
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

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<string> TextToLines([NotNull] string text)
        {
            var lines = new List<string>();
            using (var reader = new StringReader(text))
            {
                string nextLine;
                while ((nextLine = reader.ReadLine()) != null)
                {
                    lines.Add(nextLine);
                }
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