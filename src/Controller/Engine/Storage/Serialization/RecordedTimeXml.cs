using System;
using System.Runtime.Serialization;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// XML representation for <see cref="RecordedTime" />.
    /// </summary>
    [DataContract(Namespace = "")]
    public sealed class RecordedTimeXml
    {
        [DataMember]
        public TimeSpan? HardwareSynchronizedTime { get; set; }

        [DataMember]
        public DateTime SoftwareTimeInUtc { get; set; }

        [DataMember]
        public TimeAccuracy Accuracy { get; set; }

        public static RecordedTimeXml? ToXmlObject(RecordedTime? source)
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

        public static RecordedTime? FromXmlObject(RecordedTimeXml? source)
        {
            return source == null ? null : new RecordedTime(source.HardwareSynchronizedTime, source.SoftwareTimeInUtc, source.Accuracy);
        }
    }
}
