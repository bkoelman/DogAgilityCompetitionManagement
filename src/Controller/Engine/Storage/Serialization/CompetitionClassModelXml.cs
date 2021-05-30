using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="CompetitionClassModel" />.
    /// </summary>
    [DataContract(Namespace = "", Name = "CompetitionClass")]
    public sealed class CompetitionClassModelXml
    {
        [DataMember]
        public Collection<CompetitionRunResultXml>? RunResults { get; set; }

        [DataMember]
        public int? LastCompletedCompetitorNumber { get; set; }

        [DataMember]
        public string? Grade { get; set; }

        [DataMember]
        public string? ClassType { get; set; }

        [DataMember]
        public string? InspectorName { get; set; }

        [DataMember]
        public string? RingName { get; set; }

        [DataMember]
        public TimeSpan? StandardCourseTime { get; set; }

        [DataMember]
        public TimeSpan? MaximumCourseTime { get; set; }

        [DataMember]
        public int? TrackLengthInMeters { get; set; }

        [DataMember]
        public int IntermediateTimerCount { get; set; }

        [DataMember]
        public TimeSpan StartFinishMinDelayForSingleSensor { get; set; }

        [DataMember]
        public CompetitionAlertsXml? Alerts { get; set; }

        public static CompetitionClassModelXml ToXmlObject(CompetitionClassModel source)
        {
            Guard.NotNull(source, nameof(source));

            return new CompetitionClassModelXml
            {
                RunResults = source.Results.Select(CompetitionRunResultXml.ToXmlObject).ToCollection(),
                LastCompletedCompetitorNumber = source.LastCompletedCompetitorNumber,
                Grade = source.ClassInfo.Grade,
                ClassType = source.ClassInfo.Type,
                InspectorName = source.ClassInfo.InspectorName,
                RingName = source.ClassInfo.RingName,
                StandardCourseTime = source.ClassInfo.StandardCourseTime,
                MaximumCourseTime = source.ClassInfo.MaximumCourseTime,
                TrackLengthInMeters = source.ClassInfo.TrackLengthInMeters,
                IntermediateTimerCount = source.IntermediateTimerCount,
                StartFinishMinDelayForSingleSensor = source.StartFinishMinDelayForSingleSensor,
                Alerts = CompetitionAlertsXml.ToXmlObject(source.Alerts)
            };
        }

        public static CompetitionClassModel FromXmlObject(CompetitionClassModelXml source)
        {
            Guard.NotNull(source, nameof(source));

            // @formatter:keep_existing_linebreaks true

            return new CompetitionClassModel()
                .ChangeRunResults(source.RunResults.EmptyIfNull().Select(CompetitionRunResultXml.FromXmlObject))
                .SafeChangeLastCompletedCompetitorNumber(source.LastCompletedCompetitorNumber)
                .ChangeClassInfo(new CompetitionClassInfo()
                    .ChangeGrade(source.Grade)
                    .ChangeType(source.ClassType)
                    .ChangeInspectorName(source.InspectorName)
                    .ChangeRingName(source.RingName)
                    .ChangeStandardCourseTime(source.StandardCourseTime)
                    .ChangeMaximumCourseTime(source.MaximumCourseTime)
                    .ChangeTrackLengthInMeters(source.TrackLengthInMeters))
                .ChangeIntermediateTimerCount(source.IntermediateTimerCount)
                .ChangeStartFinishMinDelayForSingleSensor(source.StartFinishMinDelayForSingleSensor)
                .ChangeAlerts(CompetitionAlertsXml.FromXmlObject(source.Alerts));

            // @formatter:keep_existing_linebreaks restore
        }
    }
}
