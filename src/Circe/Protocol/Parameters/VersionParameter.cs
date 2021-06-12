using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Parameters
{
    /// <summary>
    /// Represents a parameter whose value indicates a version number.
    /// </summary>
    public sealed class VersionParameter : Parameter
    {
        private const int MaxCharCount = 11;

        private static readonly Regex ValueFormatRegex = new(@"^(?<Major>[0-9]{1,3})\.(?<Minor>[0-9]{1,3})\.(?<Release>[0-9]{1,3})$", RegexOptions.Compiled);

        private Version? innerValue;

        /// <summary>
        /// Gets or sets the value of this parameter.
        /// </summary>
        public Version? Value
        {
            get => innerValue;
            set
            {
                if (value != null)
                {
                    AssertVersionFormatIsValid(value);
                }

                innerValue = value;
            }
        }

        /// <summary>
        /// Indicates whether the value of this parameter has been set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this parameter has a value; otherwise, <c>false</c>.
        /// </value>
        public override bool HasValue => innerValue != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionParameter" /> class.
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
        public VersionParameter(string name, int id, bool isRequired)
            : base(name, id, MaxCharCount, isRequired)
        {
        }

        [AssertionMethod]
        private void AssertVersionFormatIsValid(Version value)
        {
            int majorNumber = value.Major;
            int minorNumber = value.Minor;
            int releaseNumber = value.Build;

            string valueString = $"{majorNumber}.{minorNumber}.{releaseNumber}";

            if (!ValueFormatRegex.IsMatch(valueString))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Value of {GetType().Name} {Name} must be in format XXX.YYY.ZZZ.");
            }
        }

        /// <summary>
        /// When implemented by a class, exports the value of this parameter to binary format.
        /// </summary>
        /// <returns>
        /// The exported binary value of this parameter.
        /// </returns>
        public override byte[] ExportValue()
        {
            if (innerValue == null)
            {
                throw new InvalidOperationException($"{GetType().Name} {Name} has no value.");
            }

            string valueString = $"{innerValue.Major}.{innerValue.Minor}.{innerValue.Build}";
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
            string valueString = new(chars);

            Match match = ValueFormatRegex.Match(valueString);

            if (!match.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Value of {GetType().Name} {Name} must be in format XXX.YYY.ZZZ.");
            }

            int majorNumber = int.Parse(match.Groups["Major"].Value, CultureInfo.InvariantCulture);
            int minorNumber = int.Parse(match.Groups["Minor"].Value, CultureInfo.InvariantCulture);
            int releaseNumber = int.Parse(match.Groups["Release"].Value, CultureInfo.InvariantCulture);
            Value = new Version(majorNumber, minorNumber, releaseNumber);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current <see cref="object" />.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents the current <see cref="object" />.
        /// </returns>
        [Pure]
        public override string ToString()
        {
            return innerValue != null ? base.ToString() + ": " + innerValue.ToString(3) : base.ToString();
        }
    }
}
