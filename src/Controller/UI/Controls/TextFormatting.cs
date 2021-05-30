using System;
using System.Text;
using DogAgilityCompetition.Controller.Engine;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Support for formatting numeric and time values.
    /// </summary>
    public static class TextFormatting
    {
        public static string FormatCompetitorNumber(int? number)
        {
            return FormatNumber(number, NumberEntryFilter.MaxCompetitorNumberLength);
        }

        public static string FormatPlacement(int placement)
        {
            int? value = placement > 0 ? placement : null;
            return FormatNumber(value, 3);
        }

        public static string FormatNumber(int? number, int digitCount)
        {
            if (number == null)
            {
                return string.Empty;
            }

            string formatterZeroes = new('0', digitCount);

            var formatBuilder = new StringBuilder();
            formatBuilder.Append("{0:");
            formatBuilder.Append(formatterZeroes);
            formatBuilder.Append("}");

            return string.Format(formatBuilder.ToString(), number);
        }

        public static string FormatTime(TimeSpan? time)
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
