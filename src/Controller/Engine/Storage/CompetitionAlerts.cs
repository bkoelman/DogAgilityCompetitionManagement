using System;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// Various multimedia sources, to be played when important accomplishments are achieved during a competition run.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class CompetitionAlerts : IDisposable
    {
        [NotNull]
        public AlertSource Eliminated { get; }

        [NotNull]
        public AlertSource FirstPlace { get; }

        /// <summary>
        /// A run with no faults and finished within Standard Course Time.
        /// </summary>
        [NotNull]
        public AlertSource CleanRunInStandardCourseTime { get; }

        [NotNull]
        public AlertSource ReadyToStart { get; private set; }

        [NotNull]
        public AlertSource CustomItemA { get; private set; }

        [NotNull]
        public static readonly CompetitionAlerts Empty = new CompetitionAlerts(AlertSource.None, AlertSource.None,
            AlertSource.None, AlertSource.None, AlertSource.None);

        public CompetitionAlerts([NotNull] AlertSource eliminated, [NotNull] AlertSource firstPlace,
            [NotNull] AlertSource cleanRunInStandardCourseTime, [NotNull] AlertSource readyToStart,
            [NotNull] AlertSource customItemA)
        {
            Guard.NotNull(eliminated, nameof(eliminated));
            Guard.NotNull(firstPlace, nameof(firstPlace));
            Guard.NotNull(cleanRunInStandardCourseTime, nameof(cleanRunInStandardCourseTime));
            Guard.NotNull(readyToStart, nameof(readyToStart));
            Guard.NotNull(customItemA, nameof(customItemA));

            Eliminated = eliminated;
            FirstPlace = firstPlace;
            CleanRunInStandardCourseTime = cleanRunInStandardCourseTime;
            ReadyToStart = readyToStart;
            CustomItemA = customItemA;
        }

        public void Dispose()
        {
            Eliminated.Dispose();
            FirstPlace.Dispose();
            CleanRunInStandardCourseTime.Dispose();
        }
    }
}