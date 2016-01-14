using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator
{
    /// <summary>
    /// Provides typed access to command-line arguments.
    /// </summary>
    public sealed class StartupArguments
    {
        [CanBeNull]
        public string Path { get; private set; }

        [CanBeNull]
        public Point? Location { get; }

        [CanBeNull]
        public Size? Size { get; }

        public FormWindowState State { get; }

        public bool TransparentOnTop { get; }

        public bool HasLayout => Location != null || Size != null || State != FormWindowState.Normal || TransparentOnTop
            ;

        private StartupArguments([CanBeNull] string path, [CanBeNull] Point? location, [CanBeNull] Size? size,
            FormWindowState state, bool transparentOnTop)
        {
            Path = path;
            Location = location;
            Size = size;
            State = state;
            TransparentOnTop = transparentOnTop;
        }

        [NotNull]
        public static StartupArguments Parse([NotNull] [ItemNotNull] IEnumerable<string> args)
        {
            Guard.NotNull(args, nameof(args));

            string path = null;
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
                else if (arg.IndexOf('=') == -1)
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

        private static Point ParseLocation([NotNull] string value)
        {
            int[] parts = TrySplitIntoTwoCoords(value);
            if (parts != null)
            {
                return new Point(parts[1], parts[0]);
            }

            throw new Exception("Specify position as top x left, for example: 10x15");
        }

        private static Size ParseSize([NotNull] string value)
        {
            int[] parts = TrySplitIntoTwoCoords(value);
            if (parts != null)
            {
                return new Size(parts[1], parts[0]);
            }

            throw new Exception("Specify size as height x width, for example: 250x300");
        }

        [CanBeNull]
        private static int[] TrySplitIntoTwoCoords([NotNull] string value)
        {
            string[] args = value.Split('x');
            if (args.Length == 2)
            {
                int value0;
                int value1;
                if (int.TryParse(args[0], out value0) && int.TryParse(args[1], out value1))
                {
                    return new[] { value0, value1 };
                }
            }
            return null;
        }

        private static FormWindowState ParseWindowState([NotNull] string value)
        {
            return (FormWindowState) Enum.Parse(typeof (FormWindowState), value, true);
        }
    }
}