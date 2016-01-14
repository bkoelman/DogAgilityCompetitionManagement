using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="DelimitedValuesReader" /> objects in tests.
    /// </summary>
    public sealed class DelimitedValuesReaderBuilder : ITestDataBuilder<DelimitedValuesReader>
    {
        [NotNull]
        [ItemNotNull]
        private static readonly string[] DefaultHeaders = { "ColumnHeader1", "ColumnHeader2" };

        private bool useDefaultHeaders = true;

        [CanBeNull]
        private string headerLine;

        [CanBeNull]
        private Func<TextReader, TextReader> createReader;

        [NotNull]
        [ItemNotNull]
        private List<string> dataLines = new List<string> { "RowCell_1x1,RowCell_1x2", "RowCell_2x1,RowCell_2x2" };

        [CanBeNull]
        private DelimitedValuesReaderSettings settings = new DelimitedValuesReaderSettingsBuilder().Build();

        [ItemNotNull]
        public DelimitedValuesReader Build()
        {
            Stream stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            string fieldSeparator = GetFieldSeparatorFromSettings();

            string columnHeaderLine = null;
            if (headerLine == null && useDefaultHeaders)
            {
                columnHeaderLine = string.Join(fieldSeparator, DefaultHeaders);
            }
            else if (headerLine != null)
            {
                columnHeaderLine = headerLine;
            }

            if (columnHeaderLine != null)
            {
                writer.WriteLine(columnHeaderLine);
            }

            foreach (string dataLine in dataLines)
            {
                writer.WriteLine(dataLine);
            }

            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            TextReader reader = new StreamReader(stream);
            if (createReader != null)
            {
                reader = createReader(reader);
            }

            return new DelimitedValuesReader(reader, settings);
        }

        [NotNull]
        private string GetFieldSeparatorFromSettings()
        {
            return settings?.FieldSeparator?.ToString(CultureInfo.InvariantCulture) ?? ",";
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithSettings([NotNull] DelimitedValuesReaderSettingsBuilder settingsBuilder)
        {
            Guard.NotNull(settingsBuilder, nameof(settingsBuilder));

            settings = settingsBuilder.Build();
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithSingleColumnHeader([NotNull] string name = "ColumnHeader1")
        {
            return WithColumnHeaders(name);
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithColumnHeaders([NotNull] [ItemNotNull] params string[] headers)
        {
            string fieldSeparator = GetFieldSeparatorFromSettings();
            headerLine = string.Join(fieldSeparator, headers);
            useDefaultHeaders = false;
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithHeaderLine([NotNull] string line)
        {
            headerLine = line;
            useDefaultHeaders = false;
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithRow([NotNull] [ItemNotNull] IEnumerable<string> cells)
        {
            string fieldSeparator = GetFieldSeparatorFromSettings();
            dataLines.Add(string.Join(fieldSeparator, cells));
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithDataLine([NotNull] string dataLine)
        {
            dataLines.Add(dataLine);
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithoutRows()
        {
            dataLines = new List<string>();
            return this;
        }

        [NotNull]
        public DelimitedValuesReaderBuilder WithIntermediateReader(
            [NotNull] Func<TextReader, TextReader> createReaderCallback)
        {
            createReader = createReaderCallback;
            return this;
        }
    }
}