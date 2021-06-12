using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.DeviceConfigurer.Phases
{
    /// <summary>
    /// Represents phase (4) in the network address assignment process.
    /// </summary>
    public sealed class PhaseWaitingForSetupResponse : AssignmentPhase
    {
        public WirelessNetworkAddress NewAddress { get; }

        public PhaseWaitingForSetupResponse(WirelessNetworkAddress newAddress)
        {
            Guard.NotNull(newAddress, nameof(newAddress));

            NewAddress = newAddress;
        }
    }
}
