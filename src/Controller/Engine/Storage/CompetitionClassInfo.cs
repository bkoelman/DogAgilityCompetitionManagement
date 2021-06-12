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
        public string? Grade { get; }
        public string? Type { get; }
        public string? InspectorName { get; }
        public string? RingName { get; }
        public TimeSpan? StandardCourseTime { get; }
        public TimeSpan? MaximumCourseTime { get; }
        public int? TrackLengthInMeters { get; }

        public CompetitionClassInfo()
        {
        }

        private CompetitionClassInfo(string? grade, string? type, string? inspectorName, string? ringName, TimeSpan? standardCourseTime,
            TimeSpan? maximumCourseTime, int? trackLengthInMeters)
        {
            Grade = grade;
            Type = type;
            InspectorName = string.IsNullOrWhiteSpace(inspectorName) ? null : inspectorName;
            RingName = ringName;
            StandardCourseTime = standardCourseTime;
            MaximumCourseTime = maximumCourseTime;
            TrackLengthInMeters = trackLengthInMeters;
        }

        public CompetitionClassInfo ChangeGrade(string? grade)
        {
            grade = string.IsNullOrWhiteSpace(grade) ? null : grade;

            return new CompetitionClassInfo(grade, Type, InspectorName, RingName, StandardCourseTime, MaximumCourseTime, TrackLengthInMeters);
        }

        public CompetitionClassInfo ChangeType(string? type)
        {
            type = string.IsNullOrWhiteSpace(type) ? null : type;

            return new CompetitionClassInfo(Grade, type, InspectorName, RingName, StandardCourseTime, MaximumCourseTime, TrackLengthInMeters);
        }

        public CompetitionClassInfo ChangeInspectorName(string? inspectorName)
        {
            inspectorName = string.IsNullOrWhiteSpace(inspectorName) ? null : inspectorName;

            return new CompetitionClassInfo(Grade, Type, inspectorName, RingName, StandardCourseTime, MaximumCourseTime, TrackLengthInMeters);
        }

        public CompetitionClassInfo ChangeRingName(string? ringName)
        {
            string? value = string.IsNullOrWhiteSpace(ringName) ? null : ringName;

            return new CompetitionClassInfo(Grade, Type, InspectorName, value, StandardCourseTime, MaximumCourseTime, TrackLengthInMeters);
        }

        public CompetitionClassInfo ChangeStandardCourseTime(TimeSpan? standardCourseTime)
        {
            if (standardCourseTime != null)
            {
                Guard.GreaterOrEqual(standardCourseTime.Value, nameof(standardCourseTime), TimeSpan.FromSeconds(1));
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, standardCourseTime, MaximumCourseTime, TrackLengthInMeters);
        }

        public CompetitionClassInfo ChangeMaximumCourseTime(TimeSpan? maximumCourseTime)
        {
            if (maximumCourseTime != null)
            {
                Guard.GreaterOrEqual(maximumCourseTime.Value, nameof(maximumCourseTime), TimeSpan.FromSeconds(1));
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, StandardCourseTime, maximumCourseTime, TrackLengthInMeters);
        }

        public CompetitionClassInfo ChangeTrackLengthInMeters(int? trackLengthInMeters)
        {
            if (trackLengthInMeters != null)
            {
                Guard.GreaterOrEqual(trackLengthInMeters.Value, nameof(trackLengthInMeters), 1);
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, StandardCourseTime, MaximumCourseTime, trackLengthInMeters);
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            using (var formatter = new ObjectFormatter(textBuilder, this))
            {
                formatter.Append(Grade, nameof(Grade));
                formatter.Append(Type, nameof(Type));
                formatter.Append(InspectorName, nameof(InspectorName));
                formatter.Append(RingName, nameof(RingName));

                if (StandardCourseTime != null)
                {
                    formatter.Append(StandardCourseTime.Value.TotalSeconds, nameof(StandardCourseTime));
                }

                if (MaximumCourseTime != null)
                {
                    formatter.Append(MaximumCourseTime.Value.TotalSeconds, nameof(MaximumCourseTime));
                }

                formatter.Append(TrackLengthInMeters, nameof(TrackLengthInMeters));
            }

            return textBuilder.ToString();
        }
    }
}
