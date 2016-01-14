using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary>
    /// Defines the contract for a limited user interface implementation that applies visualization changes.
    /// </summary>
    public interface ISimpleVisualizationActor
    {
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
        /// Changes the current competitor number.
        /// </summary>
        /// <param name="number">
        /// The competitor number to display, or <c>null</c> to hide.
        /// </param>
        void SetOrClearCurrentCompetitorNumber([CanBeNull] int? number);

        /// <summary>
        /// Changes the next competitor number.
        /// </summary>
        /// <param name="number">
        /// The competitor number to display, or <c>null</c> to hide..
        /// </param>
        void SetOrClearNextCompetitorNumber([CanBeNull] int? number);

        /// <summary>
        /// Changes the placement for previous competitor.
        /// </summary>
        /// <param name="placement">
        /// The competitor placement, or <c>null</c> to hide.
        /// </param>
        void SetOrClearPreviousCompetitorPlacement([CanBeNull] int? placement);
    }
}