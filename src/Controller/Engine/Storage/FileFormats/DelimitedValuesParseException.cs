namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats;

/// <summary>
/// The exception that is thrown when unable to parse data from a delimited file.
/// </summary>
public sealed class DelimitedValuesParseException : Exception
{
    public DelimitedValuesParseException(string? message)
        : base(message)
    {
    }
}
