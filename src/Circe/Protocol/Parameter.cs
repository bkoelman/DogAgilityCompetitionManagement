using System;
using DogAgilityCompetition.Circe.Protocol.Exceptions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Represents the base class for a parameter inside a CIRCE <see cref="Operation" />.
    /// </summary>
    [Serializable]
    public abstract class Parameter
    {
        /// <summary>
        /// Gets the name of this parameter.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Gets the identifier of this parameter.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the maximum length of this parameter.
        /// </summary>
        [CanBeNull]
        public int? MaxLength { get; }

        /// <summary>
        /// Gets a value indicating whether this parameter is required in a CIRCE operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this parameter is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; }

        /// <summary>
        /// When implemented by a class, indicates whether the value of this parameter has been set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this parameter has a value; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter" /> class.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter.
        /// </param>
        /// <param name="id">
        /// The identifier of the parameter.
        /// </param>
        /// <param name="maxLength">
        /// The maximum length of the parameter.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the parameter is required.
        /// </param>
        protected Parameter([NotNull] string name, int id, [CanBeNull] int? maxLength, bool isRequired)
        {
            Guard.NotNullNorEmpty(name, nameof(name));
            Guard.InRangeInclusive(id, nameof(id), 1, 999);

            if (maxLength != null)
            {
                Guard.GreaterOrEqual(maxLength.Value, nameof(maxLength), 1);
            }

            Name = name;
            Id = id;
            MaxLength = maxLength;
            IsRequired = isRequired;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        [Pure]
        public override string ToString() => $"{GetType().Name} {Name} ({Id})";

        /// <summary>
        /// Validates the value of this parameter.
        /// </summary>
        /// <param name="owner">
        /// The operation that owns this parameter instance.
        /// </param>
        /// <exception cref="OperationValidationException" />
        public void Validate([NotNull] Operation owner)
        {
            Guard.NotNull(owner, nameof(owner));

            if (IsRequired && !HasValue)
            {
                throw new OperationValidationException(owner,
                    $"Required {GetType().Name} {Name} is missing or has no value.");
            }
        }

        /// <summary>
        /// When implemented by a class, exports the value of this parameter to binary format.
        /// </summary>
        /// <returns>
        /// The exported binary value of this parameter.
        /// </returns>
        [NotNull]
        public abstract byte[] ExportValue();

        /// <summary>
        /// When implemented by a class, imports the value of this parameter from binary format.
        /// </summary>
        /// <param name="value">
        /// The bytes that contain the value to import.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> do not represent a valid parameter value.
        /// </exception>
        public virtual void ImportValue([NotNull] byte[] value)
        {
            Guard.NotNullNorEmpty(value, nameof(value));

            if (MaxLength != null)
            {
                Guard.LessOrEqual(value.Length, nameof(value), MaxLength.Value);
            }
        }
    }
}