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
        [NotNull]
        private static readonly IReadOnlyCollection<DeviceRoles> BuiltinRoles =
            new ReadOnlyCollection<DeviceRoles>(
                Enum.GetValues(typeof (DeviceRoles)).Cast<DeviceRoles>().Except(new[] { DeviceRoles.None }).ToList());

        [NotNull]
        private readonly Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities> rolesPerDevice;

        [NotNull]
        public static readonly NetworkComposition Empty =
            new NetworkComposition(new Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities>(),
                CompetitionClassRequirements.Default);

        [NotNull]
        public CompetitionClassRequirements Requirements { get; }

        private NetworkComposition(
            [NotNull] Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities> rolesPerDevice,
            [NotNull] CompetitionClassRequirements requirements)
        {
            this.rolesPerDevice = rolesPerDevice;
            Requirements = requirements;
        }

        [AssertionMethod]
        [NotNull]
        [ItemNotNull]
        public IList<NetworkComplianceMismatch> AssertComplianceWithRequirements()
        {
            return Requirements.AssertComplianceWith(this);
        }

        public bool ContainsDevice([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return rolesPerDevice.ContainsKey(deviceAddress);
        }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<WirelessNetworkAddress> GetDeviceAddresses()
        {
            return rolesPerDevice.Keys;
        }

        public bool RequiresClockSynchronization([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return rolesPerDevice.ContainsKey(deviceAddress) &&
                rolesPerDevice[deviceAddress].RequiresClockSynchronization;
        }

        [NotNull]
        [ItemNotNull]
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
                throw new ArgumentException(@"Specify one or more roles.", nameof(roles));
            }
        }

        public bool IsStartFinishGate([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            bool isTimeSensor = HasCapability(deviceAddress, DeviceCapabilities.TimeSensor);
            return isTimeSensor && IsInRoleStartTimer(deviceAddress) && IsInRoleFinishTimer(deviceAddress);
        }

        public bool HasCapability([NotNull] WirelessNetworkAddress deviceAddress, DeviceCapabilities capability)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));
            AssertNotNone(capability);

            return rolesPerDevice.ContainsKey(deviceAddress) &&
                (rolesPerDevice[deviceAddress].Capabilities & capability) == capability;
        }

        [AssertionMethod]
        private static void AssertNotNone(DeviceCapabilities capability)
        {
            if (capability == DeviceCapabilities.None)
            {
                throw new ArgumentException(@"Specify a capability.", nameof(capability));
            }
        }

        public bool IsInRoleKeypad([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.Keypad);
        }

        public bool IsInRoleStartTimer([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.StartTimer);
        }

        public bool IsInRoleFinishTimer([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.FinishTimer);
        }

        public bool IsInRoleIntermediateTimer1([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.IntermediateTimer1);
        }

        public bool IsInRoleIntermediateTimer2([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.IntermediateTimer2);
        }

        public bool IsInRoleIntermediateTimer3([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.IntermediateTimer3);
        }

        public bool IsInRoleDisplay([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return IsInRole(deviceAddress, DeviceRoles.Display);
        }

        private bool IsInRole([NotNull] WirelessNetworkAddress deviceAddress, DeviceRoles role)
        {
            return rolesPerDevice.ContainsKey(deviceAddress) && (rolesPerDevice[deviceAddress].Roles & role) == role;
        }

        [NotNull]
        public NetworkComposition ChangeRequirements([NotNull] CompetitionClassRequirements classRequirements)
        {
            Guard.NotNull(classRequirements, nameof(classRequirements));

            return new NetworkComposition(rolesPerDevice, classRequirements);
        }

        [NotNull]
        public NetworkComposition ChangeRolesFor([NotNull] WirelessNetworkAddress deviceAddress,
            DeviceCapabilities capabilities, DeviceRoles roles)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            var newRolesPerDevice = new Dictionary<WirelessNetworkAddress, DeviceRolesWithCapabilities>(rolesPerDevice)
            {
                [deviceAddress] = new DeviceRolesWithCapabilities(capabilities, roles)
            };

            return new NetworkComposition(newRolesPerDevice, Requirements);
        }

        [Pure]
        public bool Equals([CanBeNull] NetworkComposition other)
        {
            return !ReferenceEquals(other, null) && rolesPerDevice.SequenceEqual(other.rolesPerDevice) &&
                Requirements == other.Requirements;
        }

        [Pure]
        public override bool Equals([CanBeNull] object obj)
        {
            return Equals(obj as NetworkComposition);
        }

        [Pure]
        public override int GetHashCode()
        {
            return rolesPerDevice.GetHashCode() ^ Requirements.GetHashCode();
        }

        [Pure]
        public static bool operator ==([CanBeNull] NetworkComposition left, [CanBeNull] NetworkComposition right)
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
        public static bool operator !=([CanBeNull] NetworkComposition left, [CanBeNull] NetworkComposition right)
        {
            return !(left == right);
        }

        private sealed class DeviceRolesWithCapabilities
        {
            public DeviceCapabilities Capabilities { get; }
            public DeviceRoles Roles { get; }

            private const DeviceRoles RolesRequiringClockSynchronization =
                DeviceRoles.StartTimer | DeviceRoles.FinishTimer | DeviceRoles.IntermediateTimer1 |
                    DeviceRoles.IntermediateTimer2 | DeviceRoles.IntermediateTimer3;

            public bool RequiresClockSynchronization => (Roles & RolesRequiringClockSynchronization) != 0;

            public DeviceRolesWithCapabilities(DeviceCapabilities capabilities, DeviceRoles roles)
            {
                Capabilities = capabilities;
                Roles = roles;
            }
        }
    }
}