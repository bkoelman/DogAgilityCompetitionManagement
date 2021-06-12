using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.MediatorEmulator
{
    /// <summary>
    /// Provides typed access to command-line arguments.
    /// </summary>
    public sealed class StartupArguments
    {
        public string? Path { get; }
        public Point? Location { get; }
        public Size? Size { get; }
        public FormWindowState State { get; }
        public bool TransparentOnTop { get; }

        public bool HasLayout => Location != null || Size != null || State != FormWindowState.Normal || TransparentOnTop;

        private StartupArguments(string? path, Point? location, Size? size, FormWindowState state, bool transparentOnTop)
        {
            Path = path;
            Location = location;
            Size = size;
            State = state;
            TransparentOnTop = transparentOnTop;
        }

        public static StartupArguments Parse(IEnumerable<string> args)
        {
            Guard.NotNull(args, nameof(args));

            string? path = null;
            Point? location = null;
            Size? size = null;
            FormWindowState? state = null;
            bool? transparentOnTop = null;

            foreach (string arg in args)
            {
                if (arg.StartsWith("pos=", StringComparison.OrdinalIgnoreCase))
                {
                    location = ParseLocation(arg.Substring("pos=".Length));
                }

                if (arg.StartsWith("size=", StringComparison.OrdinalIgnoreCase))
                {
                    size = ParseSize(arg.Substring("size=".Length));
                }
                else if (arg.StartsWith("state=", StringComparison.OrdinalIgnoreCase))
                {
                    state = ParseWindowState(arg.Substring("state=".Length));
                }
                else if (string.Compare(arg, "transparentOnTop", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    transparentOnTop = true;
                }
                else if (!arg.Contains('=', StringComparison.Ordinal))
                {
                    if (path != null)
                    {
                        throw new InvalidOperationException("Multiple paths are not supported.");
                    }

                    path = arg;
                }
            }

            return new StartupArguments(path, location, size, state ?? FormWindowState.Normal, transparentOnTop == true);
        }

        private static Point ParseLocation(string value)
        {
            int[]? parts = TrySplitIntoTwoCoordinates(value);

            if (parts != null)
            {
                return new Point(parts[1], parts[0]);
            }

            throw new Exception("Specify position as top x left, for example: 10x15");
        }

        private static Size ParseSize(string value)
        {
            int[]? parts = TrySplitIntoTwoCoordinates(value);

            if (parts != null)
            {
                return new Size(parts[1], parts[0]);
            }

            throw new Exception("Specify size as height x width, for example: 250x300");
        }

        private static int[]? TrySplitIntoTwoCoordinates(string value)
        {
            string[] args = value.Split('x');

            if (args.Length == 2)
            {
                if (int.TryParse(args[0], out int value0) && int.TryParse(args[1], out int value1))
                {
                    return new[]
                    {
                        value0,
                        value1
                    };
                }
            }

            return null;
        }

        private static FormWindowState ParseWindowState(string value)
        {
            return (FormWindowState)Enum.Parse(typeof(FormWindowState), value, true);
        }
    }
}
