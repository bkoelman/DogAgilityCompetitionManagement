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
        public string? HandlerName { get; set; }

        [DataMember]
        public string? DogName { get; set; }

        [DataMember]
        public string? CountryCode { get; set; }

        public static CompetitorXml ToXmlObject(Competitor source)
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

        public static Competitor FromXmlObject(CompetitorXml source)
        {
            Guard.NotNull(source, nameof(source));
            string name = AssertHandlerNameNotEmpty(source);
            string dogName = AssertDogNameNotEmpty(source);

            return new Competitor(source.Number, name, dogName).ChangeCountryCode(source.CountryCode);
        }

        [AssertionMethod]
        private static string AssertHandlerNameNotEmpty(CompetitorXml source)
        {
            if (string.IsNullOrWhiteSpace(source.HandlerName))
            {
                throw new InvalidDataException("Competitor handler name is missing or empty in XML file.");
            }

            return source.HandlerName;
        }

        [AssertionMethod]
        private static string AssertDogNameNotEmpty(CompetitorXml source)
        {
            if (string.IsNullOrWhiteSpace(source.DogName))
            {
                throw new InvalidDataException("Competitor dog name is missing or empty in XML file.");
            }

            return source.DogName;
        }
    }
}
