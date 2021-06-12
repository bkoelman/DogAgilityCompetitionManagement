using System;
using System.Runtime.Serialization;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization
{
    /// <summary>
    /// Configuration settings for a mediator in an emulated wireless network.
    /// </summary>
    [DataContract(Namespace = "", Name = "Mediator")]
    public sealed class MediatorSettingsXml : IWindowSettings
    {
        [DataMember]
        public bool IsPoweredOn { get; set; }

        [DataMember]
        public WirelessNetworkAddress? DeviceAddress { get; set; }

        public WirelessNetworkAddress DeviceAddressNotNull => DeviceAddress ??= NetworkAddressGenerator.GetNextFreeAddress();

        [DataMember]
        public string? ComPortName { get; set; }

        [DataMember]
        public int MediatorStatus { get; set; }

        [DataMember]
        public string? ProtocolVersion { get; set; }

        public Version ProtocolVersionOrDefault => Version.TryParse(ProtocolVersion, out Version? result) ? result : KeepAliveOperation.CurrentProtocolVersion;

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
