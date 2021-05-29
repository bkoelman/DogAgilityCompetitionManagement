using System;
using System.Threading;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Wraps a <see cref="System.DateTime" />, where reading and writing the wrapped value always atomically returns the latest value.
    /// </summary>
    /// <remarks>
    /// It is strongly recommended to mark <see cref="FreshDateTime" /> members in your class as <c>readonly</c>, because accidentally replacing a
    /// FreshDateTime object with another FreshDateTime object defeats the whole purpose of this class.
    /// </remarks>
    public sealed class FreshDateTime
    {
        private long innerValue;

        public DateTime Value
        {
            get
            {
                long result = Interlocked.CompareExchange(ref innerValue, 0, 0);
                return DateTime.FromBinary(result);
            }
            set
            {
                long result = value.ToBinary();
                Interlocked.Exchange(ref innerValue, result);
            }
        }

        public FreshDateTime(DateTime value)
        {
            Value = value;
        }
    }
}
