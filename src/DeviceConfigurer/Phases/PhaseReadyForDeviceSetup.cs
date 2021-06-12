namespace DogAgilityCompetition.DeviceConfigurer.Phases
{
    /// <summary>
    /// Represents phase (3) in the network address assignment process.
    /// </summary>
    public sealed class PhaseReadyForDeviceSetup : AssignmentPhase
    {
        public int? MediatorStatus { get; }

        public PhaseReadyForDeviceSetup(int? mediatorStatus)
        {
            MediatorStatus = mediatorStatus;
        }
    }
}
