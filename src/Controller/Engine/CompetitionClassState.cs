namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// Indicates the state of a real-time competition run.
/// </summary>
public enum CompetitionClassState
{
    /// <summary>
    /// No active class/runs.
    /// </summary>
    Offline,

    /// <summary>
    /// Initialized to start a new run.
    /// </summary>
    SetupCompleted,

    /// <summary>
    /// Waiting for completion of clock synchronization.
    /// </summary>
    WaitingForSync,

    /// <summary>
    /// Competitor is now allowed to start running.
    /// </summary>
    ReadyToStart,

    /// <summary>
    /// Competitor has passed the start gate.
    /// </summary>
    StartPassed,

    /// <summary>
    /// Optional. Competitor has passed gate Intermediate 1.
    /// </summary>
    Intermediate1Passed,

    /// <summary>
    /// Optional. Competitor has passed gate Intermediate 2.
    /// </summary>
    Intermediate2Passed,

    /// <summary>
    /// Optional. Competitor has passed gate Intermediate 3.
    /// </summary>
    Intermediate3Passed,

    /// <summary>
    /// Competitor has passed the finish gate.
    /// </summary>
    FinishPassed,

    /// <summary>
    /// The run has ended.
    /// </summary>
    RunCompleted
}
