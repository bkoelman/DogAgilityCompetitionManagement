using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats
{
    /// <summary>
    /// Provides a forward-only writer for files in delimited (typically CSV) format. The first line of the output file contains column names by default.
    /// </summary>
    /// <remarks>
    /// This writer is compatible with Microsoft Excel .csv format.
    /// </remarks>
    public sealed class DelimitedValuesWriter : IDisposable
    {
        private readonly TextWriter target;
        private readonly List<string> innerColumnNames;
        private readonly DelimitedValuesWriterSettings settings;
        private readonly CultureInfo effectiveCulture;
        private readonly char effectiveFieldSeparator;
        private readonly char[] charactersThatRequireEscaping;

        private bool isWriterDisposed;
        private bool headerPassed;

        /// <summary>
        /// Gets the column names, used to identify cell values.
        /// </summary>
        public IReadOnlyCollection<string> ColumnNames => new ReadOnlyCollection<string>(innerColumnNames);

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedValuesWriter" /> class.
        /// </summary>
        /// <param name="target">
        /// The writer to write output to.
        /// </param>
        /// <param name="columnNames">
        /// The column name, used to identify cell values.
        /// </param>
        /// <param name="settings">
        /// Settings that customize the behavior of this instance.
        /// </param>
        public DelimitedValuesWriter(TextWriter target, ICollection<string> columnNames, DelimitedValuesWriterSettings? settings = null)
        {
            Guard.NotNull(target, nameof(target));
            AssertValidColumnNames(columnNames);

            this.target = target;
            innerColumnNames = columnNames.ToList();
            this.settings = settings?.Clone() ?? new DelimitedValuesWriterSettings();
            effectiveCulture = this.settings.Culture ?? CultureInfo.InvariantCulture;
            effectiveFieldSeparator = GetEffectiveFieldSeparator();

            charactersThatRequireEscaping = new[]
            {
                '\r',
                '\n',
                effectiveFieldSeparator,
                this.settings.TextQualifier
            };
        }

        [AssertionMethod]
        private static void AssertValidColumnNames(IEnumerable<string> columnNames)
        {
            Guard.NotNull(columnNames, nameof(columnNames));

            var nameSet = new HashSet<string>();

            foreach (string columnName in columnNames)
            {
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    throw new ArgumentException("Column names cannot be null, empty or whitespace.", nameof(columnNames));
                }

                if (nameSet.Contains(columnName))
                {
                    throw new ArgumentException($"Column '{columnName}' occurs multiple times.", nameof(columnNames));
                }

                nameSet.Add(columnName);
            }

            if (nameSet.Count == 0)
            {
                throw new ArgumentException("List of column names cannot be empty.", nameof(columnNames));
            }
        }

        private char GetEffectiveFieldSeparator()
        {
            if (settings.FieldSeparator == null)
            {
                return effectiveCulture.NumberFormat.NumberDecimalSeparator == "," || effectiveCulture.NumberFormat.CurrencyDecimalSeparator == "," ||
                    effectiveCulture.NumberFormat.PercentDecimalSeparator == ","
                        ? ';'
                        : ',';
            }

            return settings.FieldSeparator.Value;
        }

        /// <summary>
        /// Closes the underlying writer (default), unless custom settings indicate otherwise.
        /// </summary>
        public void Dispose()
        {
            if (!isWriterDisposed)
            {
                EnsureHeaderWritten();

                if (settings.AutoCloseWriter)
                {
                    target.Dispose();
                }
                else
                {
                    target.Flush();
                }

                isWriterDisposed = true;
            }
        }

        /// <summary>
        /// Directly writes the specified line to the underlying writer.
        /// </summary>
        /// <param name="line">
        /// The line of text to write.
        /// </param>
        public void WriteLine(string? line)
        {
            AssertWriterNotDisposed();
            target.WriteLine(line);
        }

        /// <summary>
        /// Returns an object on which cell values can be set. On disposal, the row is written to the underlying writer.
        /// </summary>
        /// <returns>
        /// The row object.
        /// </returns>
        public IDelimitedValuesWriterRow CreateRow()
        {
            AssertWriterNotDisposed();
            return new DelimitedValuesWriterRow(this);
        }

        [AssertionMethod]
        private void AssertWriterNotDisposed()
        {
            if (isWriterDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private void WriteDataRow(IEnumerable<string> cellValues)
        {
            AssertWriterNotDisposed();

            EnsureHeaderWritten();
            WriteRow(cellValues);
        }

        private void EnsureHeaderWritten()
        {
            if (!headerPassed && settings.IncludeColumnNamesOnFirstLine)
            {
                WriteRow(innerColumnNames);
            }

            headerPassed = true;
        }

        private void WriteRow(IEnumerable<string> cellValues)
        {
            bool isFirstCell = true;

            foreach (string cellValue in cellValues)
            {
                if (!isFirstCell)
                {
                    target.Write(effectiveFieldSeparator);
                }
                else
                {
                    isFirstCell = false;
                }

                string cell = EnsureEscaped(cellValue);
                target.Write(cell);
            }

            target.WriteLine();
        }

        private string EnsureEscaped(string value)
        {
            if (RequiresEscaping(value))
            {
                string textQualifier = new(settings.TextQualifier, 1);
                value = value.Replace(textQualifier, textQualifier + textQualifier);
                return textQualifier + value + textQualifier;
            }

            return value;
        }

        private bool RequiresEscaping(string value)
        {
            if (value.IndexOfAny(charactersThatRequireEscaping) != -1)
            {
                return true;
            }

            if (value.Length > 0)
            {
                if (HasLeadingWhiteSpace(value) || HasTrailingWhiteSpace(value))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasLeadingWhiteSpace(string value)
        {
            return char.IsWhiteSpace(value[0]);
        }

        private static bool HasTrailingWhiteSpace(string value)
        {
            return char.IsWhiteSpace(value[^1]);
        }

        private sealed class DelimitedValuesWriterRow : IDelimitedValuesWriterRow
        {
            private static readonly ITypeDescriptorContext NullContext = null!;

            private readonly DelimitedValuesWriter target;
            private readonly List<string> cellValues;

            private bool isRowDisposed;

            public IReadOnlyCollection<string> ColumnNames => target.ColumnNames;

            public DelimitedValuesWriterRow(DelimitedValuesWriter target)
            {
                this.target = target;

                cellValues = new List<string>(target.innerColumnNames.Count);

                for (int index = 0; index < target.innerColumnNames.Count; index++)
                {
                    cellValues.Add(string.Empty);
                }
            }

            public void Dispose()
            {
                if (!isRowDisposed)
                {
                    // Throws intentionally when trying to complete this row
                    // while underlying writer has been closed.
                    target.WriteDataRow(cellValues);
                    isRowDisposed = true;
                }
            }

            public void SetCell(string columnName, string? value)
            {
                Guard.NotNullNorEmpty(columnName, nameof(columnName));
                AssertRowNotDisposed();

                int index = target.innerColumnNames.FindIndex(c => c == columnName);

                if (index == -1)
                {
                    throw new ArgumentException($"Column with name '{columnName}' does not exist.", columnName);
                }

                cellValues[index] = value ?? string.Empty;
            }

            [AssertionMethod]
            private void AssertRowNotDisposed()
            {
                if (isRowDisposed)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }
            }

            public void SetCell<T>(string columnName, T? value, Converter<T?, string>? converter = null)
            {
                Guard.NotNullNorEmpty(columnName, nameof(columnName));
                AssertRowNotDisposed();

                string? cellValue = converter != null ? converter(value) : ConvertCell(value);
                SetCell(columnName, cellValue);
            }

            private string? ConvertCell<T>(T? value)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                return value is not null ? typeConverter.ConvertToString(NullContext, target.effectiveCulture, value) : null;
            }
        }
    }
}
