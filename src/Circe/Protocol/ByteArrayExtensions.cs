using System;
using System.Text;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary />
    public static class ByteArrayExtensions
    {
        private const int NumBytesPerLine = 0x10;

        /// <summary>
        /// Formats the specified buffer into human-readable text using hexadecimal notation.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to format.
        /// </param>
        /// <param name="indent">
        /// The number of spaces to indent lines with.
        /// </param>
        /// <returns>
        /// The human-readable formatted text.
        /// </returns>
        [Pure]
        public static string FormatHexBuffer(this byte[] buffer, int indent = 2)
        {
            Guard.NotNull(buffer, nameof(buffer));

            var segment = new ArraySegment<byte>(buffer);
            return FormatHexBuffer(segment, indent);
        }

        /// <summary>
        /// Formats the specified buffer into human-readable text using hexadecimal notation.
        /// </summary>
        /// <param name="segment">
        /// The buffer to format.
        /// </param>
        /// <param name="indent">
        /// The number of spaces to indent lines with.
        /// </param>
        /// <returns>
        /// The human-readable formatted text.
        /// </returns>
        [Pure]
        public static string FormatHexBuffer(this ArraySegment<byte> segment, int indent = 2)
        {
            string lineIndent = new(' ', indent);
            var linesBuilder = new StringBuilder();

            var hexBuilder = new StringBuilder(NumBytesPerLine * 3 + 1);
            var charBuilder = new StringBuilder(NumBytesPerLine + 1);

            int index = segment.Offset;

            while (index < segment.Offset + segment.Count)
            {
                string header = $"{index:X8}  ";

                hexBuilder.Length = 0;
                charBuilder.Length = 0;

                for (int column = 0; column < NumBytesPerLine; column++, index++)
                {
                    if (column == NumBytesPerLine / 2)
                    {
                        hexBuilder.Append(' ');
                        charBuilder.Append(' ');
                    }

                    if (index < segment.Offset + segment.Count)
                    {
                        string hexValue = $"{segment.Array![index]:X2} ";
                        hexBuilder.Append(hexValue);

                        char ch = segment.Array[index] < 0x20 || segment.Array[index] > 0x7E ? '.' : (char)segment.Array[index];
                        charBuilder.Append(ch);
                    }
                    else
                    {
                        hexBuilder.Append("   ");
                        charBuilder.Append(' ');
                    }
                }

                linesBuilder.AppendLine();
                linesBuilder.Append(lineIndent);
                linesBuilder.Append(header);
                linesBuilder.Append(hexBuilder);
                linesBuilder.Append(' ');
                linesBuilder.Append(charBuilder);
            }

            return linesBuilder.ToString();
        }
    }
}
