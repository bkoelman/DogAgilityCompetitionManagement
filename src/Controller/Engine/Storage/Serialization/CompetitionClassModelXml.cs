using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="CompetitionClassModel" />.
    /// </summary>
    [DataContract(Namespace = "", Name = "CompetitionClass")]
    public sealed class CompetitionClassModelXml
    {
        [DataMember]
        [CanBeNull]
        [ItemNotNull]
        public Collection<CompetitionRunResultXml> RunResults { get; set; }

        [DataMember]
        [CanBeNull]
        public int? LastCompletedCompetitorNumber { get; set; }

        [DataMember]
        [CanBeNull]
        public string Grade { get; set; }

        [DataMember]
        [CanBeNull]
        public string ClassType { get; set; }

        [DataMember]
        [CanBeNull]
        public string InspectorName { get; set; }

        [DataMember]
        [CanBeNull]
        public string RingName { get; set; }

        [DataMember]
        [CanBeNull]
        public TimeSpan? StandardCourseTime { get; set; }

        [DataMember]
        [CanBeNull]
        public TimeSpan? MaximumCourseTime { get; set; }

        [DataMember]
        [CanBeNull]
        public int? TrackLengthInMeters { get; set; }

        [DataMember]
        public int IntermediateTimerCount { get; set; }

        [DataMember]
        public TimeSpan StartFinishMinDelayForSingleSensor { get; set; }

        [DataMember]
        [CanBeNull]
        public CompetitionAlertsXml Alerts { get; set; }

        [NotNull]
        public static CompetitionClassModelXml ToXmlObject([NotNull] CompetitionClassModel source)
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

        [NotNull]
        public static CompetitionClassModel FromXmlObject([NotNull] CompetitionClassModelXml source)
        {
            Guard.NotNull(source, nameof(source));

            return
                new CompetitionClassModel().ChangeRunResults(
                    source.RunResults.EmptyIfNull().Select(CompetitionRunResultXml.FromXmlObject))
                    .SafeChangeLastCompletedCompetitorNumber(source.LastCompletedCompetitorNumber)
                    .ChangeClassInfo(
                        new CompetitionClassInfo().ChangeGrade(source.Grade)
                            .ChangeType(source.ClassType)
                            .ChangeInspectorName(source.InspectorName)
                            .ChangeRingName(source.RingName)
                            .ChangeStandardCourseTime(source.StandardCourseTime)
                            .ChangeMaximumCourseTime(source.MaximumCourseTime)
                            .ChangeTrackLengthInMeters(source.TrackLengthInMeters))
                    .ChangeIntermediateTimerCount(source.IntermediateTimerCount)
                    .ChangeStartFinishMinDelayForSingleSensor(source.StartFinishMinDelayForSingleSensor)
                    .ChangeAlerts(CompetitionAlertsXml.FromXmlObject(source.Alerts));
        }
    }
}