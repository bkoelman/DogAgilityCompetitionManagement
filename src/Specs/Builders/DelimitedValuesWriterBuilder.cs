using System.Collections.Generic;
using System.IO;
using System.Linq;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="DelimitedValuesWriter" /> objects in tests.
    /// </summary>
    public sealed class DelimitedValuesWriterBuilder : ITestDataBuilder<DelimitedValuesWriter>
    {
        [NotNull]
        [ItemNotNull]
        private static readonly List<string> DefaultHeaders = new()
        {
            "ColumnHeader1",
            "ColumnHeader2"
        };

        private bool useDefaultHeaders = true;

        [CanBeNull]
        [ItemNotNull]
        private List<string> columnHeaders;

        [NotNull]
        private DelimitedValuesWriterSettingsBuilder settingsBuilder = new();

        [CanBeNull]
        private TextWriter writer;

        public DelimitedValuesWriter Build()
        {
            DelimitedValuesWriterSettings settings = settingsBuilder.Build();
            TextWriter targetWriter = writer ?? new StreamWriter(Stream.Null);
            List<string> headers = useDefaultHeaders ? DefaultHeaders : columnHeaders;

            // ReSharper disable once AssignNullToNotNullAttribute
            // Reason: It must be testable to fail when headers are omitted.
            return new DelimitedValuesWriter(targetWriter, headers, settings);
        }

        [NotNull]
        public DelimitedValuesWriterBuilder WithSettings([NotNull] DelimitedValuesWriterSettingsBuilder settings)
        {
            settingsBuilder = settings;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterBuilder WritingTo([NotNull] TextWriter targetWriter)
        {
            writer = targetWriter;
            return this;
        }

        [NotNull]
        public DelimitedValuesWriterBuilder WithSingleColumnHeader([NotNull] string name = "ColumnHeader1")
        {
            return WithColumnHeaders(name);
        }

        [NotNull]
        public DelimitedValuesWriterBuilder WithColumnHeaders([CanBeNull] [ItemNotNull] params string[] headers)
        {
            columnHeaders = headers?.ToList();
            useDefaultHeaders = false;
            return this;
        }
    }
}
