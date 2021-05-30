using System.Runtime.Serialization;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization
{
    /// <summary>
    /// Configuration settings for a passage gate in an emulated wireless network.
    /// </summary>
    [DataContract(Namespace = "", Name = "Gate")]
    public sealed class GateSettingsXml : IWindowSettings
    {
        [DataMember]
        public bool IsPoweredOn { get; set; }

        [DataMember]
        public WirelessNetworkAddress? DeviceAddress { get; set; }

        public WirelessNetworkAddress DeviceAddressNotNull => DeviceAddress ??= NetworkAddressGenerator.GetNextFreeAddress();

        [DataMember]
        public bool IsInNetwork { get; set; }

        [DataMember]
        public DeviceRoles RolesAssigned { get; set; }

        [DataMember]
        public int SignalStrength { get; set; }

        [DataMember]
        public int BatteryStatus { get; set; }

        [DataMember]
        public bool IsAligned { get; set; }

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
