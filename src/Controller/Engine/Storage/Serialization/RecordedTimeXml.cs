using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="RecordedTime" />.
    /// </summary>
    [DataContract(Namespace = "")]
    public sealed class RecordedTimeXml
    {
        [DataMember]
        [CanBeNull]
        public TimeSpan? HardwareSynchronizedTime { get; set; }

        [DataMember]
        public DateTime SoftwareTimeInUtc { get; set; }

        [DataMember]
        public TimeAccuracy Accuracy { get; set; }

        [CanBeNull]
        public static RecordedTimeXml ToXmlObject([CanBeNull] RecordedTime source)
        {
            return source == null
                ? null
                : new RecordedTimeXml
                {
                    HardwareSynchronizedTime = source.HardwareSynchronizedTime,
                    SoftwareTimeInUtc = source.SoftwareTimeInUtc,
                    Accuracy = source.Accuracy
                };
        }

        [CanBeNull]
        public static RecordedTime FromXmlObject([CanBeNull] RecordedTimeXml source)
        {
            return source == null ? null : new RecordedTime(source.HardwareSynchronizedTime, source.SoftwareTimeInUtc, source.Accuracy);
        }
    }
}
