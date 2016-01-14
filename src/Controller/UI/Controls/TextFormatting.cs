using System;
using System.Text;
using DogAgilityCompetition.Controller.Engine;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Support for formatting numeric and time values.
    /// </summary>
    public static class TextFormatting
    {
        [NotNull]
        public static string FormatCompetitorNumber([CanBeNull] int? number)
        {
            return FormatNumber(number, NumberEntryFilter.MaxCompetitorNumberLength);
        }

        [NotNull]
        public static string FormatPlacement(int placement)
        {
            int? value = placement > 0 ? placement : (int?) null;
            return FormatNumber(value, 3);
        }

        [NotNull]
        public static string FormatNumber([CanBeNull] int? number, int digitCount)
        {
            if (number == null)
            {
                return string.Empty;
            }

            string formatterZeroes = new string('0', digitCount);

            var formatBuilder = new StringBuilder();
            formatBuilder.Append("{0:");
            formatBuilder.Append(formatterZeroes);
            formatBuilder.Append("}");

            return string.Format(formatBuilder.ToString(), number);
        }

        [NotNull]
        public static string FormatTime([CanBeNull] TimeSpan? time)
        {
            if (time == null)
            {
                return "XXX.XXX";
            }

            double seconds = Math.Truncate(time.Value.TotalSeconds);
            return $"{seconds:000}.{time.Value.Milliseconds:000}";
        }
    }
}