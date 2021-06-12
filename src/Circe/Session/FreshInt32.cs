using System.Threading;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Wraps a <see cref="System.Int32" />, where reading and writing the wrapped value always atomically returns the latest value.
    /// </summary>
    /// <remarks>
    /// It is strongly recommended to mark <see cref="FreshInt32" /> members in your class as <c>readonly</c>, because accidentally replacing a FreshInt32
    /// object with another FreshInt32 object defeats the whole purpose of this class.
    /// </remarks>
    public sealed class FreshInt32
    {
        private int innerValue;

        public int Value
        {
            get => Interlocked.CompareExchange(ref innerValue, 0, 0);
            set => Interlocked.Exchange(ref innerValue, value);
        }

        public FreshInt32(int value)
        {
            Value = value;
        }
    }
}
