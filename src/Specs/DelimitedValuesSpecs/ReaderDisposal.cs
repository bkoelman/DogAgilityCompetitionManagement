using System;
using System.Linq;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Specs.Builders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogAgilityCompetition.Specs.DelimitedValuesSpecs
{
    /// <summary>
    /// Tests for lifetime management in <see cref="DelimitedValuesReader" />.
    /// </summary>
    [TestClass]
    public sealed class ReaderDisposal
    {
        [TestMethod]
        public void When_accessing_line_number_after_disposal_it_should_fail()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
            reader.Dispose();

            // Act
            // ReSharper disable once UnusedVariable
            Action action = () => { int dummy = reader.LineNumber; };

            // Assert
            action.ShouldThrow<ObjectDisposedException>();
        }

        [TestMethod]
        public void When_accessing_line_after_disposal_it_should_fail()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
            reader.Dispose();

            // Act
            // ReSharper disable once UnusedVariable
            Action action = () => { string dummy = reader.Line; };

            // Assert
            action.ShouldThrow<ObjectDisposedException>();
        }

        [TestMethod]
        public void When_getting_enumerator_after_disposal_it_should_fail()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
            reader.Dispose();

            // Act
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once ConvertToLambdaExpression
            Action action = () => { reader.GetEnumerator(); };

            // Assert
            action.ShouldThrow<ObjectDisposedException>();
        }

        [TestMethod]
        public void When_enumerating_after_disposal_it_should_fail()
        {
            // Arrange
            DelimitedValuesReader reader = new DelimitedValuesReaderBuilder().Build();
            reader.Dispose();

            // Act
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once ConvertToLambdaExpression
            Action action = () => { reader.TakeWhile(x => true).ToArray(); };

            // Assert
            action.ShouldThrow<ObjectDisposedException>();
        }
    }
}