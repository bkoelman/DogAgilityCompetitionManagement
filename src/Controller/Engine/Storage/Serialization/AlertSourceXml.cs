using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="AlertSource" />.
    /// </summary>
    [DataContract(Namespace = "", Name = "AlertSource")]
    public sealed class AlertSourceXml
    {
        [DataMember]
        [CanBeNull]
        public AlertSourceItemXml Picture { get; set; }

        [DataMember]
        [CanBeNull]
        public AlertSourceItemXml Sound { get; set; }

        [NotNull]
        public static AlertSourceXml ToXmlObject([NotNull] AlertSource source)
        {
            Guard.NotNull(source, nameof(source));

            return new AlertSourceXml
            {
                Picture = AlertSourceItemXml.ToXmlObject(source.Picture),
                Sound = AlertSourceItemXml.ToXmlObject(source.Sound)
            };
        }

        [NotNull]
        public static AlertSource FromXmlObject([CanBeNull] AlertSourceXml source)
        {
            return source == null
                ? AlertSource.None
                : new AlertSource(AlertSourceItemXml.FromXmlObject<AlertPictureSourceItem>(source.Picture),
                    AlertSourceItemXml.FromXmlObject<AlertSoundSourceItem>(source.Sound));
        }
    }
}
