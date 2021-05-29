using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary>
    /// Generator for unique wireless network addresses.
    /// </summary>
    public static class NetworkAddressGenerator
    {
        [NotNull]
        public static WirelessNetworkAddress GetNextFreeAddress()
        {
            int lastAddressUsed = RegistrySettingsProvider.GetLastUsedAddress();
            lastAddressUsed++;
            RegistrySettingsProvider.SaveLastUsedAddress(lastAddressUsed);

            string address = $"{lastAddressUsed:X6}";
            return new WirelessNetworkAddress(address);
        }
    }
}
