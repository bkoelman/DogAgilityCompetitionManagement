using JetBrains.Annotations;

namespace DogAgilityCompetition.DeviceConfigurer.Phases
{
    /// <summary>
    /// Represents phase (3) in the network address assignment process.
    /// </summary>
    public sealed class PhaseReadyForDeviceSetup : AssignmentPhase
    {
        [CanBeNull]
        public int? MediatorStatus { get; private set; }

        public PhaseReadyForDeviceSetup([CanBeNull] int? mediatorStatus)
        {
            MediatorStatus = mediatorStatus;
        }
    }
}