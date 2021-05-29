using JetBrains.Annotations;

namespace DogAgilityCompetition.DeviceConfigurer.Phases
{
    /// <summary>
    /// Represents phase (5) in the network address assignment process.
    /// </summary>
    public sealed class PhaseAssignmentCompleted : AssignmentPhase
    {
        [CanBeNull]
        public int? MediatorStatus { get; }

        public PhaseAssignmentCompleted([CanBeNull] int? mediatorStatus)
        {
            MediatorStatus = mediatorStatus;
        }
    }
}
