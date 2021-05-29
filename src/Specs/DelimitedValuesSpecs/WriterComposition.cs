using System;
using System.Globalization;
using System.IO;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using NUnit.Framework;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs
{
    /// <summary>
    /// Tests for validation and escaping cells in <see cref="DelimitedValuesWriter" />.
    /// </summary>
    [TestFixture]
    public sealed class WriterComposition
    {
        [Test]
        public void When_no_rows_are_written_it_should_succeed()
        {
            // Act
            var output = new StringWriter();

            using (new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSingleColumnHeader("A")
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithColumnNamesOnFirstLine())
                .Build())
            {
            }

            // Assert
            output.ToString().Should().Be("A" + Environment.NewLine);
        }

        [Test]
        public void When_no_header_and_no_rows_are_written_it_should_succeed()
        {
            // Act
            var output = new StringWriter();

            using (new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithoutColumnNamesOnFirstLine())
                .Build())
            {
            }

            // Assert
            output.ToString().Should().Be(string.Empty);
        }

        [Test]
        public void When_no_column_names_are_used_it_should_fail()
        {
            // Act
            Action action = () => new DelimitedValuesWriterBuilder()
                .WithColumnHeaders(null)
                .Build();

            // Assert
            action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("columnNames");
        }

        [Test]
        public void When_empty_list_of_column_names_is_used_it_should_fail()
        {
            // Act
            Action action = () => new DelimitedValuesWriterBuilder()
                .WithColumnHeaders()
                .Build();

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("List of column names cannot be empty.*");
        }

        [Test]
        public void When_empty_column_names_are_used_it_should_fail()
        {
            // Act
            Action action = () => new DelimitedValuesWriterBuilder()
                .WithSingleColumnHeader("")
                .Build();

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Column names cannot be null, empty or whitespace.*");
        }

        [Test]
        public void When_duplicate_column_names_are_used_it_should_fail()
        {
            // Act
            Action action = () => new DelimitedValuesWriterBuilder()
                .WithColumnHeaders("A", "A")
                .Build();

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Column 'A' occurs multiple times.*");
        }

        [Test]
        public void When_unspecified_column_names_are_assigned_a_cell_value_it_should_fail()
        {
            // Arrange
            DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WithSingleColumnHeader("A")
                .Build();

            // Act
            Action action = () =>
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("B", "dummy");
                }
            };

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("Column with name 'B' does not exist.*");
        }

        [Test]
        public void When_using_invariant_culture_it_should_use_comma_as_default_field_separator()
        {
            // Arrange
            var output = new StringWriter();

            using (new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithCulture(CultureInfo.InvariantCulture))
                .WithColumnHeaders("A", "B")
                .Build())
            {
                // Act
            }

            // Assert
            output.ToString().Should().Be("A,B" + Environment.NewLine);
        }

        [Test]
        public void When_culture_uses_comma_as_decimal_separator_it_should_use_semicolon_as_default_field_separator()
        {
            // Arrange
            var output = new StringWriter();
            var cultureWithCommaAsDecimalSeparator = new CultureInfo("nl-NL");

            using (new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithCulture(cultureWithCommaAsDecimalSeparator))
                .WithColumnHeaders("A", "B")
                .Build())
            {
                // Act
            }

            // Assert
            output.ToString().Should().Be("A;B" + Environment.NewLine);
        }

        [Test]
        public void When_cell_contains_leading_whitespace_it_should_escape_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithTextQualifier('\'')
                    .WithoutColumnNamesOnFirstLine())
                .WithSingleColumnHeader("A")
                .Build())
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", " X");
                }

                // Act
            }

            // Assert
            output.ToString().Should().Be("' X'" + Environment.NewLine);
        }

        [Test]
        public void When_cell_contains_trailing_whitespace_it_should_escape_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithTextQualifier('\'')
                    .WithoutColumnNamesOnFirstLine())
                .WithSingleColumnHeader("A")
                .Build())
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", "X ");
                }

                // Act
            }

            // Assert
            output.ToString().Should().Be("'X '" + Environment.NewLine);
        }

        [Test]
        public void When_cell_contains_carriage_return_it_should_escape_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithTextQualifier('\'')
                    .WithoutColumnNamesOnFirstLine())
                .WithSingleColumnHeader("A")
                .Build())
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", "X\r");
                }

                // Act
            }

            // Assert
            output.ToString().Should().Be("'X\r'" + Environment.NewLine);
        }

        [Test]
        public void When_cell_contains_line_feed_it_should_escape_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithTextQualifier('\'')
                    .WithoutColumnNamesOnFirstLine())
                .WithSingleColumnHeader("A")
                .Build())
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", "X\n");
                }

                // Act
            }

            // Assert
            output.ToString().Should().Be("'X\n'" + Environment.NewLine);
        }

        [Test]
        public void When_cell_contains_field_separator_it_should_escape_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithTextQualifier('\'')
                    .WithFieldSeparator('|')
                    .WithoutColumnNamesOnFirstLine())
                .WithColumnHeaders("A", "B")
                .Build())
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", "X|Y");
                    row.SetCell("B", "Z");
                }

                // Act
            }

            // Assert
            output.ToString().Should().Be("'X|Y'|Z" + Environment.NewLine);
        }

        [Test]
        public void When_cell_contains_text_qualifier_it_should_escape_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithTextQualifier('\'')
                    .WithoutColumnNamesOnFirstLine())
                .WithSingleColumnHeader("A")
                .Build())
            {
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", "Bed 'n Breakfast");
                }

                // Act
            }

            // Assert
            output.ToString().Should().Be("'Bed ''n Breakfast'" + Environment.NewLine);
        }

        [Test]
        public void When_cell_is_not_assigned_a_value_it_should_become_an_empty_cell()
        {
            // Arrange
            var output = new StringWriter();

            using (DelimitedValuesWriter writer = new DelimitedValuesWriterBuilder()
                .WritingTo(output)
                .WithSettings(new DelimitedValuesWriterSettingsBuilder()
                    .WithFieldSeparator('|')
                    .WithoutColumnNamesOnFirstLine())
                .WithColumnHeaders("A", "B", "C")
                .Build())
            {
                // Act
                using (IDelimitedValuesWriterRow row = writer.CreateRow())
                {
                    row.SetCell("A", "X");
                }
            }

            // Assert
            output.ToString().Should().Be("X||" + Environment.NewLine);
        }
    }
}
