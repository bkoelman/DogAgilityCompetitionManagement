using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats
{
    /// <summary>
    /// Provides a forward-only reader for files in delimited (typically CSV) format. The first line of the input file must contain column names.
    /// </summary>
    /// <remarks>
    /// This reader is compatible with Microsoft Excel .csv format, although parsing is slightly more strict. See
    /// http://creativyst.com/Doc/Articles/CSV/CSV01.htm#CSVAndExcel
    /// </remarks>
    public sealed class DelimitedValuesReader : IEnumerable<IDelimitedValuesReaderRow>, IDisposable
    {
        [NotNull]
        private readonly DelimitedValuesReaderSettings settings;

        [NotNull]
        private readonly DelimitedValuesEnumerator enumerator;

        [CanBeNull]
        private TextReader source;

        /// <summary>
        /// Gets the line number in source at which the current row starts.
        /// </summary>
        public int LineNumber
        {
            get
            {
                AssertNotDisposed();
                return enumerator.RowStartLineNumber;
            }
        }

        /// <summary>
        /// Gets the column names on the first line in source.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyCollection<string> ColumnNames => new ReadOnlyCollection<string>(enumerator.ColumnNames);

        /// <summary>
        /// Gets the current unparsed line of text from source.
        /// </summary>
        /// <remarks>
        /// Can be used for logging purposes, after parsing into cells has thrown an exception.
        /// </remarks>
        [NotNull]
        public string Line
        {
            get
            {
                AssertNotDisposed();
                return enumerator.Line;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedValuesReader" /> class.
        /// </summary>
        /// <param name="source">
        /// The reader that provides input.
        /// </param>
        /// <param name="settings">
        /// Settings that customize the behavior of this instance.
        /// </param>
        public DelimitedValuesReader([NotNull] TextReader source, [CanBeNull] DelimitedValuesReaderSettings settings = null)
        {
            Guard.NotNull(source, nameof(source));

            this.settings = settings?.Clone() ?? new DelimitedValuesReaderSettings();
            this.source = source;
            enumerator = new DelimitedValuesEnumerator(this);
        }

        /// <summary>
        /// Closes the underlying source (default), unless custom settings indicate otherwise.
        /// </summary>
        public void Dispose()
        {
            if (source != null)
            {
                if (settings.AutoCloseReader)
                {
                    source.Dispose();
                }

                enumerator.Dispose();
                source = null;
            }
        }

        /// <summary>
        /// Returns the enumerator that iterates through the source rows.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        public IEnumerator<IDelimitedValuesReaderRow> GetEnumerator()
        {
            AssertNotDisposed();
            return enumerator;
        }

        /// <summary>
        /// Returns the enumerator that iterates through the source rows.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [AssertionMethod]
        private void AssertNotDisposed()
        {
            if (source == null)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private sealed class DelimitedValuesEnumerator : IEnumerator<DelimitedValuesReaderRow>
        {
            private const char CarriageReturnCharCode = (char)13;
            private const char LineFeedCharCode = (char)10;

            [NotNull]
            [ItemNotNull]
            private readonly DelimitedValuesReader owner;

            [NotNull]
            [ItemNotNull]
            private readonly List<string> columnNames;

            private char effectiveFieldSeparator;
            private int readerLineNumber;
            private int rowStartLineNumber;

            [CanBeNull]
            private DelimitedValuesReaderRow currentRow;

            [CanBeNull]
            private string currentLine;

            object IEnumerator.Current => Current;

            private bool IsPositionBeforeStart => readerLineNumber == 0;

            public DelimitedValuesReaderRow Current
            {
                get
                {
                    AssertNotBeforeStart();
                    return currentRow;
                }
            }

            public int RowStartLineNumber
            {
                get
                {
                    AssertNotBeforeStart();
                    return rowStartLineNumber;
                }
            }

            [NotNull]
            public string Line
            {
                get
                {
                    AssertNotBeforeStart();
                    return currentLine ?? string.Empty;
                }
            }

            [NotNull]
            [ItemNotNull]
            public List<string> ColumnNames
            {
                get
                {
                    AssertNotBeforeStart();
                    return columnNames;
                }
            }

            [NotNull]
            public CultureInfo EffectiveCulture { get; }

            public DelimitedValuesEnumerator([NotNull] [ItemNotNull] DelimitedValuesReader owner)
            {
                Guard.NotNull(owner, nameof(owner));
                owner.AssertNotDisposed();

                this.owner = owner;
                EffectiveCulture = this.owner.settings.Culture ?? CultureInfo.InvariantCulture;

                string headerLine = ConsumeLinesForSingleRow();

                if (headerLine == null)
                {
                    throw new DelimitedValuesParseException("Missing column names on first line.");
                }

                columnNames = ParseColumnHeaders(headerLine);
            }

            [NotNull]
            [ItemNotNull]
            private List<string> ParseColumnHeaders([NotNull] string line)
            {
                SetEffectiveFieldSeparator(line);

                List<string> columnHeaderNames = ParseLineIntoCells(line);
                AssertNotEmpty(columnHeaderNames);
                AssertNoDuplicates(columnHeaderNames);
                return columnHeaderNames;
            }

            private void SetEffectiveFieldSeparator([NotNull] string line)
            {
                if (owner.settings.FieldSeparator != null)
                {
                    effectiveFieldSeparator = owner.settings.FieldSeparator.Value;
                    return;
                }

                effectiveFieldSeparator = AutoDetectFieldSeparator(line);
            }

            private static char AutoDetectFieldSeparator([NotNull] string line)
            {
                // It would be even better to not count occurrences inside qualified text.
                // But because callers are expected to verify that expected column headers exist
                // in source, an incorrect separator detection is likely to fail during header 
                // verification anyway.

                var charCounts = new Dictionary<char, int>
                {
                    { '\t', line.Count(c => c == '\t') },
                    { ';', line.Count(c => c == ';') },
                    { ',', line.Count(c => c == ',') },
                    { ':', line.Count(c => c == ':') },
                    { '|', line.Count(c => c == '|') }
                };

                int highestOccurrence = charCounts.Max(pair => pair.Value);
                return charCounts.First(pair => pair.Value == highestOccurrence).Key;
            }

            [NotNull]
            [ItemNotNull]
            private List<string> ParseLineIntoCells([NotNull] string line)
            {
                // A cell must be surrounded by text qualifiers when the cell value contains field separator 
                // characters, significant leading/trailing whitespace, line breaks or text qualifier 
                // characters. Any text qualifier characters in the cell value must be duplicated.
                //
                // Example: __"_Doe,_John_"__,_"a_""great""_idea"_ => { _Doe,_John_ , a_"great"_idea }

                var cells = new List<string>();
                StringBuilder cellBuilder = null;

                // True when cell starts with a normal character (not a text qualifier).
                bool? isPlainCell = null;

                // True when positioned inside text-qualified section within a cell.
                bool inTextSection = false;

                // True when passed the closing qualifier of a text-qualified cell.
                bool textSectionCompleted = false;

                for (int index = 0; index < line.Length; index++)
                {
                    if (line[index] == effectiveFieldSeparator && !inTextSection)
                    {
                        string cell = cellBuilder?.ToString() ?? string.Empty;

                        if (isPlainCell != null && isPlainCell.Value)
                        {
                            cell = cell.TrimEnd();
                        }

                        cells.Add(cell);

                        cellBuilder = new StringBuilder();
                        isPlainCell = null;
                        textSectionCompleted = false;
                    }
                    else if (line[index] == owner.settings.TextQualifier)
                    {
                        if (isPlainCell == null || inTextSection)
                        {
                            // Check for "" inside a quoted string.
                            if (inTextSection && index + 1 < line.Length && line[index + 1] == owner.settings.TextQualifier)
                            {
                                cellBuilder ??= new StringBuilder();
                                cellBuilder.Append(owner.settings.TextQualifier);
                                index++;
                            }
                            else
                            {
                                inTextSection = !inTextSection;
                                isPlainCell = false;

                                if (!inTextSection)
                                {
                                    textSectionCompleted = true;
                                }
                            }
                        }
                        else
                        {
                            throw new DelimitedValuesParseException("Text qualifier must be the first non-whitespace character of a cell.");
                        }
                    }
                    else
                    {
                        if (isPlainCell == null && char.IsWhiteSpace(line[index]))
                        {
                            // Skip over leading whitespace.
                        }
                        else if (textSectionCompleted)
                        {
                            // Skip over trailing whitespace for text-qualified cell.
                            if (!char.IsWhiteSpace(line[index]))
                            {
                                throw new DelimitedValuesParseException("Text-qualified cell cannot contain non-whitespace after the closing text qualifier.");
                            }
                        }
                        else
                        {
                            cellBuilder ??= new StringBuilder();
                            cellBuilder.Append(line[index]);

                            isPlainCell ??= true;
                        }
                    }
                }

                if (cellBuilder != null)
                {
                    string lastCell = cellBuilder.ToString();

                    if (isPlainCell != null && isPlainCell.Value)
                    {
                        lastCell = lastCell.TrimEnd();
                    }

                    cells.Add(lastCell);
                }

                return cells;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                owner.AssertNotDisposed();

                string dataLine = ConsumeLinesForSingleRow();

                if (dataLine == null)
                {
                    return false;
                }

                List<string> cellValues = ParseLineIntoCells(dataLine);
                AssertCellCountSameAsColumnCount(cellValues);
                currentRow = new DelimitedValuesReaderRow(this, cellValues);

                return true;
            }

            [CanBeNull]
            private string ConsumeLinesForSingleRow()
            {
                int newRowStartLineNumber = readerLineNumber + 1;

                string line = ReadToNextLineBreak(out bool missingClosingTextQualifier);

                // Set properties before parsing into cells, so that callers can inspect/log 
                // the unparsed text line after catching any exceptions raised while parsing.
                rowStartLineNumber = newRowStartLineNumber;
                currentLine = line;

                if (missingClosingTextQualifier)
                {
                    throw new DelimitedValuesParseException("Missing closing text qualifier.");
                }

                return line;
            }

            [CanBeNull]
            private string ReadToNextLineBreak(out bool missingClosingTextQualifier)
            {
                // Keep reading until we see a line break outside a quoted field.
                int charsSeen = 0;
                bool inTextSection = false;

                var builder = new StringBuilder();

                while (true)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    // Reason: All callers up the call stack have called owner.AssertNotDisposed().
                    int charCode = owner.source.Read();

                    if (charCode != -1)
                    {
                        charsSeen++;
                    }

                    bool maximumReached = MaximumLineLengthEquals(charsSeen);

                    if (charCode == -1 || maximumReached)
                    {
                        missingClosingTextQualifier = inTextSection;
                        return builder.Length > 0 ? builder.ToString() : null;
                    }

                    if (charCode == CarriageReturnCharCode || charCode == LineFeedCharCode)
                    {
                        readerLineNumber++;

                        if (charCode == CarriageReturnCharCode && owner.source.Peek() == LineFeedCharCode)
                        {
                            owner.source.Read();
                            charsSeen++;

                            if (inTextSection)
                            {
                                builder.Append(CarriageReturnCharCode);
                                builder.Append(LineFeedCharCode);
                                continue;
                            }
                        }
                        else
                        {
                            if (inTextSection)
                            {
                                builder.Append((char)charCode);
                                continue;
                            }
                        }

                        missingClosingTextQualifier = false;

                        if (builder.Length == 0 && owner.source.Peek() == -1)
                        {
                            // Allow a single empty line at EOF.
                            owner.source.Read();
                            return null;
                        }

                        return builder.ToString();
                    }

                    if (charCode == owner.settings.TextQualifier)
                    {
                        inTextSection = !inTextSection;
                    }

                    builder.Append((char)charCode);
                }
            }

            private bool MaximumLineLengthEquals(int charsSeen)
            {
                return owner.settings.MaximumLineLength != null && owner.settings.MaximumLineLength == charsSeen;
            }

            [AssertionMethod]
            private void AssertNotBeforeStart()
            {
                if (IsPositionBeforeStart)
                {
                    throw new InvalidOperationException("Call MoveNext() first.");
                }
            }

            [AssertionMethod]
            private static void AssertNotEmpty([NotNull] [ItemNotNull] List<string> columnHeaderNames)
            {
                if (columnHeaderNames.Count == 0)
                {
                    throw new DelimitedValuesParseException("Source contains no columns.");
                }
            }

            [AssertionMethod]
            private static void AssertNoDuplicates([NotNull] [ItemNotNull] IEnumerable<string> headerColumnNames)
            {
                var columnSet = new HashSet<string>();

                foreach (string columnName in headerColumnNames)
                {
                    if (columnSet.Contains(columnName))
                    {
                        throw new DelimitedValuesParseException($"Column '{columnName}' occurs multiple times.");
                    }

                    columnSet.Add(columnName);
                }
            }

            [AssertionMethod]
            private void AssertCellCountSameAsColumnCount([NotNull] [ItemNotNull] IReadOnlyCollection<string> cellValues)
            {
                if (cellValues.Count != columnNames.Count)
                {
                    throw new DelimitedValuesParseException($"Expected {columnNames.Count} cells on row instead of {cellValues.Count}.");
                }
            }
        }

        private sealed class DelimitedValuesReaderRow : IDelimitedValuesReaderRow
        {
#pragma warning disable 649 // Readonly field is never assigned
            [NotNull]
            // ReSharper disable once NotNullMemberIsNotInitialized
            // Reason: This blocks Resharper warning "Possible 'null' assignment to entity marked with 'NotNull' attribute"
            private static readonly ITypeDescriptorContext NullContext;
#pragma warning restore 649

            [NotNull]
            private readonly DelimitedValuesEnumerator sourceEnumerator;

            [NotNull]
            [ItemNotNull]
            private readonly List<string> cellValues;

            public IReadOnlyCollection<string> ColumnNames => new ReadOnlyCollection<string>(sourceEnumerator.ColumnNames);

            public DelimitedValuesReaderRow([NotNull] DelimitedValuesEnumerator sourceEnumerator, [NotNull] [ItemNotNull] List<string> cellValues)
            {
                this.sourceEnumerator = sourceEnumerator;
                this.cellValues = cellValues;
            }

            public IReadOnlyCollection<string> GetCells()
            {
                return new ReadOnlyCollection<string>(cellValues);
            }

            public string GetCell(string columnName)
            {
                Guard.NotNullNorEmpty(columnName, nameof(columnName));

                int index = sourceEnumerator.ColumnNames.FindIndex(c => c == columnName);

                if (index == -1)
                {
                    throw new ArgumentException($"Column with name '{columnName}' does not exist.");
                }

                return cellValues[index];
            }

            public T GetCell<T>(string columnName, Converter<string, T> converter = null)
            {
                Guard.NotNullNorEmpty(columnName, nameof(columnName));

                string cellValue = GetCell(columnName);
                return converter != null ? converter(cellValue) : ConvertCell<T>(cellValue);
            }

            [CanBeNull]
            private T ConvertCell<T>([NotNull] string cellValue)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                object converted = converter.ConvertFrom(NullContext, sourceEnumerator.EffectiveCulture, cellValue);
                return (T)converted;
            }
        }
    }
}
