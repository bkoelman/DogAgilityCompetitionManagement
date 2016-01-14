using System;
using System.Text;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// Information about a competitor person and his/her dog.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class Competitor : IEquatable<Competitor>
    {
        private static readonly int CompetitorNumberMaximumValue =
            int.Parse(new string('9', NumberEntryFilter.MaxCompetitorNumberLength));

        public int Number { get; }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public string DogName { get; }

        [CanBeNull]
        public string CountryCode { get; }

        public Competitor(int number, [NotNull] string name, [NotNull] string dogName)
            : this(number, name, dogName, null)
        {
        }

        private Competitor(int number, [NotNull] string name, [NotNull] string dogName, [CanBeNull] string countryCode)
        {
            Guard.InRangeInclusive(number, nameof(number), 1, CompetitorNumberMaximumValue);
            Guard.NotNullNorWhiteSpace(name, nameof(name));
            Guard.NotNullNorWhiteSpace(dogName, nameof(dogName));

            Number = number;
            Name = name;
            DogName = dogName;
            CountryCode = string.IsNullOrWhiteSpace(countryCode) ? null : countryCode;
        }

        [NotNull]
        public Competitor ChangeCountryCode([CanBeNull] string countryCode)
        {
            return new Competitor(Number, Name, DogName, countryCode);
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();
            textBuilder.Append("Competitor ");
            textBuilder.Append(Name);
            textBuilder.Append(" (");
            textBuilder.Append(Number);
            textBuilder.Append(") with ");
            textBuilder.Append(DogName);

            if (!string.IsNullOrEmpty(CountryCode))
            {
                textBuilder.Append(" from ");
                textBuilder.Append(CountryCode);
            }

            return textBuilder.ToString();
        }

        public bool Equals([CanBeNull] Competitor other)
        {
            return !ReferenceEquals(other, null) && other.Number == Number && other.Name == Name &&
                other.DogName == DogName && other.CountryCode == CountryCode;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return Equals(obj as Competitor);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode() ^ Name.GetHashCode() ^ DogName.GetHashCode() ^
                (CountryCode ?? string.Empty).GetHashCode();
        }

        public static bool operator ==([CanBeNull] Competitor left, [CanBeNull] Competitor right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(left, null))
            {
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=([CanBeNull] Competitor left, [CanBeNull] Competitor right)
        {
            return !(left == right);
        }
    }
}