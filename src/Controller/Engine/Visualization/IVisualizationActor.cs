using System;
using System.Collections.Generic;
using System.Drawing;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Defines the contract for a rich user interface implementation that applies visualization changes.
    /// </summary>
    public interface IVisualizationActor
    {
        /// <summary>
        /// Displays some class-related texts that do not change during a competition run.
        /// </summary>
        /// <param name="classInfo">
        /// The class information, or <c>null</c> to hide.
        /// </param>
        void SetClass([CanBeNull] CompetitionClassInfo classInfo);

        /// <summary>
        /// Signals to start (from zero) a local, on-screen running timer, which represents the time passed.
        /// </summary>
        void StartPrimaryTimer();

        /// <summary>
        /// If running, stops the local on-screen timer, before replacing it with the specified value.
        /// </summary>
        /// <param name="time">
        /// The time value to display, or <c>null</c> to hide.
        /// </param>
        void StopAndSetOrClearPrimaryTime([CanBeNull] TimeSpan? time);

        /// <summary>
        /// Replaces the secondary time with the specified value.
        /// </summary>
        /// <param name="time">
        /// The time value to display, or <c>null</c> to hide.
        /// </param>
        /// <param name="doBlink">
        /// If set to <c>true</c>, blink shortly after change.
        /// </param>
        void SetOrClearSecondaryTime([CanBeNull] TimeSpan? time, bool doBlink);

        /// <summary>
        /// Changes the fault count with the specified value.
        /// </summary>
        /// <param name="count">
        /// The fault count to display, or <c>null</c> to hide.
        /// </param>
        void SetOrClearFaultCount([CanBeNull] int? count);

        /// <summary>
        /// Changes the refusal count with the specified value.
        /// </summary>
        /// <param name="count">
        /// The refusal count to display, or <c>null</c> to hide.
        /// </param>
        void SetOrClearRefusalCount([CanBeNull] int? count);

        /// <summary>
        /// Turns the visualization for elimination on or off.
        /// </summary>
        /// <param name="isEliminated">
        /// If set to <c>true</c>, turn on visualization; otherwise, turn it off.
        /// </param>
        void SetElimination(bool isEliminated);

        /// <summary>
        /// Changes the associated display fields for current competitor.
        /// </summary>
        /// <param name="competitor">
        /// The competitor, or <c>null</c> to hide.
        /// </param>
        void SetOrClearCurrentCompetitor([CanBeNull] Competitor competitor);

        /// <summary>
        /// In the process of number entry, changes the current competitor number.
        /// </summary>
        /// <param name="number">
        /// The competitor number to display.
        /// </param>
        void SetCurrentCompetitorNumber(int number);

        /// <summary>
        /// In the process of number entry, instructs to start or stop blinking of current competitor number.
        /// </summary>
        /// <param name="isEnabled">
        /// If set to <c>true</c>, blinking is active.
        /// </param>
        void BlinkCurrentCompetitorNumber(bool isEnabled);

        /// <summary>
        /// Changes the associated display fields for next competitor.
        /// </summary>
        /// <param name="competitor">
        /// The competitor, or <c>null</c> to hide.
        /// </param>
        void SetOrClearNextCompetitor([CanBeNull] Competitor competitor);

        /// <summary>
        /// In the process of number entry, changes the next competitor number.
        /// </summary>
        /// <param name="number">
        /// The competitor number to display.
        /// </param>
        void SetNextCompetitorNumber(int number);

        /// <summary>
        /// In the process of number entry, instructs to start or stop blinking of next competitor number.
        /// </summary>
        /// <param name="isEnabled">
        /// If set to <c>true</c>, blinking is active.
        /// </param>
        void BlinkNextCompetitorNumber(bool isEnabled);

        /// <summary>
        /// Changes the associated display fields for previous competitor, along with its run result.
        /// </summary>
        /// <param name="competitorRunResult">
        /// The competitor result, or <c>null</c> to hide.
        /// </param>
        void SetOrClearPreviousCompetitorRun([CanBeNull] CompetitionRunResult competitorRunResult);

        /// <summary>
        /// Updates the displayed rankings to the specified values.
        /// </summary>
        /// <param name="rankings">
        /// The rankings to use, or an empty list to hide.
        /// </param>
        void SetOrClearRankings([NotNull] [ItemNotNull] IEnumerable<CompetitionRunResult> rankings);

        /// <summary>
        /// Shows or hides a message regarding clock synchronization status.
        /// </summary>
        /// <param name="mode">
        /// The clock synchronization mode to activate.
        /// </param>
        void SetClockSynchronizationMode(ClockSynchronizationMode mode);

        /// <summary>
        /// Instructs to start playing a fade-in/fade-out animation for the specified picture.
        /// </summary>
        /// <param name="bitmap">
        /// The picture to animate.
        /// </param>
        void StartAnimation([NotNull] Bitmap bitmap);

        /// <summary>
        /// Instructs to start playing the specified sound file.
        /// </summary>
        /// <param name="path">
        /// Full path to the sound file to play, or <c>null</c> to mute.
        /// </param>
        void PlaySound([CanBeNull] string path);
    }
}