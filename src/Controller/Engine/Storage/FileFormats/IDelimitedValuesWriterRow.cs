using System;
using System.Collections.Generic;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats
{
    /// <summary>
    /// Represents a row to be written to a file in delimited (typically CSV) format.
    /// </summary>
    public interface IDelimitedValuesWriterRow : IDisposable
    {
        /// <summary>
        /// Gets the column names.
        /// </summary>
        IReadOnlyCollection<string> ColumnNames { get; }

        /// <summary>
        /// Sets the value of the cell at the specified column.
        /// </summary>
        /// <param name="columnName">
        /// Name of the column.
        /// </param>
        /// <param name="value">
        /// The cell value to assign.
        /// </param>
        void SetCell(string columnName, string? value);

        /// <summary>
        /// Converts a value and puts it in the cell at the specified column.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to convert from.
        /// </typeparam>
        /// <param name="columnName">
        /// Name of the column.
        /// </param>
        /// <param name="value">
        /// The cell value to assign, after conversion.
        /// </param>
        /// <param name="converter">
        /// Optional. Used to convert from <typeparamref name="T" /> to string.
        /// </param>
        void SetCell<T>(string columnName, T? value, Converter<T?, string>? converter = null);
    }
}
