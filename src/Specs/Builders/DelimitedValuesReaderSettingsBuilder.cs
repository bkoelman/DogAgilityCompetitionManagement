using System.Globalization;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="DelimitedValuesReaderSettings" /> objects in tests.
    /// </summary>
    public sealed class DelimitedValuesReaderSettingsBuilder : ITestDataBuilder<DelimitedValuesReaderSettings>
    {
        [CanBeNull]
        private char? fieldSeparator;

        [CanBeNull]
        private CultureInfo culture;

        [CanBeNull]
        private int? maximumLineLength;

        public DelimitedValuesReaderSettings Build()
        {
            return new()
            {
                FieldSeparator = fieldSeparator,
                Culture = culture,
                MaximumLineLength = maximumLineLength
            };
        }

        [NotNull]
        public DelimitedValuesReaderSettingsBuilder WithFieldSeparator([CanBeNull] char? separator)
        {
            fieldSeparator = separator;
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderSettingsBuilder WithCulture([CanBeNull] CultureInfo c)
        {
            culture = c;
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderSettingsBuilder WithMaximumLineLength([CanBeNull] int? length)
        {
            maximumLineLength = length;
            return this;
        }
    }
}
