using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

namespace DogAgilityCompetition.Specs.Builders;

/// <summary>
/// Enables composition of <see cref="DelimitedValuesWriter" /> objects in tests.
/// </summary>
public sealed class DelimitedValuesWriterBuilder : ITestDataBuilder<DelimitedValuesWriter>
{
    private static readonly List<string> DefaultHeaders = new()
    {
        "ColumnHeader1",
        "ColumnHeader2"
    };

    private bool useDefaultHeaders = true;
    private List<string>? columnHeaders;
    private DelimitedValuesWriterSettingsBuilder settingsBuilder = new();
    private TextWriter? writer;

    public DelimitedValuesWriter Build()
    {
        DelimitedValuesWriterSettings settings = settingsBuilder.Build();
        TextWriter targetWriter = writer ?? new StreamWriter(Stream.Null);
        List<string>? headers = useDefaultHeaders ? DefaultHeaders : columnHeaders;

        // Justification for nullable suppression: It must be testable to fail when headers are omitted.
        return new DelimitedValuesWriter(targetWriter, headers!, settings);
    }

    public DelimitedValuesWriterBuilder WithSettings(DelimitedValuesWriterSettingsBuilder settings)
    {
        settingsBuilder = settings;
        return this;
    }

    public DelimitedValuesWriterBuilder WritingTo(TextWriter targetWriter)
    {
        writer = targetWriter;
        return this;
    }

    public DelimitedValuesWriterBuilder WithSingleColumnHeader(string name = "ColumnHeader1")
    {
        return WithColumnHeaders(name);
    }

    public DelimitedValuesWriterBuilder WithColumnHeaders(params string[]? headers)
    {
        columnHeaders = headers?.ToList();
        useDefaultHeaders = false;
        return this;
    }
}
