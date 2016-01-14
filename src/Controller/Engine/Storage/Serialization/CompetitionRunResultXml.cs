using System.IO;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="CompetitionRunResult" />.
    /// </summary>
    [DataContract(Namespace = "", Name = "RunResult")]
    public sealed class CompetitionRunResultXml
    {
        [DataMember]
        [CanBeNull]
        public CompetitorXml Competitor { get; set; }

        [DataMember]
        [CanBeNull]
        public CompetitionRunTimingsXml Timings { get; set; }

        [DataMember]
        public int FaultCount { get; set; }

        [DataMember]
        public int RefusalCount { get; set; }

        [DataMember]
        public bool IsEliminated { get; set; }

        [NotNull]
        public static CompetitionRunResultXml ToXmlObject([NotNull] CompetitionRunResult source)
        {
            Guard.NotNull(source, nameof(source));

            return new CompetitionRunResultXml
            {
                Competitor = CompetitorXml.ToXmlObject(source.Competitor),
                Timings = CompetitionRunTimingsXml.ToXmlObject(source.Timings),
                FaultCount = source.FaultCount,
                RefusalCount = source.RefusalCount,
                IsEliminated = source.IsEliminated
            };
        }

        [NotNull]
        public static CompetitionRunResult FromXmlObject([NotNull] CompetitionRunResultXml source)
        {
            Guard.NotNull(source, nameof(source));
            CompetitorXml competitor = AssertCompetitorNotNull(source);

            return
                new CompetitionRunResult(CompetitorXml.FromXmlObject(competitor)).ChangeTimings(
                    CompetitionRunTimingsXml.FromXmlObject(source.Timings))
                    .ChangeFaultCount(source.FaultCount)
                    .ChangeRefusalCount(source.RefusalCount)
                    .ChangeIsEliminated(source.IsEliminated);
        }

        [AssertionMethod]
        [NotNull]
        private static CompetitorXml AssertCompetitorNotNull([NotNull] CompetitionRunResultXml source)
        {
            if (source.Competitor == null)
            {
                throw new InvalidDataException("Competitor is missing in XML file.");
            }
            return source.Competitor;
        }
    }
}