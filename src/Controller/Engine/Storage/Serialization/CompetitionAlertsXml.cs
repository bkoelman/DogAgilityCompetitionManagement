using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization;

/// <summary>
/// XML representation for <see cref="CompetitionAlerts" />.
/// </summary>
[DataContract(Namespace = "", Name = "Alerts")]
public sealed class CompetitionAlertsXml
{
    [DataMember]
    public AlertSourceXml? Eliminated { get; set; }

    [DataMember]
    public AlertSourceXml? FirstPlace { get; set; }

    [DataMember]
    public AlertSourceXml? CleanRunInStandardCourseTime { get; set; }

    [DataMember]
    public AlertSourceXml? ReadyToStart { get; set; }

    [DataMember]
    public AlertSourceXml? CustomItemA { get; set; }

    public static CompetitionAlertsXml ToXmlObject(CompetitionAlerts source)
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

    public static CompetitionAlerts FromXmlObject(CompetitionAlertsXml? source)
    {
        // @formatter:keep_existing_linebreaks true

        return source == null
            ? CompetitionAlerts.Empty
            : new CompetitionAlerts(AlertSourceXml.FromXmlObject(source.Eliminated),
                AlertSourceXml.FromXmlObject(source.FirstPlace),
                AlertSourceXml.FromXmlObject(source.CleanRunInStandardCourseTime),
                AlertSourceXml.FromXmlObject(source.ReadyToStart),
                AlertSourceXml.FromXmlObject(source.CustomItemA));

        // @formatter:keep_existing_linebreaks restore
    }
}
