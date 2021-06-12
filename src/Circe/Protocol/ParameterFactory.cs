using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Provides a factory for creating known CIRCE <see cref="Parameter" /> instances.
    /// </summary>
    public static class ParameterFactory
    {
        private static readonly Dictionary<ParameterType.Integer, IntegerParameterDefinition> IntegerMap = new()
        {
            { ParameterType.Integer.MediatorStatus, new IntegerParameterDefinition(12, 0, 999) },
            { ParameterType.Integer.Capabilities, new IntegerParameterDefinition(19, 0, 127) },
            { ParameterType.Integer.Roles, new IntegerParameterDefinition(20, 0, 127) },
            { ParameterType.Integer.SignalStrength, new IntegerParameterDefinition(21, 0, 255) },
            { ParameterType.Integer.BatteryStatus, new IntegerParameterDefinition(22, 0, 255) },
            { ParameterType.Integer.InputKeys, new IntegerParameterDefinition(24, 0, 65535) },
            { ParameterType.Integer.SensorTime, new IntegerParameterDefinition(25, 0, 999999) },
            { ParameterType.Integer.ClockSynchronization, new IntegerParameterDefinition(27, 1, 2) },
            { ParameterType.Integer.CurrentCompetitorNumber, new IntegerParameterDefinition(28, 0, 999) },
            { ParameterType.Integer.NextCompetitorNumber, new IntegerParameterDefinition(29, 0, 999) },
            { ParameterType.Integer.StartTimer, new IntegerParameterDefinition(30, 1, 1) },
            { ParameterType.Integer.PrimaryTimerValue, new IntegerParameterDefinition(31, 0, 999999) },
            { ParameterType.Integer.SecondaryTimerValue, new IntegerParameterDefinition(38, 0, 999999) },
            { ParameterType.Integer.FaultCount, new IntegerParameterDefinition(32, 0, 99) },
            { ParameterType.Integer.RefusalCount, new IntegerParameterDefinition(33, 0, 99) },
            { ParameterType.Integer.PreviousPlacement, new IntegerParameterDefinition(35, 0, 999) }
        };

        private static readonly Dictionary<ParameterType.Boolean, ParameterDefinition> BooleanMap = new()
        {
            { ParameterType.Boolean.GetMembership, new ParameterDefinition(16) },
            { ParameterType.Boolean.SetMembership, new ParameterDefinition(17) },
            { ParameterType.Boolean.IsAligned, new ParameterDefinition(23) },
            { ParameterType.Boolean.Eliminated, new ParameterDefinition(34) },
            { ParameterType.Boolean.HasVersionMismatch, new ParameterDefinition(37) }
        };

        private static readonly Dictionary<ParameterType.NetworkAddress, ParameterDefinition> NetworkAddressMap = new()
        {
            { ParameterType.NetworkAddress.DestinationAddress, new ParameterDefinition(14) },
            { ParameterType.NetworkAddress.OriginatingAddress, new ParameterDefinition(15) },
            { ParameterType.NetworkAddress.AssignAddress, new ParameterDefinition(26) }
        };

        private static readonly Dictionary<ParameterType.Version, ParameterDefinition> VersionMap = new()
        {
            { ParameterType.Version.ProtocolVersion, new ParameterDefinition(10) }
        };

        private static readonly Dictionary<ParameterType.Binary, ParameterDefinition> BinaryMap = new()
        {
            { ParameterType.Binary.LogData, new ParameterDefinition(36) }
        };

        /// <summary>
        /// Creates an <see cref="IntegerParameter" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to create.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the value of the parameter is required.
        /// </param>
        /// <returns>
        /// The parameter.
        /// </returns>
        [Pure]
        public static IntegerParameter Create(ParameterType.Integer name, bool isRequired)
        {
            IntegerParameterDefinition definition = IntegerMap[name];

            string? nameString = Enum.GetName(typeof(ParameterType.Integer), name);
            Assertions.IsNotNull(nameString, nameof(nameString));

            return new IntegerParameter(nameString, definition.Id, definition.MinValue, definition.MaxValue, isRequired);
        }

        /// <summary>
        /// Creates a <see cref="BooleanParameter" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to create.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the value of the parameter is required.
        /// </param>
        /// <returns>
        /// The parameter.
        /// </returns>
        [Pure]
        public static BooleanParameter Create(ParameterType.Boolean name, bool isRequired)
        {
            ParameterDefinition definition = BooleanMap[name];

            string? nameString = Enum.GetName(typeof(ParameterType.Boolean), name);
            Assertions.IsNotNull(nameString, nameof(nameString));

            return new BooleanParameter(nameString, definition.Id, isRequired);
        }

        /// <summary>
        /// Creates a <see cref="NetworkAddressParameter" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to create.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the value of the parameter is required.
        /// </param>
        /// <returns>
        /// The parameter.
        /// </returns>
        [Pure]
        public static NetworkAddressParameter Create(ParameterType.NetworkAddress name, bool isRequired)
        {
            ParameterDefinition definition = NetworkAddressMap[name];

            string? nameString = Enum.GetName(typeof(ParameterType.NetworkAddress), name);
            Assertions.IsNotNull(nameString, nameof(nameString));

            return new NetworkAddressParameter(nameString, definition.Id, isRequired);
        }

        /// <summary>
        /// Creates a <see cref="VersionParameter" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to create.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the value of the parameter is required.
        /// </param>
        /// <returns>
        /// The parameter.
        /// </returns>
        [Pure]
        public static VersionParameter Create(ParameterType.Version name, bool isRequired)
        {
            ParameterDefinition definition = VersionMap[name];

            string? nameString = Enum.GetName(typeof(ParameterType.Version), name);
            Assertions.IsNotNull(nameString, nameof(nameString));

            return new VersionParameter(nameString, definition.Id, isRequired);
        }

        /// <summary>
        /// Creates a <see cref="BinaryParameter" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the parameter to create.
        /// </param>
        /// <param name="isRequired">
        /// If set to <c>true</c>, the value of the parameter is required.
        /// </param>
        /// <returns>
        /// The parameter.
        /// </returns>
        [Pure]
        public static BinaryParameter Create(ParameterType.Binary name, bool isRequired)
        {
            ParameterDefinition definition = BinaryMap[name];

            string? nameString = Enum.GetName(typeof(ParameterType.Binary), name);
            Assertions.IsNotNull(nameString, nameof(nameString));

            return new BinaryParameter(nameString, definition.Id, isRequired);
        }

        private class ParameterDefinition
        {
            public int Id { get; }

            public ParameterDefinition(int id)
            {
                Id = id;
            }
        }

        private sealed class IntegerParameterDefinition : ParameterDefinition
        {
            public int MinValue { get; }
            public int MaxValue { get; }

            public IntegerParameterDefinition(int id, int minValue, int maxValue)
                : base(id)
            {
                MinValue = minValue;
                MaxValue = maxValue;
            }
        }
    }
}
