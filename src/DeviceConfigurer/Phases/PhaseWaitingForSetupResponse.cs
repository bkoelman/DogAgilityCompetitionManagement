using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.DeviceConfigurer.Phases
{
    /// <summary>
    /// Represents phase (4) in the network address assignment process.
    /// </summary>
    public sealed class PhaseWaitingForSetupResponse : AssignmentPhase
    {
        [NotNull]
        public WirelessNetworkAddress NewAddress { get; }

        public PhaseWaitingForSetupResponse([NotNull] WirelessNetworkAddress newAddress)
        {
            Guard.NotNull(newAddress, nameof(newAddress));

            NewAddress = newAddress;
        }
    }
}
