using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Holds a reference to an object, where reading and writing the wrapped value always atomically returns the latest value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the wrapped object reference.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// This class is identical to <see cref="FreshReference{T}" />, but having <see cref="Value" /> decorated with <see cref="NotNullAttribute" />.
    /// </para>
    /// </remarks>
    public sealed class FreshNotNullableReference<T> : FreshReference<T>
        where T : class
    {
        // ReSharper disable once AnnotationConflictInHierarchy
        // Reason: This class is more constrained than its base.
        [NotNull]
        public override T Value
        {
            get =>
                // ReSharper disable once AssignNullToNotNullAttribute
                // Reason: This class is more constrained than its base.
                base.Value;
            set
            {
                Guard.NotNull(value, nameof(value));
                base.Value = value;
            }
        }

        public FreshNotNullableReference([NotNull] T value)
            : base(value)
        {
            Guard.NotNull(value, nameof(value));
        }

        [NotNull]
        // ReSharper disable once AnnotationConflictInHierarchy
        // Reason: This class is more constrained than its base.
        public override T Exchange([NotNull] T newValue)
        {
            Guard.NotNull(newValue, nameof(newValue));

            // ReSharper disable once AssignNullToNotNullAttribute
            // Reason: This class is more constrained than its base.
            return base.Exchange(newValue);
        }
    }
}
