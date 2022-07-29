using System.Globalization;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

/// <summary>
/// Specifies a set of features to support on the <see cref="DelimitedValuesWriter" /> object.
/// </summary>
public sealed class DelimitedValuesWriterSettings
{
    /// <summary>
    /// Gets or sets whether to write column names on the first line.
    /// </summary>
    /// <value>
    /// <c>true</c> to write column names on the first line (default); otherwise, <c>false</c>.
    /// </value>
    public bool IncludeColumnNamesOnFirstLine { get; set; }

    /// <summary>
    /// Gets or sets whether to close the underlying writer on disposal. True by default.
    /// </summary>
    /// <value>
    /// <c>true</c> to close the underlying writer on disposal (default); otherwise, <c>false</c>.
    /// </value>
    public bool AutoCloseWriter { get; set; }

    /// <summary>
    /// Gets or sets the character that is used to separate two cell values.
    /// </summary>
    /// <value>
    /// The field separator character. Set to <c>null</c> to use auto-detection based on culture (default).
    /// </value>
    public char? FieldSeparator { get; set; }

    /// <summary>
    /// Gets or sets the character that qualifies text in a cell.
    /// </summary>
    /// <value>
    /// The text qualifier character. A double quote by default.
    /// </value>
    public char TextQualifier { get; set; }

    /// <summary>
    /// Gets or sets the culture that is used in type conversions.
    /// </summary>
    /// <value>
    /// The culture. Set to <c>null</c> (default) to use the invariant culture.
    /// </value>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelimitedValuesWriterSettings" /> class.
    /// </summary>
    public DelimitedValuesWriterSettings()
    {
        IncludeColumnNamesOnFirstLine = true;
        AutoCloseWriter = true;
        TextQualifier = '\"';
    }

    /// <summary>
    /// Creates a copy of this instance.
    /// </summary>
    public DelimitedValuesWriterSettings Clone()
    {
        return new DelimitedValuesWriterSettings
        {
            IncludeColumnNamesOnFirstLine = IncludeColumnNamesOnFirstLine,
            AutoCloseWriter = AutoCloseWriter,
            FieldSeparator = FieldSeparator,
            TextQualifier = TextQualifier,
            Culture = Culture != null ? new CultureInfo(Culture.Name) : null
        };
    }
}
