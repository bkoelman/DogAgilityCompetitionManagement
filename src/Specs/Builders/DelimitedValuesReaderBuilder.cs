using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="DelimitedValuesReader" /> objects in tests.
    /// </summary>
    public sealed class DelimitedValuesReaderBuilder : ITestDataBuilder<DelimitedValuesReader>
    {
        private static readonly string[] DefaultHeaders =
        {
            "ColumnHeader1",
            "ColumnHeader2"
        };

        private bool useDefaultHeaders = true;
        private string? headerLine;
        private Func<TextReader, TextReader>? createReader;

        private List<string> dataLines = new()
        {
            "RowCell_1x1,RowCell_1x2",
            "RowCell_2x1,RowCell_2x2"
        };

        private DelimitedValuesReaderSettings? settings = new DelimitedValuesReaderSettingsBuilder().Build();

        public DelimitedValuesReader Build()
        {
            Stream stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            string fieldSeparator = GetFieldSeparatorFromSettings();

            string? columnHeaderLine = null;

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

        private string GetFieldSeparatorFromSettings()
        {
            return settings?.FieldSeparator?.ToString(CultureInfo.InvariantCulture) ?? ",";
        }

        public DelimitedValuesReaderBuilder WithSettings(DelimitedValuesReaderSettingsBuilder settingsBuilder)
        {
            Guard.NotNull(settingsBuilder, nameof(settingsBuilder));

            settings = settingsBuilder.Build();
            return this;
        }

        public DelimitedValuesReaderBuilder WithSingleColumnHeader(string name = "ColumnHeader1")
        {
            return WithColumnHeaders(name);
        }

        public DelimitedValuesReaderBuilder WithColumnHeaders(params string[] headers)
        {
            string fieldSeparator = GetFieldSeparatorFromSettings();
            headerLine = string.Join(fieldSeparator, headers);
            useDefaultHeaders = false;
            return this;
        }

        public DelimitedValuesReaderBuilder WithHeaderLine(string line)
        {
            headerLine = line;
            useDefaultHeaders = false;
            return this;
        }

        public DelimitedValuesReaderBuilder WithRow(IEnumerable<string> cells)
        {
            string fieldSeparator = GetFieldSeparatorFromSettings();
            dataLines.Add(string.Join(fieldSeparator, cells));
            return this;
        }

        public DelimitedValuesReaderBuilder WithDataLine(string dataLine)
        {
            dataLines.Add(dataLine);
            return this;
        }

        public DelimitedValuesReaderBuilder WithoutRows()
        {
            dataLines = new List<string>();
            return this;
        }

        public DelimitedValuesReaderBuilder WithIntermediateReader(Func<TextReader, TextReader> createReaderCallback)
        {
            createReader = createReaderCallback;
            return this;
        }
    }
}
