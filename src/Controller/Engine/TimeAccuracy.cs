namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Defines the accuracy of time.
    /// </summary>
    public enum TimeAccuracy
    {
        /// <summary>
        /// Indicates that a time was measured using hardware-synchronized clocks in the wireless network.
        /// </summary>
        HighPrecision,

        /// <summary>
        /// Indicates that a time was measured using software clock.
        /// </summary>
        LowPrecision,

        /// <summary>
        /// Indicates a time that was manually edited by a user.
        /// </summary>
        UserEdited
    }
}