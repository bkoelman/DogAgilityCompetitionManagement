using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization;

/// <summary>
/// XML representation for <see cref="AlertSource" />.
/// </summary>
[DataContract(Namespace = "", Name = "AlertSource")]
public sealed class AlertSourceXml
{
    [DataMember]
    public AlertSourceItemXml? Picture { get; set; }

    [DataMember]
    public AlertSourceItemXml? Sound { get; set; }

    public static AlertSourceXml ToXmlObject(AlertSource source)
    {
        Guard.NotNull(source, nameof(source));

        return new AlertSourceXml
        {
            Picture = AlertSourceItemXml.ToXmlObject(source.Picture),
            Sound = AlertSourceItemXml.ToXmlObject(source.Sound)
        };
    }

    public static AlertSource FromXmlObject(AlertSourceXml? source)
    {
        return source == null
            ? AlertSource.None
            : new AlertSource(AlertSourceItemXml.FromXmlObject<AlertPictureSourceItem>(source.Picture),
                AlertSourceItemXml.FromXmlObject<AlertSoundSourceItem>(source.Sound));
    }
}
