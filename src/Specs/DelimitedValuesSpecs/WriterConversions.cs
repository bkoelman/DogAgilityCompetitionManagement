using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs
{
    /// <summary>
    /// Tests for type conversions in <see cref="DelimitedValuesWriter" />.
    /// </summary>
    [TestFixture]
    public sealed class WriterConversions
    {
        [Test]
        public void When_writing_cell_value_as_nullable_boolean_it_should_succeed()
        {
            // Arrange
            var output = new StringWriter();
            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithColumnHeaders("A", "B", "C")
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithoutColumnNamesOnFirstLine())
                .Build())
            {
                // Act
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell<bool?>("A", true);
                    row.SetCell<bool?>("B", null);
                    row.SetCell<bool?>("C", false);
                }
            }

            // Assert
            output.ToString().Should().Be("True,,False" + Environment.NewLine);
        }

        [Test]
        public void When_writing_cell_value_it_should_respect_culture()
        {
            // Arrange
            var output = new StringWriter();
            var culture = new CultureInfo("nl-NL");
            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSingleColumnHeader("A")
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithoutColumnNamesOnFirstLine()
                    .WithCulture(culture))
                .Build())
            {
                // Act
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    string firstColumnName = writer.ColumnNames.First();
                    row.SetCell(firstColumnName, 3.5m);
                }
            }

            // Assert
            output.ToString().Should().Be("3,5" + Environment.NewLine);
        }

        [Test]
        public void When_writing_cell_value_with_custom_converter_it_should_succeed()
        {
            // Arrange
            var output = new StringWriter();
            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSingleColumnHeader("A")
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithoutColumnNamesOnFirstLine())
                .Build())
            {
                // Act
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", 123, c => "Y");
                }
            }

            // Assert
            output.ToString().Should().Be("Y" + Environment.NewLine);
        }
    }
}