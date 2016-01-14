namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Indicates the current operation mode regarding synchronization of hardware clocks in the wireless network. Because of
    /// hardware imperfections, synchronization gets lost after waiting long times.
    /// </summary>
    public enum ClockSynchronizationMode
    {
        /// <summary>
        /// A resynchronization is should be performed when convenient, because after some more time, starting a new run will no
        /// longer be possible.
        /// </summary>
        RecommendSynchronization,

        /// <summary>
        /// A resynchronization must take place before a new run can be started.
        /// </summary>
        RequireSynchronization,

        /// <summary>
        /// All clocks in the logical network are synchronized.
        /// </summary>
        Normal
    }
}