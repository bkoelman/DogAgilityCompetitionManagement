using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

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
        [CanBeNull]
        [ItemNotNull]
        public Collection<GateSettingsXml> Gates { get; set; }

        [NotNull]
        [ItemNotNull]
        public Collection<GateSettingsXml> GatesOrEmpty => Gates ?? (Gates = new Collection<GateSettingsXml>());

        [DataMember]
        [CanBeNull]
        [ItemNotNull]
        public Collection<DisplaySettingsXml> Displays { get; set; }

        [NotNull]
        [ItemNotNull]
        public Collection<DisplaySettingsXml> DisplaysOrEmpty
            => Displays ?? (Displays = new Collection<DisplaySettingsXml>());

        [DataMember]
        [CanBeNull]
        [ItemNotNull]
        public Collection<RemoteSettingsXml> Remotes { get; set; }

        [NotNull]
        [ItemNotNull]
        public Collection<RemoteSettingsXml> RemotesOrEmpty
            => Remotes ?? (Remotes = new Collection<RemoteSettingsXml>());

        [DataMember]
        [CanBeNull]
        public MediatorSettingsXml Mediator { get; set; }

        [NotNull]
        public MediatorSettingsXml MediatorOrDefault => Mediator ?? (Mediator = new MediatorSettingsXml());

        public void RemoveDevice([NotNull] WirelessNetworkAddress address)
        {
            Guard.NotNull(address, nameof(address));

            GateSettingsXml gateToRemove = GatesOrEmpty.FirstOrDefault(gate => gate.DeviceAddressNotNull == address);
            if (gateToRemove != null)
            {
                GatesOrEmpty.Remove(gateToRemove);
                return;
            }

            RemoteSettingsXml remoteToRemove =
                RemotesOrEmpty.FirstOrDefault(remote => remote.DeviceAddressNotNull == address);
            if (remoteToRemove != null)
            {
                RemotesOrEmpty.Remove(remoteToRemove);
                return;
            }

            DisplaySettingsXml displayToRemove =
                DisplaysOrEmpty.FirstOrDefault(display => display.DeviceAddressNotNull == address);
            if (displayToRemove != null)
            {
                DisplaysOrEmpty.Remove(displayToRemove);
            }
        }
    }
}