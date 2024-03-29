﻿using System.Globalization;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using Xunit;

// @formatter:keep_existing_linebreaks true

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs;

/// <summary>
/// Tests for type conversions in <see cref="DelimitedValuesReader" />.
/// </summary>
public sealed class ReaderConversions
{
    [Fact]
    public void When_reading_cell_value_as_nullable_boolean_it_should_succeed()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithColumnHeaders("A", "B", "C")
            .WithoutRows()
            .WithRow(new[]
            {
                "True",
                "False",
                ""
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.First();
        bool? cell1 = row.GetCell<bool?>("A");
        bool? cell2 = row.GetCell<bool?>("B");
        bool? cell3 = row.GetCell<bool?>("C");

        // Assert
        cell1.Should().BeTrue();
        cell2.Should().BeFalse();
        cell3.Should().Be(null);
    }

    [Fact]
    public void When_reading_cell_value_it_should_respect_culture()
    {
        // Arrange
        var culture = new CultureInfo("nl-NL");

        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSettings(new DelimitedValuesReaderSettingsBuilder()
                .WithCulture(culture))
            .WithSingleColumnHeader("A")
            .WithoutRows()
            .WithRow(new[]
            {
                "3,5"
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.First();
        decimal cell = row.GetCell<decimal>("A");

        // Assert
        cell.Should().Be(3.5m);
    }

    [Fact]
    public void When_reading_cell_value_with_custom_converter_it_should_succeed()
    {
        // Arrange
        using DelimitedValuesReader reader = new DelimitedValuesReaderBuilder()
            .WithSingleColumnHeader("A")
            .WithoutRows()
            .WithRow(new[]
            {
                "X"
            })
            .Build();

        // Act
        IDelimitedValuesReaderRow row = reader.First();
        string? cell = row.GetCell("A", _ => "Y");

        // Assert
        cell.Should().Be("Y");
    }
}
