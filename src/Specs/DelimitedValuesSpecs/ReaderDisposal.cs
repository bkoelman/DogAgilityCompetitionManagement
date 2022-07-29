using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using Xunit;

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs;

/// <summary>
/// Tests for lifetime management in <see cref="DelimitedValuesReader" />.
/// </summary>
public sealed class ReaderDisposal
{
    [Fact]
    public void When_accessing_line_number_after_disposal_it_should_fail()
    {
        // Arrange
        DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
        reader.Dispose();

        // Act
        Action action = () => _ = reader.LineNumber;

        // Assert
        action.Should().ThrowExactly<ObjectDisposedException>();
    }

    [Fact]
    public void When_accessing_line_after_disposal_it_should_fail()
    {
        // Arrange
        DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
        reader.Dispose();

        // Act
        Action action = () => _ = reader.Line;

        // Assert
        action.Should().ThrowExactly<ObjectDisposedException>();
    }

    [Fact]
    public void When_getting_enumerator_after_disposal_it_should_fail()
    {
        // Arrange
        DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
        reader.Dispose();

        // Act
        Action action = () => _ = reader.GetEnumerator();

        // Assert
        action.Should().ThrowExactly<ObjectDisposedException>();
    }

    [Fact]
    public void When_enumerating_after_disposal_it_should_fail()
    {
        // Arrange
        DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
        reader.Dispose();

        // Act
        Action action = () => _ = reader.TakeWhile(_ => true).ToArray();

        // Assert
        action.Should().ThrowExactly<ObjectDisposedException>();
    }
}
