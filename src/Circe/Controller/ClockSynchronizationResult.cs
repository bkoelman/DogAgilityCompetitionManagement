namespace DogAgilityCompetition.Circe.Controller;

/// <summary>
/// The possible outcomes when synchronizing the hardware clocks of a set of wireless devices.
/// </summary>
public enum ClockSynchronizationResult
{
    /// <summary>
    /// Collective clock synchronization succeeded.
    /// </summary>
    Succeeded,

    /// <summary>
    /// Collective clock synchronization failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Collective clock synchronization timed out or was canceled before completion by caller.
    /// </summary>
    CanceledOrTimedOut
}
