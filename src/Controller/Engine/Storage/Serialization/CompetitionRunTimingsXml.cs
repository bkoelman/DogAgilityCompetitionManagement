using System.Runtime.Serialization;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="CompetitionRunTimings" />.
    /// </summary>
    [DataContract(Namespace = "")]
    public sealed class CompetitionRunTimingsXml
    {
        [DataMember]
        public RecordedTimeXml? StartTime { get; set; }

        [DataMember]
        public RecordedTimeXml? IntermediateTime1 { get; set; }

        [DataMember]
        public RecordedTimeXml? IntermediateTime2 { get; set; }

        [DataMember]
        public RecordedTimeXml? IntermediateTime3 { get; set; }

        [DataMember]
        public RecordedTimeXml? FinishTime { get; set; }

        public static CompetitionRunTimingsXml? ToXmlObject(CompetitionRunTimings? source)
        {
            return source == null
                ? null
                : new CompetitionRunTimingsXml
                {
                    StartTime = RecordedTimeXml.ToXmlObject(source.StartTime),
                    IntermediateTime1 = RecordedTimeXml.ToXmlObject(source.IntermediateTime1),
                    IntermediateTime2 = RecordedTimeXml.ToXmlObject(source.IntermediateTime2),
                    IntermediateTime3 = RecordedTimeXml.ToXmlObject(source.IntermediateTime3),
                    FinishTime = RecordedTimeXml.ToXmlObject(source.FinishTime)
                };
        }

        public static CompetitionRunTimings? FromXmlObject(CompetitionRunTimingsXml? source)
        {
            RecordedTime? startTime = source != null ? RecordedTimeXml.FromXmlObject(source.StartTime) : null;

            // @formatter:keep_existing_linebreaks true

            return startTime != null && source != null
                ? new CompetitionRunTimings(startTime)
                    .ChangeIntermediateTime1(RecordedTimeXml.FromXmlObject(source.IntermediateTime1))
                    .ChangeIntermediateTime2(RecordedTimeXml.FromXmlObject(source.IntermediateTime2))
                    .ChangeIntermediateTime3(RecordedTimeXml.FromXmlObject(source.IntermediateTime3))
                    .ChangeFinishTime(RecordedTimeXml.FromXmlObject(source.FinishTime))
                : null;

            // @formatter:keep_existing_linebreaks restore
        }
    }
}
