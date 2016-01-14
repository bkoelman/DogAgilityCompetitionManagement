using System.Runtime.Serialization;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization
{
    /// <summary>
    /// Configuration settings for a remote display in an emulated wireless network.
    /// </summary>
    [DataContract(Namespace = "", Name = "Display")]
    public sealed class DisplaySettingsXml : IWindowSettings
    {
        [DataMember]
        public bool IsPoweredOn { get; set; }

        [DataMember]
        [CanBeNull]
        public WirelessNetworkAddress DeviceAddress { get; set; }

        [NotNull]
        public WirelessNetworkAddress DeviceAddressNotNull
            => DeviceAddress ?? (DeviceAddress = NetworkAddressGenerator.GetNextFreeAddress());

        [DataMember]
        public bool IsInNetwork { get; set; }

        [DataMember]
        public DeviceRoles RolesAssigned { get; set; }

        [DataMember]
        public int SignalStrength { get; set; }

        [DataMember]
        public int BatteryStatus { get; set; }

        [DataMember]
        public bool HasVersionMismatch { get; set; }

        [DataMember]
        public int? WindowLocationX { get; set; }

        [DataMember]
        public int? WindowLocationY { get; set; }

        [DataMember]
        public int WindowWidth { get; set; }

        [DataMember]
        public int WindowHeight { get; set; }

        [DataMember]
        public bool IsMinimized { get; set; }
    }
}