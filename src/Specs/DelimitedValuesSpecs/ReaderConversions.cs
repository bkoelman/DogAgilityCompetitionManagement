using System.Globalization;
using System.Linq;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs
{
    /// <summary>
    /// Tests for type conversions in <see cref="DelimitedValuesReader" />.
    /// </summary>
    [TestClass]
    public sealed class ReaderConversions
    {
        [TestMethod]
        public void When_reading_cell_value_as_nullable_boolean_it_should_succeed()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithColumnHeaders("A", "B", "C")
                .WithoutRows()
                .WithRow(new[] { "True", "False", "" })
                .Build();

            // Act
            IDelimitedValuesReaderRow row = reader.First();
            var cell1 = row.GetCell<bool?>("A");
            var cell2 = row.GetCell<bool?>("B");
            var cell3 = row.GetCell<bool?>("C");

            // Assert
            cell1.Should().BeTrue();
            cell2.Should().BeFalse();
            cell3.Should().Be(null);
        }

        [TestMethod]
        public void When_reading_cell_value_it_should_respect_culture()
        {
            // Arrange
            var culture = new CultureInfo("nl-NL");
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                    .WithCulture(culture))
                .WithSingleColumnHeader("A")
                .WithoutRows()
                .WithRow(new[] { "3,5" })
                .Build();

            // Act
            IDelimitedValuesReaderRow row = reader.First();
            decimal cell = row.GetCell<decimal>("A");

            // Assert
            cell.Should().Be(3.5m);
        }

        [TestMethod]
        public void When_reading_cell_value_with_custom_converter_it_should_succeed()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
                .WithSingleColumnHeader("A")
                .WithoutRows()
                .WithRow(new[] { "X" })
                .Build();

            // Act
            IDelimitedValuesReaderRow row = reader.First();
            string cell = row.GetCell("A", c => "Y");

            // Assert
            cell.Should().Be("Y");
        }
    }
}