using System;
using System.Text;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// Contains detailed information about a competition class.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class CompetitionClassInfo
    {
        [CanBeNull]
        public string Grade { get; }

        [CanBeNull]
        public string Type { get; }

        [CanBeNull]
        public string InspectorName { get; }

        [CanBeNull]
        public string RingName { get; }

        [CanBeNull]
        public TimeSpan? StandardCourseTime { get; }

        [CanBeNull]
        public TimeSpan? MaximumCourseTime { get; }

        [CanBeNull]
        public int? TrackLengthInMeters { get; }

        public CompetitionClassInfo()
        {
        }

        private CompetitionClassInfo([CanBeNull] string grade, [CanBeNull] string type, [CanBeNull] string inspectorName,
            [CanBeNull] string ringName, [CanBeNull] TimeSpan? standardCourseTime,
            [CanBeNull] TimeSpan? maximumCourseTime, [CanBeNull] int? trackLengthInMeters)
        {
            Grade = grade;
            Type = type;
            InspectorName = string.IsNullOrWhiteSpace(inspectorName) ? null : inspectorName;
            RingName = ringName;
            StandardCourseTime = standardCourseTime;
            MaximumCourseTime = maximumCourseTime;
            TrackLengthInMeters = trackLengthInMeters;
        }

        [NotNull]
        public CompetitionClassInfo ChangeGrade([CanBeNull] string grade)
        {
            grade = string.IsNullOrWhiteSpace(grade) ? null : grade;

            return new CompetitionClassInfo(grade, Type, InspectorName, RingName, StandardCourseTime, MaximumCourseTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeType([CanBeNull] string type)
        {
            type = string.IsNullOrWhiteSpace(type) ? null : type;

            return new CompetitionClassInfo(Grade, type, InspectorName, RingName, StandardCourseTime, MaximumCourseTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeInspectorName([CanBeNull] string inspectorName)
        {
            inspectorName = string.IsNullOrWhiteSpace(inspectorName) ? null : inspectorName;

            return new CompetitionClassInfo(Grade, Type, inspectorName, RingName, StandardCourseTime, MaximumCourseTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeRingName([CanBeNull] string ringName)
        {
            string value = string.IsNullOrWhiteSpace(ringName) ? null : ringName;

            return new CompetitionClassInfo(Grade, Type, InspectorName, value, StandardCourseTime, MaximumCourseTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeStandardCourseTime([CanBeNull] TimeSpan? standardCourseTime)
        {
            if (standardCourseTime != null)
            {
                Guard.GreaterOrEqual(standardCourseTime.Value, nameof(standardCourseTime), TimeSpan.FromSeconds(1));
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, standardCourseTime, MaximumCourseTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeMaximumCourseTime([CanBeNull] TimeSpan? maximumCourseTime)
        {
            if (maximumCourseTime != null)
            {
                Guard.GreaterOrEqual(maximumCourseTime.Value, nameof(maximumCourseTime), TimeSpan.FromSeconds(1));
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, StandardCourseTime, maximumCourseTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeTrackLengthInMeters([CanBeNull] int? trackLengthInMeters)
        {
            if (trackLengthInMeters != null)
            {
                Guard.GreaterOrEqual(trackLengthInMeters.Value, nameof(trackLengthInMeters), 1);
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, StandardCourseTime, MaximumCourseTime,
                trackLengthInMeters);
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();
            using (var formatter = new ObjectFormatter(textBuilder, this))
            {
                formatter.Append(() => Grade, () => Grade);
                formatter.Append(() => Type, () => Type);
                formatter.Append(() => InspectorName, () => InspectorName);
                formatter.Append(() => RingName, () => RingName);

                if (StandardCourseTime != null)
                {
                    formatter.Append(() => StandardCourseTime.Value.TotalSeconds, () => StandardCourseTime);
                }
                if (MaximumCourseTime != null)
                {
                    formatter.Append(() => MaximumCourseTime.Value.TotalSeconds, () => MaximumCourseTime);
                }

                formatter.Append(() => TrackLengthInMeters, () => TrackLengthInMeters);
            }
            return textBuilder.ToString();
        }
    }
}