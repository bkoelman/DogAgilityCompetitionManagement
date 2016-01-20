using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="CompetitionAlerts" />.
    /// </summary>
    [DataContract(Namespace = "", Name = "Alerts")]
    public sealed class CompetitionAlertsXml
    {
        [DataMember]
        [CanBeNull]
        public AlertSourceXml Eliminated { get; set; }

        [DataMember]
        [CanBeNull]
        public AlertSourceXml FirstPlace { get; set; }

        [DataMember]
        [CanBeNull]
        public AlertSourceXml CleanRunInStandardCourseTime { get; set; }

        [DataMember]
        [CanBeNull]
        public AlertSourceXml ReadyToStart { get; set; }

        [DataMember]
        [CanBeNull]
        public AlertSourceXml CustomItemA { get; set; }

        [NotNull]
        public static CompetitionAlertsXml ToXmlObject([NotNull] CompetitionAlerts source)
        {
            Guard.NotNull(source, nameof(source));

            return new CompetitionAlertsXml
            {
                Eliminated = AlertSourceXml.ToXmlObject(source.Eliminated),
                FirstPlace = AlertSourceXml.ToXmlObject(source.FirstPlace),
                CleanRunInStandardCourseTime = AlertSourceXml.ToXmlObject(source.CleanRunInStandardCourseTime),
                ReadyToStart = AlertSourceXml.ToXmlObject(source.ReadyToStart),
                CustomItemA = AlertSourceXml.ToXmlObject(source.CustomItemA)
            };
        }

        [NotNull]
        public static CompetitionAlerts FromXmlObject([CanBeNull] CompetitionAlertsXml source)
        {
            return source == null
                ? CompetitionAlerts.Empty
                : new CompetitionAlerts(AlertSourceXml.FromXmlObject(source.Eliminated),
                    AlertSourceXml.FromXmlObject(source.FirstPlace),
                    AlertSourceXml.FromXmlObject(source.CleanRunInStandardCourseTime),
                    AlertSourceXml.FromXmlObject(source.ReadyToStart), AlertSourceXml.FromXmlObject(source.CustomItemA));
        }
    }
}