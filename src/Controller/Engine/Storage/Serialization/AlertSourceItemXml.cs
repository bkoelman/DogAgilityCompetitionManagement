using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization;

/// <summary>
/// XML representation for <see cref="AlertSourceItem" />.
/// </summary>
[DataContract(Namespace = "", Name = "AlertSourceItem")]
public sealed class AlertSourceItemXml
{
    [DataMember]
    public bool IsEnabled { get; set; }

    [DataMember]
    public string? Path { get; set; }

    public static AlertSourceItemXml ToXmlObject(AlertSourceItem source)
    {
        Guard.NotNull(source, nameof(source));

        return new AlertSourceItemXml
        {
            IsEnabled = source.IsEnabled,
            Path = source.Path
        };
    }

    public static T FromXmlObject<T>(AlertSourceItemXml? source)
        where T : AlertSourceItem
    {
        // @formatter:keep_existing_linebreaks true

        return source == null
            ? typeof(T) == typeof(AlertPictureSourceItem)
                ? (T)(AlertSourceItem)AlertPictureSourceItem.None
                : (T)(AlertSourceItem)AlertSoundSourceItem.None
            : typeof(T) == typeof(AlertPictureSourceItem)
                ? (T)(AlertSourceItem)new AlertPictureSourceItem(source.IsEnabled, source.Path)
                : (T)(AlertSourceItem)new AlertSoundSourceItem(source.IsEnabled, source.Path);

        // @formatter:keep_existing_linebreaks restore
    }
}
