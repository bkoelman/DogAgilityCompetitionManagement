using System.Threading;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Holds a nullable or non-nullable reference to an object, where reading and writing the wrapped value always atomically returns the latest value.
    /// </summary>
    /// <typeparam name="T">
    /// The nullable or non-nullable type of the wrapped object reference.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// It is strongly recommended to mark <see cref="FreshObjectReference{T}" /> members in your class as <c>readonly</c>, because accidentally replacing a
    /// FreshObjectReference object with another FreshObjectReference object defeats the whole purpose of this class.
    /// </para>
    /// <para>
    /// Note that <see cref="FreshObjectReference{T}" /> only guards non-cached and atomic exchange of the wrapped object reference. If you need to access
    /// members of the wrapped object reference non-cached or atomically, locking is probably a better solution.
    /// </para>
    /// </remarks>
    public sealed class FreshObjectReference<T>
        where T : class?
    {
        private T innerValue;

        public T Value
        {
            get => Interlocked.CompareExchange(ref innerValue!, null!, null!);
            set => Interlocked.Exchange(ref innerValue, value);
        }

        public FreshObjectReference(T value)
        {
            innerValue = value;
            Value = value;
        }

        public T Exchange(T newValue)
        {
            return Interlocked.Exchange(ref innerValue, newValue);
        }
    }
}
