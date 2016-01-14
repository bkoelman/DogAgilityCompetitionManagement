using System.Globalization;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="DelimitedValuesWriterSettings" /> objects in tests.
    /// </summary>
    public sealed class DelimitedValuesWriterSettingsBuilder : ITestDataBuilder<DelimitedValuesWriterSettings>
    {
        [CanBeNull]
        private bool? includeColumnNamesOnFirstLine;

        [CanBeNull]
        private bool? autoCloseWriter;

        [CanBeNull]
        private char? fieldSeparator;

        [CanBeNull]
        private char? textQualifier;

        [CanBeNull]
        private CultureInfo culture;

        public DelimitedValuesWriterSettings Build()
        {
            var settings = new DelimitedValuesWriterSettings();
            if (includeColumnNamesOnFirstLine != null)
            {
                settings.IncludeColumnNamesOnFirstLine = includeColumnNamesOnFirstLine.Value;
            }
            if (autoCloseWriter != null)
            {
                settings.AutoCloseWriter = autoCloseWriter.Value;
            }
            settings.FieldSeparator = fieldSeparator;
            if (textQualifier != null)
            {
                settings.TextQualifier = textQualifier.Value;
            }
            settings.Culture = culture;
            return settings;
        }

        [NotNull]
        public DelimitedValuesWriterSettingsBuilder WithColumnNamesOnFirstLine()
        {
            includeColumnNamesOnFirstLine = true;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterSettingsBuilder WithoutColumnNamesOnFirstLine()
        {
            includeColumnNamesOnFirstLine = false;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterSettingsBuilder KeepingReaderOpen()
        {
            autoCloseWriter = false;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterSettingsBuilder WithFieldSeparator([CanBeNull] char? separator)
        {
            fieldSeparator = separator;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterSettingsBuilder WithTextQualifier(char qualifier)
        {
            textQualifier = qualifier;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterSettingsBuilder WithCulture([CanBeNull] CultureInfo c)
        {
            culture = c;
            return this;
        }
    }
}