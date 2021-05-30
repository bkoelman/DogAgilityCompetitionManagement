using System;
using System.Collections.Generic;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats
{
    /// <summary>
    /// Represents a row from a file in delimited (typically CSV) format.
    /// </summary>
    public interface IDelimitedValuesReaderRow
    {
        /// <summary>
        /// Gets the column names.
        /// </summary>
        IReadOnlyCollection<string> ColumnNames { get; }

        /// <summary>
        /// Gets the cell values as strings.
        /// </summary>
        IReadOnlyCollection<string> GetCells();

        /// <summary>
        /// Gets the cell value at the specified column.
        /// </summary>
        /// <param name="columnName">
        /// Name of the column.
        /// </param>
        /// <returns>
        /// The cell value.
        /// </returns>
        string GetCell(string columnName);

        /// <summary>
        /// Gets the cell value at the specified column and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type to convert the cell value to.
        /// </typeparam>
        /// <param name="columnName">
        /// Name of the column.
        /// </param>
        /// <param name="converter">
        /// Optional. Used to convert from string to <typeparamref name="T" />.
        /// </param>
        /// <returns>
        /// The converted cell value.
        /// </returns>
        T? GetCell<T>(string columnName, Converter<string, T>? converter = null);
    }
}
