using System;
using DogAgilityCompetition.Circe;

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
        public static readonly CompetitionAlerts Empty = new(AlertSource.None, AlertSource.None, AlertSource.None, AlertSource.None, AlertSource.None);

        public AlertSource Eliminated { get; }
        public AlertSource FirstPlace { get; }

        /// <summary>
        /// A run with no faults and finished within Standard Course Time.
        /// </summary>
        public AlertSource CleanRunInStandardCourseTime { get; }

        public AlertSource ReadyToStart { get; }
        public AlertSource CustomItemA { get; }

        public CompetitionAlerts(AlertSource eliminated, AlertSource firstPlace, AlertSource cleanRunInStandardCourseTime, AlertSource readyToStart,
            AlertSource customItemA)
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
