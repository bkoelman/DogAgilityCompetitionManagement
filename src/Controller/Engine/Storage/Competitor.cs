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
        public string HandlerName { get; }

        [NotNull]
        public string DogName { get; }

        [CanBeNull]
        public string CountryCode { get; }

        public Competitor(int number, [NotNull] string handlerName, [NotNull] string dogName)
            : this(number, handlerName, dogName, null)
        {
        }

        private Competitor(int number, [NotNull] string handlerName, [NotNull] string dogName,
            [CanBeNull] string countryCode)
        {
            Guard.InRangeInclusive(number, nameof(number), 1, CompetitorNumberMaximumValue);
            Guard.NotNullNorWhiteSpace(handlerName, nameof(handlerName));
            Guard.NotNullNorWhiteSpace(dogName, nameof(dogName));

            Number = number;
            HandlerName = handlerName;
            DogName = dogName;
            CountryCode = string.IsNullOrWhiteSpace(countryCode) ? null : countryCode;
        }

        [NotNull]
        public Competitor ChangeCountryCode([CanBeNull] string countryCode)
        {
            return new Competitor(Number, HandlerName, DogName, countryCode);
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();
            textBuilder.Append("Competitor ");
            textBuilder.Append(HandlerName);
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
            return !ReferenceEquals(other, null) && other.Number == Number && other.HandlerName == HandlerName &&
                other.DogName == DogName && other.CountryCode == CountryCode;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return Equals(obj as Competitor);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode() ^ HandlerName.GetHashCode() ^ DogName.GetHashCode() ^
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