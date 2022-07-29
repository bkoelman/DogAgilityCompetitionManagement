using System.Globalization;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

/// <summary>
/// Specifies a set of features to support on the <see cref="DelimitedValuesReader" /> object.
/// </summary>
public sealed class DelimitedValuesReaderSettings
{
    /// <summary>
    /// Gets or sets whether to close the underlying reader on disposal. True by default.
    /// </summary>
    /// <value>
    /// <c>true</c> to close the underlying reader on disposal; otherwise, <c>false</c>.
    /// </value>
    public bool AutoCloseReader { get; set; }

    /// <summary>
    /// Gets or sets the character that is used to separate two cell values.
    /// </summary>
    /// <value>
    /// The field separator character. Set to <c>null</c> to use auto-detection.
    /// </value>
    public char? FieldSeparator { get; set; }

    /// <summary>
    /// Gets or sets the character that qualifies text in a cell.
    /// </summary>
    /// <value>
    /// The text qualifier character.
    /// </value>
    public char TextQualifier { get; set; }

    /// <summary>
    /// Gets or sets the culture to use for type conversions.
    /// </summary>
    /// <value>
    /// The culture. Set to <c>null</c> to use the invariant culture.
    /// </value>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets the maximum length of the line. Setting this prevents that the entire source is read in memory when an uneven number of text qualifiers
    /// occurs.
    /// </summary>
    /// <value>
    /// The maximum length of a single line of text in source.
    /// </value>
    public int? MaximumLineLength { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelimitedValuesReaderSettings" /> class.
    /// </summary>
    public DelimitedValuesReaderSettings()
    {
        AutoCloseReader = true;
        TextQualifier = '\"';
    }

    /// <summary>
    /// Creates a copy of this instance.
    /// </summary>
    public DelimitedValuesReaderSettings Clone()
    {
        return new DelimitedValuesReaderSettings
        {
            AutoCloseReader = AutoCloseReader,
            FieldSeparator = FieldSeparator,
            TextQualifier = TextQualifier,
            Culture = Culture != null ? new CultureInfo(Culture.Name) : null,
            MaximumLineLength = MaximumLineLength
        };
    }
}
