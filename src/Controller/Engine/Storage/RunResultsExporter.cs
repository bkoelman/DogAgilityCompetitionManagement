using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage.FileFormats;
using DogAgilityCompetition.Controller.Properties;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// Exports competitors with optional run results to a delimited file.
    /// </summary>
    public static class RunResultsExporter
    {
        private static readonly ReadOnlyCollection<string> ExportColumnNames = new List<string>
        {
            ImportExportColumns.CompetitorNumber,
            ImportExportColumns.HandlerName,
            ImportExportColumns.DogName,
            ImportExportColumns.CountryCode,
            ImportExportColumns.IntermediateTime1,
            ImportExportColumns.IntermediateTime2,
            ImportExportColumns.IntermediateTime3,
            ImportExportColumns.FinishTime,
            ImportExportColumns.FaultCount,
            ImportExportColumns.RefusalCount,
            ImportExportColumns.IsEliminated,
            ImportExportColumns.Placement
        }.AsReadOnly();

        public static void ExportTo(string path, IEnumerable<CompetitionRunResult> runResults)
        {
            Guard.NotNullNorEmpty(path, nameof(path));
            Guard.NotNull(runResults, nameof(runResults));

            using var textWriter = new StreamWriter(path);

            var settings = new DelimitedValuesWriterSettings
            {
                Culture = Settings.Default.ImportExportCulture
            };

            using var valuesWriter = new DelimitedValuesWriter(textWriter, ExportColumnNames, settings);

            foreach (CompetitionRunResult runResult in runResults)
            {
                using IDelimitedValuesWriterRow row = valuesWriter.CreateRow();

                row.SetCell(ImportExportColumns.CompetitorNumber, runResult.Competitor.Number);
                row.SetCell(ImportExportColumns.HandlerName, runResult.Competitor.HandlerName);
                row.SetCell(ImportExportColumns.DogName, runResult.Competitor.DogName);
                row.SetCell(ImportExportColumns.CountryCode, runResult.Competitor.CountryCode);

                if (runResult.Timings != null)
                {
                    SetTimeElapsedSinceStart(row, ImportExportColumns.IntermediateTime1, runResult, runResult.Timings.IntermediateTime1);
                    SetTimeElapsedSinceStart(row, ImportExportColumns.IntermediateTime2, runResult, runResult.Timings.IntermediateTime2);
                    SetTimeElapsedSinceStart(row, ImportExportColumns.IntermediateTime3, runResult, runResult.Timings.IntermediateTime3);
                    SetTimeElapsedSinceStart(row, ImportExportColumns.FinishTime, runResult, runResult.Timings.FinishTime);
                }

                if (runResult.HasCompleted)
                {
                    row.SetCell(ImportExportColumns.FaultCount, runResult.FaultCount);
                    row.SetCell(ImportExportColumns.RefusalCount, runResult.RefusalCount);
                    row.SetCell(ImportExportColumns.IsEliminated, runResult.IsEliminated);
                    row.SetCell(ImportExportColumns.Placement, runResult.PlacementText);
                }
            }
        }

        private static void SetTimeElapsedSinceStart(IDelimitedValuesWriterRow row, string columnName, CompetitionRunResult runResult, RecordedTime? time)
        {
            TimeSpanWithAccuracy? elapsed = GetElapsedSinceStart(runResult.Timings, time);

            if (elapsed != null)
            {
                row.SetCell(columnName, elapsed.Value);
            }
        }

        private static TimeSpanWithAccuracy? GetElapsedSinceStart(CompetitionRunTimings? timings, RecordedTime? time)
        {
            return timings != null && time != null ? time.ElapsedSince(timings.StartTime) : null;
        }
    }
}
