namespace DogAgilityCompetition.DeviceConfigurer.Phases;

/// <summary>
/// Represents phase (5) in the network address assignment process.
/// </summary>
public sealed class PhaseAssignmentCompleted : AssignmentPhase
{
    public int? MediatorStatus { get; }

    public PhaseAssignmentCompleted(int? mediatorStatus)
    {
        MediatorStatus = mediatorStatus;
    }
}
