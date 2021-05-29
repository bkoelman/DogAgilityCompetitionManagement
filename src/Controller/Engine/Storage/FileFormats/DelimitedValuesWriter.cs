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
        [NotNull]
        private readonly TextWriter target;

        [NotNull]
        [ItemNotNull]
        private readonly List<string> innerColumnNames;

        [NotNull]
        private readonly DelimitedValuesWriterSettings settings;

        [NotNull]
        private readonly CultureInfo effectiveCulture;

        private readonly char effectiveFieldSeparator;

        [NotNull]
        private readonly char[] charactersThatRequireEscaping;

        private bool isWriterDisposed;

        private bool headerPassed;

        /// <summary>
        /// Gets the column names, used to identify cell values.
        /// </summary>
        [NotNull]
        [ItemNotNull]
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
        public DelimitedValuesWriter([NotNull] TextWriter target, [NotNull] [ItemNotNull] ICollection<string> columnNames,
            [CanBeNull] DelimitedValuesWriterSettings settings = null)
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
        private static void AssertValidColumnNames([NotNull] [ItemNotNull] IEnumerable<string> columnNames)
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
        public void WriteLine([CanBeNull] string line)
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
        [NotNull]
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

        private void WriteDataRow([NotNull] [ItemNotNull] IEnumerable<string> cellValues)
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

        private void WriteRow([NotNull] [ItemNotNull] IEnumerable<string> cellValues)
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

        [NotNull]
        private string EnsureEscaped([NotNull] string value)
        {
            if (RequiresEscaping(value))
            {
                string textQualifier = new(settings.TextQualifier, 1);
                value = value.Replace(textQualifier, textQualifier + textQualifier);
                return textQualifier + value + textQualifier;
            }

            return value;
        }

        private bool RequiresEscaping([NotNull] string value)
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

        private static bool HasLeadingWhiteSpace([NotNull] string value)
        {
            return char.IsWhiteSpace(value[0]);
        }

        private static bool HasTrailingWhiteSpace([NotNull] string value)
        {
            return char.IsWhiteSpace(value[^1]);
        }

        private sealed class DelimitedValuesWriterRow : IDelimitedValuesWriterRow
        {
#pragma warning disable 649 // Readonly field is never assigned
            [NotNull]
            // ReSharper disable once NotNullMemberIsNotInitialized
            // Reason: This blocks Resharper warning "Possible 'null' assignment to entity marked with 'NotNull' attribute"
            private static readonly ITypeDescriptorContext NullContext;
#pragma warning restore 649

            [NotNull]
            private readonly DelimitedValuesWriter target;

            [NotNull]
            [ItemNotNull]
            private readonly List<string> cellValues;

            private bool isRowDisposed;

            public IReadOnlyCollection<string> ColumnNames => target.ColumnNames;

            public DelimitedValuesWriterRow([NotNull] DelimitedValuesWriter target)
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

            public void SetCell(string columnName, string value)
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

            public void SetCell<T>(string columnName, T value, Converter<T, string> converter = null)
            {
                Guard.NotNullNorEmpty(columnName, nameof(columnName));
                AssertRowNotDisposed();

                string cellValue = converter != null ? converter(value) : ConvertCell(value);
                SetCell(columnName, cellValue);
            }

            [CanBeNull]
            private string ConvertCell<T>([CanBeNull] T value)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                return value is not null ? typeConverter.ConvertToString(NullContext, target.effectiveCulture, value) : null;
            }
        }
    }
}
