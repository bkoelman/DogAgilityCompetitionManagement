using System;
using System.Text;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Parameters
{
    /// <summary>
    /// Represents a parameter whose value indicates either <c>true</c> or <c>false</c>.
    /// </summary>
    [Serializable]
    public sealed class BooleanParameter : Parameter
    {
        /// <summary>
        /// Gets or sets the value of this parameter.
        /// </summary>
        [CanBeNull]
        public bool? Value { get; set; }

        /// <summary>
        /// Indicates whether the value of this parameter has been set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this parameter has a value; otherwise, <c>false</c>.
        /// </value>
        public override bool HasValue => Value != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParameter" /> class.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter.
        /// </param>
        /// <param name="id">
        /// The identifier of the parameter.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the parameter is required.
        /// </param>
        public BooleanParameter([NotNull] string name, int id, bool isRequired)
            : base(name, id, 1, isRequired)
        {
        }

        /// <summary>
        /// Exports the value of this parameter to binary format.
        /// </summary>
        /// <returns>
        /// The exported binary value of this parameter.
        /// </returns>
        public override byte[] ExportValue()
        {
            if (!HasValue)
            {
                throw new InvalidOperationException($"{GetType().Name} {Name} has no value.");
            }

            string valueString = Value == true ? "1" : "0";
            return Encoding.ASCII.GetBytes(valueString);
        }

        /// <summary>
        /// Imports the value of this parameter from binary format.
        /// </summary>
        /// <param name="value">
        /// The bytes that contain the value to import.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> do not represent a valid parameter value.
        /// </exception>
        public override void ImportValue(byte[] value)
        {
            base.ImportValue(value);

            char[] chars = Encoding.ASCII.GetChars(value);
            string text = new(chars);

            if (text != "0" && text != "1")
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Value of {GetType().Name} {Name} must be 0 or 1.");
            }

            Value = text != "0";
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        [Pure]
        public override string ToString()
        {
            return HasValue ? base.ToString() + ": " + Value : base.ToString();
        }
    }
}
