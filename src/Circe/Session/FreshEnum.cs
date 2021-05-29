using System;
using System.Globalization;
using System.Threading;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Wraps a <see cref="System.Enum" />, where reading and writing the wrapped value always atomically returns the latest value.
    /// </summary>
    /// <remarks>
    /// It is strongly recommended to mark <see cref="FreshEnum{T}" /> members in your class as <c>readonly</c>, because accidentally replacing a FreshEnum
    /// object with another FreshEnum object defeats the whole purpose of this class.
    /// </remarks>
    public sealed class FreshEnum<T>
        where T : struct
    {
        private long innerValue;

        public T Value
        {
            get
            {
                long longValue = Interlocked.CompareExchange(ref innerValue, 999, 999);
                return FromInt64(longValue);
            }
            set
            {
                long longValue = ToInt64(value);
                Interlocked.Exchange(ref innerValue, longValue);
            }
        }

        public FreshEnum(T value)
        {
            Value = value;
        }

        private static T FromInt64(long value)
        {
            return (T)Enum.Parse(typeof(T), value.ToString(CultureInfo.InvariantCulture));
        }

        private static long ToInt64(T value)
        {
            return Convert.ToInt64(value);
        }
    }
}
