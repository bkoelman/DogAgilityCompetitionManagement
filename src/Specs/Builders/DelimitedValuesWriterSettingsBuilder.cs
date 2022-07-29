using System.Globalization;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

namespace DogAgilityCompetition.Specs.Builders;

/// <summary>
/// Enables composition of <see cref="DelimitedValuesWriterSettings" /> objects in tests.
/// </summary>
public sealed class DelimitedValuesWriterSettingsBuilder : ITestDataBuilder<DelimitedValuesWriterSettings>
{
    private bool? includeColumnNamesOnFirstLine;
    private bool? autoCloseWriter;
    private char? fieldSeparator;
    private char? textQualifier;
    private CultureInfo? culture;

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

    public DelimitedValuesWriterSettingsBuilder WithColumnNamesOnFirstLine()
    {
        includeColumnNamesOnFirstLine = true;
        return this;
    }

    public DelimitedValuesWriterSettingsBuilder WithoutColumnNamesOnFirstLine()
    {
        includeColumnNamesOnFirstLine = false;
        return this;
    }

    public DelimitedValuesWriterSettingsBuilder KeepingReaderOpen()
    {
        autoCloseWriter = false;
        return this;
    }

    public DelimitedValuesWriterSettingsBuilder WithFieldSeparator(char? separator)
    {
        fieldSeparator = separator;
        return this;
    }

    public DelimitedValuesWriterSettingsBuilder WithTextQualifier(char qualifier)
    {
        textQualifier = qualifier;
        return this;
    }

    public DelimitedValuesWriterSettingsBuilder WithCulture(CultureInfo? c)
    {
        culture = c;
        return this;
    }
}
