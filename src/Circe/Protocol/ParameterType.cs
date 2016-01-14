using System;
using System.Diagnostics.CodeAnalysis;
using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Contains the set of predefined CIRCE parameters, grouped by parameter type.
    /// </summary>
    public static class ParameterType
    {
        /// <summary>
        /// Lists the predefined <see cref="IntegerParameter" /> types.
        /// </summary>
        [Serializable]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Integer")]
        public enum Integer
        {
            MediatorStatus,
            Capabilities,
            Roles,
            SignalStrength,
            BatteryStatus,
            InputKeys,
            SensorTime,
            ClockSynchronization,
            CurrentCompetitorNumber,
            NextCompetitorNumber,
            StartTimer,
            TimerValue,
            FaultCount,
            RefusalCount,
            PreviousPlacement
        }

        /// <summary>
        /// Lists the predefined <see cref="BooleanParameter" /> types.
        /// </summary>
        [Serializable]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Boolean")]
        public enum Boolean
        {
            GetMembership,
            SetMembership,
            IsAligned,
            Eliminated,
            HasVersionMismatch
        }

        /// <summary>
        /// Lists the predefined <see cref="NetworkAddressParameter" /> types.
        /// </summary>
        [Serializable]
        public enum NetworkAddress
        {
            DestinationAddress,
            OriginatingAddress,
            AssignAddress
        }

        /// <summary>
        /// Lists the predefined <see cref="VersionParameter" /> types.
        /// </summary>
        [Serializable]
        public enum Version
        {
            ProtocolVersion
        }

        /// <summary>
        /// Lists the predefined <see cref="BinaryParameter" /> types.
        /// </summary>
        [Serializable]
        [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
        public enum Binary
        {
            LogData
        }
    }
}