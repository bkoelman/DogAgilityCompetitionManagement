namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// Indicates which gate a competitor passed.
/// </summary>
public enum GatePassage
{
    /// <summary>
    /// Competitor passed the start gate.
    /// </summary>
    PassStart,

    /// <summary>
    /// Competitor passed the first intermediate gate.
    /// </summary>
    PassIntermediate1,

    /// <summary>
    /// Competitor passed the second intermediate gate.
    /// </summary>
    PassIntermediate2,

    /// <summary>
    /// Competitor passed the third intermediate gate.
    /// </summary>
    PassIntermediate3,

    /// <summary>
    /// Competitor passed the finish gate.
    /// </summary>
    PassFinish,

    /// <summary>
    /// Competitor passed the gate that functions as both and finish.
    /// </summary>
    PassStartFinish
}
