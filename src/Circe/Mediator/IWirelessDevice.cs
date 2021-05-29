using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Mediator
{
    /// <summary>
    /// Defines the contract for a hardware device on the CIRCE network.
    /// </summary>
    public interface IWirelessDevice
    {
        bool IsPoweredOn { get; }

        [NotNull]
        WirelessNetworkAddress Address { get; }

        void ChangeAddress([NotNull] WirelessNetworkAddress newAddress);

        void Accept([NotNull] AlertOperation operation);

        void Accept([NotNull] NetworkSetupOperation operation);

        void Accept([NotNull] SynchronizeClocksOperation operation);

        void Accept([NotNull] VisualizeOperation operation);
    }
}
