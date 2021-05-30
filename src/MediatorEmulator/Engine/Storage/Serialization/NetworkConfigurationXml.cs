using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization
{
    /// <summary>
    /// The root element in configuration of an emulated wireless network.
    /// </summary>
    [DataContract(Namespace = "", Name = "NetworkConfiguration")]
    public sealed class NetworkConfigurationXml
    {
        [DataMember]
        public bool IsMaximized { get; set; }

        [DataMember]
        public Collection<GateSettingsXml>? Gates { get; set; }

        public Collection<GateSettingsXml> GatesOrEmpty => Gates ??= new Collection<GateSettingsXml>();

        [DataMember]
        public Collection<DisplaySettingsXml>? Displays { get; set; }

        public Collection<DisplaySettingsXml> DisplaysOrEmpty => Displays ??= new Collection<DisplaySettingsXml>();

        [DataMember]
        public Collection<RemoteSettingsXml>? Remotes { get; set; }

        public Collection<RemoteSettingsXml> RemotesOrEmpty => Remotes ??= new Collection<RemoteSettingsXml>();

        [DataMember]
        public MediatorSettingsXml? Mediator { get; set; }

        public MediatorSettingsXml MediatorOrDefault => Mediator ??= new MediatorSettingsXml();

        public void RemoveDevice(WirelessNetworkAddress address)
        {
            Guard.NotNull(address, nameof(address));

            GateSettingsXml? gateToRemove = GatesOrEmpty.FirstOrDefault(gate => gate.DeviceAddressNotNull == address);

            if (gateToRemove != null)
            {
                GatesOrEmpty.Remove(gateToRemove);
                return;
            }

            RemoteSettingsXml? remoteToRemove = RemotesOrEmpty.FirstOrDefault(remote => remote.DeviceAddressNotNull == address);

            if (remoteToRemove != null)
            {
                RemotesOrEmpty.Remove(remoteToRemove);
                return;
            }

            DisplaySettingsXml? displayToRemove = DisplaysOrEmpty.FirstOrDefault(display => display.DeviceAddressNotNull == address);

            if (displayToRemove != null)
            {
                DisplaysOrEmpty.Remove(displayToRemove);
            }
        }
    }
}
