using System.Globalization;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using Xunit;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs;

/// <summary>
/// Tests for cell data parsing in <see cref="DelimitedValuesReader" />.
/// </summary>
public sealed class ReaderParsing
{
    private static readonly string DefaultTextQualifier = new DelimitedValuesReaderSettings().TextQualifier.ToString(CultureInfo.InvariantCulture);

    [Fact]
    public void When_source_is_empty_it_should_fail()
    {
        // Arrange
        var source = new StringReader("");

        // Act
        Action action = () => _ = new DelimitedValuesReader(source);

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>().WithMessage("Missing column names on first line.");
    }

    [Fact]
    public void When_source_contains_single_line_break_it_should_fail()
    {
        // Arrange
        var source = new StringReader("\n");

        // Act
        Action action = () => _ = new DelimitedValuesReader(source);

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>().WithMessage("Missing column names on first line.");
    }

    [Fact]
    public void When_source_contains_only_line_breaks_it_should_fail()
    {
        // Arrange
        var source = new StringReader("\n\n");

        // Act
        Action action = () => _ = new DelimitedValuesReader(source);

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>().WithMessage("Source contains no columns.");
    }

    [Fact]
    public void When_source_contains_multiple_column_names_they_must_be_exposed()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithColumnHeaders("C1", "C2", "C3")
            .Build();

        // Act
        string[] columnNames = reader.ColumnNames.ToArray();

        // Assert
        columnNames.Should().HaveCount(3);
        columnNames[0].Should().Be("C1");
        columnNames[1].Should().Be("C2");
        columnNames[2].Should().Be("C3");
    }

    [Fact]
    public void When_source_contains_duplicate_column_names_it_should_fail()
    {
        // Act
        Action action = () => new DelimitedValuesReaderBuilder()
            .WithHeaderLine("A,A")
            .Build();

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>().WithMessage("Column 'A' occurs multiple times.");
    }

    [Fact]
    public void When_no_field_separator_is_specified_it_must_autodetect_tab_as_separator()
    {
        // Act
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(null))
            .WithHeaderLine("A\tB\tC")
            .WithoutRows()
            .Build();

        // Assert
        reader.ColumnNames.Should().HaveCount(3);
    }

    [Fact]
    public void When_no_field_separator_is_specified_it_must_autodetect_semicolon_as_separator()
    {
        // Act
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(null))
            .WithHeaderLine("A;B;C")
            .WithoutRows()
            .Build();

        // Assert
        reader.ColumnNames.Should().HaveCount(3);
    }

    [Fact]
    public void When_no_field_separator_is_specified_it_must_autodetect_comma_as_separator()
    {
        // Act
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(null))
            .WithHeaderLine("A,B,C")
            .WithoutRows()
            .Build();

        // Assert
        reader.ColumnNames.Should().HaveCount(3);
    }

    [Fact]
    public void When_no_field_separator_is_specified_it_must_autodetect_colon_as_separator()
    {
        // Act
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(null))
            .WithHeaderLine("A:B:C")
            .WithoutRows()
            .Build();

        // Assert
        reader.ColumnNames.Should().HaveCount(3);
    }

    [Fact]
    public void When_no_field_separator_is_specified_it_must_autodetect_pipe_as_separator()
    {
        // Act
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(null))
            .WithHeaderLine("A|B|C")
            .WithoutRows()
            .Build();

        // Assert
        reader.ColumnNames.Should().HaveCount(3);
    }

    [Fact]
    public void When_no_field_separator_is_specified_it_must_give_semicolon_precedence_over_comma_during_auto_detection()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(null))
            .WithHeaderLine("A;B;C,D,E")
            .WithoutRows()
            .WithDataLine("1;2;345")
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();

        // Assert
        reader.ColumnNames.Should().HaveCount(3);
        row.GetCell("A").Should().Be("1");
    }

    [Fact]
    public void When_cell_contains_unquoted_text_before_quoted_text_it_should_fail()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader()
            .WithoutRows()
            .WithDataLine("a" + DefaultTextQualifier + "b" + DefaultTextQualifier)
            .Build();

        // Act
        // ReSharper disable once AccessToDisposedClosure
        Action action = () => _ = reader.Take(1).ToArray();

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>().WithMessage("Text qualifier must be the first non-whitespace character of a cell.");
    }

    [Fact]
    public void When_cell_contains_unquoted_text_after_quoted_text_it_should_fail()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader()
            .WithoutRows()
            .WithDataLine(DefaultTextQualifier + "a" + DefaultTextQualifier + "b")
            .Build();

        // Act
        // ReSharper disable once AccessToDisposedClosure
        Action action = () => _ = reader.Take(1).ToArray();

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>()
            .WithMessage("Text-qualified cell cannot contain non-whitespace after the closing text qualifier.");
    }

    [Fact]
    public void When_quoted_cell_contains_like_breaks_they_must_be_preserved()
    {
        // Arrange
        const string cellValue = "a\rb\nc\r\nd";

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader("C")
            .WithoutRows()
            .WithDataLine(DefaultTextQualifier + cellValue + DefaultTextQualifier)
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();
        string cell = row.GetCell("C");

        // Assert
        cell.Should().Be(cellValue);
    }

    [Fact]
    public void When_quoted_cell_contains_field_separators_they_must_be_preserved()
    {
        // Arrange
        const char separator = ':';
        const string cellValue = "x:y:z";

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithFieldSeparator(separator))
            .WithHeaderLine("A" + separator + "B")
            .WithoutRows()
            .WithDataLine(DefaultTextQualifier + cellValue + DefaultTextQualifier + separator)
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();
        string cell = row.GetCell("A");

        // Assert
        cell.Should().Be(cellValue);
    }

    [Fact]
    public void When_quoted_cell_contains_text_qualifiers_they_must_be_unescaped()
    {
        // Arrange
        const string cellValue = "A \"nice\" day...";

        string escaped = DefaultTextQualifier +
            cellValue.Replace(DefaultTextQualifier, DefaultTextQualifier + DefaultTextQualifier, StringComparison.Ordinal) + DefaultTextQualifier;

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader("C")
            .WithoutRows()
            .WithDataLine(escaped)
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();
        string cell = row.GetCell("C");

        // Assert
        cell.Should().Be(cellValue);
    }

    [Fact]
    public void When_quoted_cell_surrounds_text_qualifiers_they_must_be_unescaped()
    {
        // Arrange
        const string cellValue = "\"A nice day...\"";

        string escaped = DefaultTextQualifier +
            cellValue.Replace(DefaultTextQualifier, DefaultTextQualifier + DefaultTextQualifier, StringComparison.Ordinal) + DefaultTextQualifier;

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader("C")
            .WithoutRows()
            .WithDataLine(escaped)
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();
        string cell = row.GetCell("C");

        // Assert
        cell.Should().Be(cellValue);
    }

    [Fact]
    public void When_cells_are_empty_they_must_be_exposed_as_empty()
    {
        // Arrange
        string emptyCellValue = string.Empty;

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithColumnHeaders("A", "B", "C")
            .WithoutRows()
            .WithRow(new[]
            {
                emptyCellValue,
                emptyCellValue,
                emptyCellValue
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();
        string cell1 = row.GetCell("A");
        string cell2 = row.GetCell("B");
        string cell3 = row.GetCell("C");

        // Assert
        cell1.Should().Be(emptyCellValue);
        cell2.Should().Be(emptyCellValue);
        cell3.Should().Be(emptyCellValue);
    }

    [Fact]
    public void When_source_contains_uneven_number_of_quotes_it_should_fail()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader()
            .WithoutRows()
            .WithDataLine(DefaultTextQualifier)
            .Build();

        // Act
        // ReSharper disable once AccessToDisposedClosure
        Action action = () => _ = reader.SkipWhile(_ => true).ToArray();

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>().WithMessage("Missing closing text qualifier.");
    }

    [Fact]
    public void When_unquoted_cell_contains_leading_whitespace_it_must_be_discarded()
    {
        // Arrange
        const string columnHeaderName = "C";

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader(columnHeaderName)
            .WithoutRows()
            .WithRow(new[]
            {
                " A"
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();

        // Assert
        row.GetCell(columnHeaderName).Should().Be("A");
    }

    [Fact]
    public void When_unquoted_cell_contains_trailing_whitespace_it_must_be_discarded()
    {
        // Arrange
        const string columnHeaderName = "C";

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader(columnHeaderName)
            .WithoutRows()
            .WithRow(new[]
            {
                "A "
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();

        // Assert
        row.GetCell(columnHeaderName).Should().Be("A");
    }

    [Fact]
    public void When_quoted_cell_contains_leading_and_trailing_whitespace_it_must_be_preserved()
    {
        // Arrange
        const string columnHeaderName = "C";

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader(columnHeaderName)
            .WithoutRows()
            .WithRow(new[]
            {
                " \"  A \"  "
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.Single();

        // Assert
        row.GetCell(columnHeaderName).Should().Be("  A ");
    }

    [Fact]
    public void When_source_contains_uneven_number_of_quotes_it_should_not_read_entire_source()
    {
        // Arrange
        const string header = "C1";
        string cell = DefaultTextQualifier + "12345";
        int bufferSize = header.Length + Environment.NewLine.Length + cell.Length;

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithIntermediateReader(r => new ReaderThatFailsAfterReadingTooMuch(r, bufferSize))
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithMaximumLineLength(cell.Length))
            .WithHeaderLine(header)
            .WithoutRows()
            .WithDataLine(cell)
            .WithDataLine("A")
            .WithDataLine("B")
            .Build();

        // Act
        // ReSharper disable once AccessToDisposedClosure
        Action action = () => _ = reader.SkipWhile(_ => true).ToArray();

        // Assert
        action.Should().ThrowExactly<DelimitedValuesParseException>();
    }

    private sealed class ReaderThatFailsAfterReadingTooMuch : TextReader
    {
        private readonly TextReader source;
        private readonly int maximumNumberOfCharacters;

        private int position;

        public ReaderThatFailsAfterReadingTooMuch(TextReader source, int maximumNumberOfCharacters)
        {
            Guard.NotNull(source, nameof(source));

            this.source = source;
            this.maximumNumberOfCharacters = maximumNumberOfCharacters;
        }

        public override int Peek()
        {
            return source.Peek();
        }

        public override int Read()
        {
            int result = Peek();

            if (result != -1)
            {
                source.Read();
                position++;
            }

            if (position > maximumNumberOfCharacters)
            {
                throw new IOException($"Attempt to read more than {maximumNumberOfCharacters} characters.");
            }

            return result;
        }
    }
}
