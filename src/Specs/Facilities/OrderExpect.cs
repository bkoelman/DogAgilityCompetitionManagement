namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary>
    /// Defines expected outcomes for comparing competitor run results.
    /// </summary>
    internal enum OrderExpect
    {
        /// <summary>
        /// Competitor run results are equivalent.
        /// </summary>
        IsEven,

        /// <summary>
        /// Competitor Y is better (scores a higher rank) than competitor X.
        /// </summary>
        WinnerIsY,

        /// <summary>
        /// Competitor X is better (scores a higher rank) than competitor Y.
        /// </summary>
        WinnerIsX,

        /// <summary>
        /// Outcome is irrelevant. Usually due to a scenario that cannot occur.
        /// </summary>
        DontCare
    }
}