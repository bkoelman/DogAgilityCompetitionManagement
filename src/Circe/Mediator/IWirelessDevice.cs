using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;

namespace DogAgilityCompetition.Circe.Mediator
{
    /// <summary>
    /// Defines the contract for a hardware device on the CIRCE network.
    /// </summary>
    public interface IWirelessDevice
    {
        bool IsPoweredOn { get; }

        WirelessNetworkAddress Address { get; }

        void ChangeAddress(WirelessNetworkAddress newAddress);

        void Accept(AlertOperation operation);

        void Accept(NetworkSetupOperation operation);

        void Accept(SynchronizeClocksOperation operation);

        void Accept(VisualizeOperation operation);
    }
}
