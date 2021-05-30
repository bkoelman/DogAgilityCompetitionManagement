using System.Threading;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Wraps a nullable <see cref="System.Boolean" />, where reading and writing the wrapped value always atomically returns the latest value.
    /// </summary>
    /// <remarks>
    /// It is strongly recommended to mark <see cref="FreshNullableBoolean" /> members in your class as <c>readonly</c>, because accidentally replacing a
    /// FreshNullableBoolean object with another FreshNullableBoolean object defeats the whole purpose of this class.
    /// </remarks>
    public sealed class FreshNullableBoolean
    {
        private const int TrueValue = 1;
        private const int FalseValue = 0;
        private const int NullValue = -1;

        private int innerValue;

        public bool? Value
        {
            get
            {
                int intValue = Interlocked.CompareExchange(ref innerValue, 999, 999);
                return FromInt32(intValue);
            }
            set
            {
                int intValue = ToInt32(value);
                Interlocked.Exchange(ref innerValue, intValue);
            }
        }

        public FreshNullableBoolean(bool? value)
        {
            Value = value;
        }

        public bool? CompareExchange(bool? newValue, bool? comparand)
        {
            int newIntValue = ToInt32(newValue);
            int comparandInt = ToInt32(comparand);
            int previousIntValue = Interlocked.CompareExchange(ref innerValue, newIntValue, comparandInt);
            return FromInt32(previousIntValue);
        }

        private static bool? FromInt32(int value)
        {
            return value == TrueValue ? true : value == FalseValue ? false : null;
        }

        private static int ToInt32(bool? value)
        {
            return value == true ? TrueValue : value == false ? FalseValue : NullValue;
        }
    }
}
