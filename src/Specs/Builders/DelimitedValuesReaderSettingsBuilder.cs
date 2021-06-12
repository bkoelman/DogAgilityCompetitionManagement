using System.Globalization;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="DelimitedValuesReaderSettings" /> objects in tests.
    /// </summary>
    public sealed class DelimitedValuesReaderSettingsBuilder : ITestDataBuilder<DelimitedValuesReaderSettings>
    {
        private char? fieldSeparator;
        private CultureInfo? culture;
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

        public DelimitedValuesReaderSettingsBuilder WithFieldSeparator(char? separator)
        {
            fieldSeparator = separator;
            return this;
        }

        public DelimitedValuesReaderSettingsBuilder WithCulture(CultureInfo? c)
        {
            culture = c;
            return this;
        }

        public DelimitedValuesReaderSettingsBuilder WithMaximumLineLength(int? length)
        {
            maximumLineLength = length;
            return this;
        }
    }
}
