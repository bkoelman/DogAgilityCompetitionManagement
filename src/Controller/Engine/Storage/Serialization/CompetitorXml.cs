using System.IO;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="Competitor" />.
    /// </summary>
    [DataContract(Namespace = "")]
    public sealed class CompetitorXml
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        [CanBeNull]
        public string HandlerName { get; set; }

        [DataMember]
        [CanBeNull]
        public string DogName { get; set; }

        [DataMember]
        [CanBeNull]
        public string CountryCode { get; set; }

        [NotNull]
        public static CompetitorXml ToXmlObject([NotNull] Competitor source)
        {
            Guard.NotNull(source, nameof(source));

            return new CompetitorXml
            {
                Number = source.Number,
                HandlerName = source.HandlerName,
                DogName = source.DogName,
                CountryCode = source.CountryCode
            };
        }

        [NotNull]
        public static Competitor FromXmlObject([NotNull] CompetitorXml source)
        {
            Guard.NotNull(source, nameof(source));
            string name = AssertHandlerNameNotEmpty(source);
            string dogName = AssertDogNameNotEmpty(source);

            return new Competitor(source.Number, name, dogName).ChangeCountryCode(source.CountryCode);
        }

        [AssertionMethod]
        [NotNull]
        private static string AssertHandlerNameNotEmpty([NotNull] CompetitorXml source)
        {
            if (string.IsNullOrWhiteSpace(source.HandlerName))
            {
                throw new InvalidDataException("Competitor handler name is missing or empty in XML file.");
            }
            return source.HandlerName;
        }

        [AssertionMethod]
        [NotNull]
        private static string AssertDogNameNotEmpty([NotNull] CompetitorXml source)
        {
            if (string.IsNullOrWhiteSpace(source.DogName))
            {
                throw new InvalidDataException("Competitor dog name is missing or empty in XML file.");
            }
            return source.DogName;
        }
    }
}