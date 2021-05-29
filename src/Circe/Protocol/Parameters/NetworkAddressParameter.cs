using System;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Parameters
{
    /// <summary>
    /// Represents a parameter whose value indicates a wireless network address.
    /// </summary>
    [Serializable]
    public sealed class NetworkAddressParameter : Parameter
    {
        private const int CharCount = 6;

        [NotNull]
        private static readonly Regex ValueFormatRegex = new("^[0-9A-F]{" + CharCount + "}$", RegexOptions.Compiled);

        [CanBeNull]
        private string innerValue;

        /// <summary>
        /// Gets or sets the value of this parameter.
        /// </summary>
        [CanBeNull]
        public string Value
        {
            get => innerValue;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    innerValue = null;
                }
                else
                {
                    if (!ValueFormatRegex.IsMatch(value))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), value,
                            $"Value of {GetType().Name} {Name} must consist of {CharCount} characters in range 0-9 or A-F.");
                    }

                    innerValue = value;
                }
            }
        }

        /// <summary>
        /// Indicates whether the value of this parameter has been set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this parameter has a value; otherwise, <c>false</c>.
        /// </value>
        public override bool HasValue => !string.IsNullOrEmpty(innerValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkAddressParameter" /> class.
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
        public NetworkAddressParameter([NotNull] string name, int id, bool isRequired)
            : base(name, id, CharCount, isRequired)
        {
        }

        [CanBeNull]
        public string GetValueOrNull()
        {
            return HasValue ? Value : null;
        }

        /// <summary>
        /// Exports the value of this parameter to binary format.
        /// </summary>
        /// <returns>
        /// The exported binary value of this parameter.
        /// </returns>
        public override byte[] ExportValue()
        {
            if (!HasValue || innerValue == null)
            {
                throw new InvalidOperationException($"{GetType().Name} {Name} has no value.");
            }

            return Encoding.ASCII.GetBytes(innerValue);
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
            Value = new string(chars);
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
            return HasValue ? base.ToString() + ": " + innerValue : base.ToString();
        }

        public static bool IsValidAddress([CanBeNull] string value)
        {
            return !string.IsNullOrEmpty(value) && ValueFormatRegex.IsMatch(value);
        }
    }
}
