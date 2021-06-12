using System.Linq;
using System.Text;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary>
    /// Supports writing truth tables in unit tests.
    /// </summary>
    internal sealed class OrderingScenario
    {
        public ulong Value { get; }
        public int BitCount { get; }
        public OrderExpect Result { get; }

        public bool this[int offset]
        {
            get
            {
                Guard.InRangeInclusive(offset, nameof(offset), 0, BitCount - 1);

                int shift = BitCount - offset - 1;
                return (Value & ((ulong)1 << shift)) != 0;
            }
        }

        public OrderingScenario(int bitCount, ulong value, OrderExpect result)
        {
            Guard.InRangeInclusive(bitCount, nameof(bitCount), 1, 64);

            ulong mask = ulong.MaxValue << bitCount;
            Guard.InRangeInclusive(value, nameof(value), 0, ~mask);

            BitCount = bitCount;
            Result = result;
            Value = value;
        }

        public static ulong FromBits(params int[] bits)
        {
            Guard.NotNull(bits, nameof(bits));
            Guard.InRangeInclusive(bits.Length, nameof(bits), 1, 64);

            return bits.Aggregate<int, ulong>(0, (current, bit) => (current << 1) | (bit != 0 ? (ulong)1 : 0));
        }

        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            for (int offset = 0; offset < BitCount; offset++)
            {
                int shift = BitCount - offset - 1;
                bool bitIsSet = (Value & ((ulong)1 << shift)) != 0;
                textBuilder.Append(bitIsSet ? '1' : '0');
            }

            // Add leading zeros when not whole nibbles.
            while (textBuilder.Length % 4 != 0)
            {
                textBuilder.Insert(0, '0');
            }

            // Insert dot (.) between nibbles.
            for (int index = textBuilder.Length - 4; index > 0; index -= 4)
            {
                textBuilder.Insert(index, '.');
            }

            return textBuilder.ToString();
        }
    }
}
