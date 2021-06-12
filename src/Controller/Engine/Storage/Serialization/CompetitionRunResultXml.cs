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
        public CompetitorXml? Competitor { get; set; }

        [DataMember]
        public CompetitionRunTimingsXml? Timings { get; set; }

        [DataMember]
        public int FaultCount { get; set; }

        [DataMember]
        public int RefusalCount { get; set; }

        [DataMember]
        public bool IsEliminated { get; set; }

        public static CompetitionRunResultXml ToXmlObject(CompetitionRunResult source)
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

        public static CompetitionRunResult FromXmlObject(CompetitionRunResultXml source)
        {
            Guard.NotNull(source, nameof(source));
            CompetitorXml competitor = AssertCompetitorNotNull(source);

            // @formatter:keep_existing_linebreaks true

            return new CompetitionRunResult(CompetitorXml.FromXmlObject(competitor))
                .ChangeTimings(CompetitionRunTimingsXml.FromXmlObject(source.Timings))
                .ChangeFaultCount(source.FaultCount)
                .ChangeRefusalCount(source.RefusalCount)
                .ChangeIsEliminated(source.IsEliminated);

            // @formatter:keep_existing_linebreaks restore
        }

        [AssertionMethod]
        private static CompetitorXml AssertCompetitorNotNull(CompetitionRunResultXml source)
        {
            if (source.Competitor == null)
            {
                throw new InvalidDataException("Competitor is missing in XML file.");
            }

            return source.Competitor;
        }
    }
}
