using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// The set of devices that forms a logical network, which is used in a competition run.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class NetworkComposition : IEquatable<NetworkComposition>
    {
        private static readonly IReadOnlyCollection<DeviceRoles> BuiltinRoles = new ReadOnlyCollection<DeviceRoles>(Enum.GetValues(typeof(DeviceRoles))
            .Cast<DeviceRoles>().Except(new[]
            {
                DeviceRoles.None
            }).ToList());

        public static readonly NetworkComposition Empty =
            new(new Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities>(), CompetitionClassRequirements.Default);

        private readonly Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities> rolesPerDevice;

        public CompetitionClassRequirements Requirements { get; }

        private NetworkComposition(Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities> rolesPerDevice, CompetitionClassRequirements requirements)
        {
            this.rolesPerDevice = rolesPerDevice;
            Requirements = requirements;
        }

        [AssertionMethod]
        public IList<NetworkComplianceMismatch> AssertComplianceWithRequirements()
        {
            return Requirements.AssertComplianceWith(this);
        }

        public bool ContainsDevice(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return rolesPerDevice.ContainsKey(deviceAddress);
        }

        public IEnumerable<WirelessNetworkAddress> GetDeviceAddresses()
        {
            return rolesPerDevice.Keys;
        }

        public bool RequiresClockSynchronization(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return rolesPerDevice.ContainsKey(deviceAddress) && rolesPerDevice[deviceAddress].RequiresClockSynchronization;
        }

        public IEnumerable<WirelessNetworkAddress> GetDevicesInAnyRole(DeviceRoles roles)
        {
            AssertNotNone(roles);

            var devices = new HashSet<WirelessNetworkAddress>();

            foreach (KeyValuePair<WirelessNetworkAddress, DeviceRolesWithCapabilities> pair in rolesPerDevice)
            {
                foreach (DeviceRoles nextRole in BuiltinRoles)
                {
                    if ((roles & nextRole) == nextRole && (pair.Value.Roles & nextRole) == nextRole)
                    {
                        devices.Add(pair.Key);
                    }
                }
            }

            return devices;
        }

        [AssertionMethod]
        private static void AssertNotNone(DeviceRoles roles)
        {
            if (roles == DeviceRoles.None)
            {
                throw new ArgumentException("Specify one or more roles.", nameof(roles));
            }
        }

        public bool IsStartFinishGate(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            bool isTimeSensor = HasCapability(deviceAddress, DeviceCapabilities.TimeSensor);
            return isTimeSensor && IsInRoleStartTimer(deviceAddress) && IsInRoleFinishTimer(deviceAddress);
        }

        public bool HasCapability(WirelessNetworkAddress deviceAddress, DeviceCapabilities capability)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));
            AssertNotNone(capability);

            return rolesPerDevice.ContainsKey(deviceAddress) && (rolesPerDevice[deviceAddress].Capabilities & capability) == capability;
        }

        [AssertionMethod]
        private static void AssertNotNone(DeviceCapabilities capability)
        {
            if (capability == DeviceCapabilities.None)
            {
                throw new ArgumentException("Specify a capability.", nameof(capability));
            }
        }

        public bool IsInRoleKeypad(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.Keypad);
        }

        public bool IsInRoleStartTimer(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.StartTimer);
        }

        public bool IsInRoleFinishTimer(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.FinishTimer);
        }

        public bool IsInRoleIntermediateTimer1(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.IntermediateTimer1);
        }

        public bool IsInRoleIntermediateTimer2(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.IntermediateTimer2);
        }

        public bool IsInRoleIntermediateTimer3(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.IntermediateTimer3);
        }

        public bool IsInRoleDisplay(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.Display);
        }

        private bool IsInRole(WirelessNetworkAddress deviceAddress, DeviceRoles role)
        {
            return rolesPerDevice.ContainsKey(deviceAddress) && (rolesPerDevice[deviceAddress].Roles & role) == role;
        }

        public NetworkComposition ChangeRequirements(CompetitionClassRequirements classRequirements)
        {
            Guard.NotNull(classRequirements, nameof(classRequirements));

            return new NetworkComposition(rolesPerDevice, classRequirements);
        }

        public NetworkComposition ChangeRolesFor(WirelessNetworkAddress deviceAddress, DeviceCapabilities capabilities, DeviceRoles roles)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            var newRolesPerDevice = new Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities>(rolesPerDevice)
            {
                [deviceAddress] = new(capabilities, roles)
            };

            return new NetworkComposition(newRolesPerDevice, Requirements);
        }

        [Pure]
        public bool Equals(NetworkComposition? other)
        {
            return !ReferenceEquals(other, null) && rolesPerDevice.SequenceEqual(other.rolesPerDevice) && Requirements == other.Requirements;
        }

        [Pure]
        public override bool Equals(object? obj)
        {
            return Equals(obj as NetworkComposition);
        }

        [Pure]
        public override int GetHashCode()
        {
            return rolesPerDevice.GetHashCode() ^ Requirements.GetHashCode();
        }

        [Pure]
        public static bool operator ==(NetworkComposition? left, NetworkComposition? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        [Pure]
        public static bool operator !=(NetworkComposition? left, NetworkComposition? right)
        {
            return !(left == right);
        }

        private sealed class DeviceRolesWithCapabilities
        {
            private const DeviceRoles RolesRequiringClockSynchronization = DeviceRoles.StartTimer | DeviceRoles.FinishTimer | DeviceRoles.IntermediateTimer1 |
                DeviceRoles.IntermediateTimer2 | DeviceRoles.IntermediateTimer3;

            public DeviceCapabilities Capabilities { get; }
            public DeviceRoles Roles { get; }

            public bool RequiresClockSynchronization => (Roles & RolesRequiringClockSynchronization) != 0;

            public DeviceRolesWithCapabilities(DeviceCapabilities capabilities, DeviceRoles roles)
            {
                Capabilities = capabilities;
                Roles = roles;
            }
        }
    }
}
