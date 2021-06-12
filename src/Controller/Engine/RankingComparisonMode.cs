namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Lists the various competitor run result comparison modes.
    /// </summary>
    public enum RankingComparisonMode
    {
        /// <summary>
        /// Comparison mode for normal operation.
        /// </summary>
        Regular,

        /// <summary>
        /// Only the Completion comparison phase is applied. Intended for unit tests.
        /// </summary>
        OnlyPhaseCompletion,

        /// <summary>
        /// Only the PenaltyOverrun comparison phase is applied. Intended for unit tests.
        /// </summary>
        OnlyPhasePenaltyOverrun,

        /// <summary>
        /// Only the FinishNumber comparison phase is applied. Intended for unit tests.
        /// </summary>
        OnlyPhaseFinishNumber
    }
}
