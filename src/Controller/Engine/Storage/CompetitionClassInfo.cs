using System;
using System.Diagnostics.CodeAnalysis;
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

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        [CanBeNull]
        public string Type { get; }

        [CanBeNull]
        public string InspectorName { get; }

        [CanBeNull]
        public string RingName { get; }

        [CanBeNull]
        public TimeSpan? StandardParcoursTime { get; }

        [CanBeNull]
        public TimeSpan? MaximumParcoursTime { get; }

        [CanBeNull]
        public int? TrackLengthInMeters { get; }

        public CompetitionClassInfo()
        {
        }

        private CompetitionClassInfo([CanBeNull] string grade, [CanBeNull] string type, [CanBeNull] string inspectorName,
            [CanBeNull] string ringName, [CanBeNull] TimeSpan? standardParcoursTime,
            [CanBeNull] TimeSpan? maximumParcoursTime, [CanBeNull] int? trackLengthInMeters)
        {
            Grade = grade;
            Type = type;
            InspectorName = string.IsNullOrWhiteSpace(inspectorName) ? null : inspectorName;
            RingName = ringName;
            StandardParcoursTime = standardParcoursTime;
            MaximumParcoursTime = maximumParcoursTime;
            TrackLengthInMeters = trackLengthInMeters;
        }

        [NotNull]
        public CompetitionClassInfo ChangeGrade([CanBeNull] string grade)
        {
            grade = string.IsNullOrWhiteSpace(grade) ? null : grade;

            return new CompetitionClassInfo(grade, Type, InspectorName, RingName, StandardParcoursTime,
                MaximumParcoursTime, TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeType([CanBeNull] string type)
        {
            type = string.IsNullOrWhiteSpace(type) ? null : type;

            return new CompetitionClassInfo(Grade, type, InspectorName, RingName, StandardParcoursTime,
                MaximumParcoursTime, TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeInspectorName([CanBeNull] string inspectorName)
        {
            inspectorName = string.IsNullOrWhiteSpace(inspectorName) ? null : inspectorName;

            return new CompetitionClassInfo(Grade, Type, inspectorName, RingName, StandardParcoursTime,
                MaximumParcoursTime, TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeRingName([CanBeNull] string ringName)
        {
            string value = string.IsNullOrWhiteSpace(ringName) ? null : ringName;

            return new CompetitionClassInfo(Grade, Type, InspectorName, value, StandardParcoursTime, MaximumParcoursTime,
                TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeStandardParcoursTime([CanBeNull] TimeSpan? standardParcoursTime)
        {
            if (standardParcoursTime != null)
            {
                Guard.GreaterOrEqual(standardParcoursTime.Value, nameof(standardParcoursTime), TimeSpan.FromSeconds(1));
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, standardParcoursTime,
                MaximumParcoursTime, TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeMaximumParcoursTime([CanBeNull] TimeSpan? maximumParcoursTime)
        {
            if (maximumParcoursTime != null)
            {
                Guard.GreaterOrEqual(maximumParcoursTime.Value, nameof(maximumParcoursTime), TimeSpan.FromSeconds(1));
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, StandardParcoursTime,
                maximumParcoursTime, TrackLengthInMeters);
        }

        [NotNull]
        public CompetitionClassInfo ChangeTrackLengthInMeters([CanBeNull] int? trackLengthInMeters)
        {
            if (trackLengthInMeters != null)
            {
                Guard.GreaterOrEqual(trackLengthInMeters.Value, nameof(trackLengthInMeters), 1);
            }

            return new CompetitionClassInfo(Grade, Type, InspectorName, RingName, StandardParcoursTime,
                MaximumParcoursTime, trackLengthInMeters);
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

                if (StandardParcoursTime != null)
                {
                    formatter.Append(() => StandardParcoursTime.Value.TotalSeconds, () => StandardParcoursTime);
                }
                if (MaximumParcoursTime != null)
                {
                    formatter.Append(() => MaximumParcoursTime.Value.TotalSeconds, () => MaximumParcoursTime);
                }

                formatter.Append(() => TrackLengthInMeters, () => TrackLengthInMeters);
            }
            return textBuilder.ToString();
        }
    }
}