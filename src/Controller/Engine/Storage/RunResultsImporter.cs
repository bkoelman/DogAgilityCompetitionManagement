using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Controller.Properties;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// Imports competitors with optional run results from a delimited file.
    /// </summary>
    public sealed class RunResultsImporter
    {
        [NotNull]
        [ItemNotNull]
        private static readonly IReadOnlyCollection<string> RequiredColumnNames = new List<string>
        {
            ImportExportColumns.CompetitorNumber,
            ImportExportColumns.HandlerName,
            ImportExportColumns.DogName,
            ImportExportColumns.CountryCode
        }.AsReadOnly();

        [NotNull]
        [ItemNotNull]
        private static readonly IReadOnlyCollection<string> OptionalColumnNames = new List<string>
        {
            ImportExportColumns.IntermediateTime1,
            ImportExportColumns.IntermediateTime2,
            ImportExportColumns.IntermediateTime3,
            ImportExportColumns.FinishTime,
            ImportExportColumns.FaultCount,
            ImportExportColumns.RefusalCount,
            ImportExportColumns.IsEliminated
        }.AsReadOnly();

        [NotNull]
        [ItemNotNull]
        private readonly List<CompetitionRunResult> existingRunResults;

        public RunResultsImporter([NotNull] [ItemNotNull] IEnumerable<CompetitionRunResult> existingRunResults)
        {
            Guard.NotNull(existingRunResults, nameof(existingRunResults));

            this.existingRunResults = existingRunResults.ToList();
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<CompetitionRunResult> ImportFrom([NotNull] string path, bool mergeDeletes, bool skipTimings)
        {
            Guard.NotNullNorEmpty(path, nameof(path));

            var mergeRunResults = new List<CompetitionRunResult>();

            Dictionary<int, CompetitionRunResult> importRunResults = ImportRunResultsFrom(path);

            foreach (CompetitionRunResult existingRunResult in existingRunResults)
            {
                if (importRunResults.ContainsKey(existingRunResult.Competitor.Number))
                {
                    // Merge action: Content merge
                    CompetitionRunResult importRunResult = importRunResults[existingRunResult.Competitor.Number];
                    CompetitionRunResult mergeRunResult = Merge(existingRunResult, importRunResult, skipTimings);

                    mergeRunResults.Add(mergeRunResult);
                    importRunResults.Remove(existingRunResult.Competitor.Number);
                }
                else
                {
                    if (mergeDeletes)
                    {
                        // Merge action: Delete
                    }
                    else
                    {
                        // Merge action: None
                        mergeRunResults.Add(existingRunResult);
                    }
                }
            }

            // Merge action: Add
            mergeRunResults.AddRange(importRunResults.Select(importRunResult => importRunResult.Value));

            return mergeRunResults;
        }

        [NotNull]
        private static Dictionary<int, CompetitionRunResult> ImportRunResultsFrom([NotNull] string path)
        {
            DateTime currentTimeUtc = SystemContext.UtcNow();
            var imported = new Dictionary<int, CompetitionRunResult>();

            using (var textReader = new StreamReader(path))
            {
                var settings = new DelimitedValuesReaderSettings
                {
                    Culture = Settings.Default.ImportExportCulture
                };

                using (var valuesReader = new DelimitedValuesReader(textReader, settings))
                {
                    AssertRequiredColumnsExist(valuesReader);
                    bool hasOptionalColumns = ContainsOptionalColumnNames(valuesReader);

                    foreach (IDelimitedValuesReaderRow row in valuesReader)
                    {
                        try
                        {
                            CompetitionRunResult runResult = GetRunResultFrom(row, hasOptionalColumns, currentTimeUtc);
                            imported[runResult.Competitor.Number] = runResult;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Failed to read import file at row {valuesReader.LineNumber}: {ex.Message}", ex);
                        }
                    }
                }
            }

            return imported;
        }

        [AssertionMethod]
        private static void AssertRequiredColumnsExist([NotNull] [ItemNotNull] DelimitedValuesReader reader)
        {
            List<string> missingColumnNames = RequiredColumnNames.Where(requiredColumnName => !reader.ColumnNames.Contains(requiredColumnName)).ToList();

            if (missingColumnNames.Count > 0)
            {
                throw new Exception($"Missing column names: {string.Join(", ", missingColumnNames)}.");
            }
        }

        private static bool ContainsOptionalColumnNames([NotNull] [ItemNotNull] DelimitedValuesReader reader)
        {
            return OptionalColumnNames.All(optionalColumnName => reader.ColumnNames.Contains(optionalColumnName));
        }

        [NotNull]
        private static CompetitionRunResult GetRunResultFrom([NotNull] IDelimitedValuesReaderRow row, bool hasOptionalColumns, DateTime startTimeUtc)
        {
            Competitor competitor = GetCompetitorFrom(row);
            var runResult = new CompetitionRunResult(competitor);

            if (hasOptionalColumns)
            {
                int? faultCount = row.GetCell<int?>(ImportExportColumns.FaultCount);
                int? refusalCount = row.GetCell<int?>(ImportExportColumns.RefusalCount);
                bool? isEliminated = row.GetCell<bool?>(ImportExportColumns.IsEliminated);

                // Note: We cannot reconstruct whether start time was high precision, because it does not roundtrip
                // through import/export. However, we do not need to know. A low-precision elapsed time is caused by 
                // either one or both times to be low precision. So although we lost some information, the nett effect
                // when the precision of an elapsed time is recalculated will be the same as long as we assume that the start
                // time was high precision.
                var startTime = new RecordedTime(TimeSpan.Zero, startTimeUtc);

                RecordedTime intermediateTime1 = GetTimeElapsedSinceStart(row, ImportExportColumns.IntermediateTime1, startTime);
                RecordedTime intermediateTime2 = GetTimeElapsedSinceStart(row, ImportExportColumns.IntermediateTime2, startTime);
                RecordedTime intermediateTime3 = GetTimeElapsedSinceStart(row, ImportExportColumns.IntermediateTime3, startTime);
                RecordedTime finishTime = GetTimeElapsedSinceStart(row, ImportExportColumns.FinishTime, startTime);

                if (intermediateTime1 == null && intermediateTime2 == null && intermediateTime3 == null && finishTime == null)
                {
                    startTime = null;
                }

                bool runCompleted = finishTime != null || (isEliminated != null && isEliminated.Value);

                if (runCompleted)
                {
                    if (faultCount != null)
                    {
                        runResult = runResult.ChangeFaultCount(faultCount.Value);
                    }

                    if (refusalCount != null)
                    {
                        runResult = runResult.ChangeRefusalCount(refusalCount.Value);
                    }

                    if (isEliminated != null)
                    {
                        runResult = runResult.ChangeIsEliminated(isEliminated.Value);
                    }

                    if (startTime != null)
                    {
                        // @formatter:keep_existing_linebreaks true

                        runResult = runResult
                            .ChangeTimings(new CompetitionRunTimings(startTime)
                                .ChangeIntermediateTime1(intermediateTime1)
                                .ChangeIntermediateTime2(intermediateTime2)
                                .ChangeIntermediateTime3(intermediateTime3)
                                .ChangeFinishTime(finishTime));

                        // @formatter:keep_existing_linebreaks restore
                    }
                }
            }

            return runResult;
        }

        [NotNull]
        private static Competitor GetCompetitorFrom([NotNull] IDelimitedValuesReaderRow row)
        {
            int competitorNumber = row.GetCell<int>(ImportExportColumns.CompetitorNumber);
            string handlerName = row.GetCell<string>(ImportExportColumns.HandlerName);
            string dogName = row.GetCell<string>(ImportExportColumns.DogName);
            string countryCode = row.GetCell<string>(ImportExportColumns.CountryCode);

            if (string.IsNullOrWhiteSpace(handlerName))
            {
                throw new InvalidDataException("Handler name is missing.");
            }

            if (string.IsNullOrWhiteSpace(dogName))
            {
                throw new InvalidDataException("Dog name is missing.");
            }

            return new Competitor(competitorNumber, handlerName, dogName).ChangeCountryCode(countryCode);
        }

        [CanBeNull]
        private static RecordedTime GetTimeElapsedSinceStart([NotNull] IDelimitedValuesReaderRow row, [NotNull] string columnName,
            [NotNull] RecordedTime startTime)
        {
            string timeString = row.GetCell(columnName);
            TimeSpanWithAccuracy? timeWithAccuracy = TimeSpanWithAccuracy.FromString(timeString, Settings.Default.ImportExportCulture);

            return timeWithAccuracy != null ? startTime.Add(timeWithAccuracy.Value) : null;
        }

        [NotNull]
        private static CompetitionRunResult Merge([NotNull] CompetitionRunResult existing, [NotNull] CompetitionRunResult imported, bool skipTimings)
        {
            return skipTimings ? existing.ChangeCompetitor(imported.Competitor) : MergeWithTimings();
        }

        [NotNull]
        private static CompetitionRunResult MergeWithTimings()
        {
            // NICE-TO-HAVE: Add merge support for timings.
            // Q: How is this expected to work? Maybe forget about doing this at all?
            throw new NotImplementedException();
        }
    }
}
