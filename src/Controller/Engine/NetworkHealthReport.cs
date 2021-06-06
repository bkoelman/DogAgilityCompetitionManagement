using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Contains status information on the wireless network at a specific point in time.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class NetworkHealthReport : IEquatable<NetworkHealthReport>
    {
        private static readonly IEnumerable<WirelessNetworkAddress> NoAddresses = Array.Empty<WirelessNetworkAddress>();

        public static readonly NetworkHealthReport Default = new(false, false, 0, NoAddresses, NoAddresses, NoAddresses, null, null);

        public bool IsConnected { get; }
        public bool HasProtocolVersionMismatch { get; }
        public int MediatorStatus { get; }
        public IReadOnlyCollection<WirelessNetworkAddress> MisalignedSensors { get; }
        public IReadOnlyCollection<WirelessNetworkAddress> UnsyncedSensors { get; }
        public IReadOnlyCollection<WirelessNetworkAddress> VersionMismatchingSensors { get; }

        // The composition used by the current run, or null when no competition run is active.
        public NetworkComposition? RunComposition { get; }

        // Empty when no mismatches; null when not applicable (class requirements unknown).
        public IReadOnlyCollection<NetworkComplianceMismatch>? ClassCompliance { get; }

        public bool HasErrors => MediatorStatus != KnownMediatorStatusCode.Normal || MisalignedSensors.Count > 0 || ClassCompliance is { Count: > 0 };

        private NetworkHealthReport(bool isConnected, bool hasProtocolVersionMismatch, int mediatorStatus,
            IEnumerable<WirelessNetworkAddress> misalignedSensors, IEnumerable<WirelessNetworkAddress> unsyncedSensors,
            IEnumerable<WirelessNetworkAddress> versionMismatchingSensors, NetworkComposition? runComposition,
            IEnumerable<NetworkComplianceMismatch>? classCompliance)
        {
            IsConnected = isConnected;
            HasProtocolVersionMismatch = hasProtocolVersionMismatch;
            MediatorStatus = mediatorStatus;
            RunComposition = runComposition;
            MisalignedSensors = new ReadOnlyCollection<WirelessNetworkAddress>(misalignedSensors.ToList());
            UnsyncedSensors = new ReadOnlyCollection<WirelessNetworkAddress>(unsyncedSensors.ToList());
            VersionMismatchingSensors = new ReadOnlyCollection<WirelessNetworkAddress>(versionMismatchingSensors.ToList());
            ClassCompliance = classCompliance == null ? null : new ReadOnlyCollection<NetworkComplianceMismatch>(classCompliance.ToList());
        }

        public NetworkHealthReport ChangeIsConnected(bool isConnected)
        {
            return new(isConnected, HasProtocolVersionMismatch, MediatorStatus, MisalignedSensors, UnsyncedSensors, VersionMismatchingSensors, RunComposition,
                ClassCompliance);
        }

        public NetworkHealthReport ChangeHasProtocolVersionMismatch(bool hasProtocolVersionMismatch)
        {
            return new(IsConnected, hasProtocolVersionMismatch, MediatorStatus, MisalignedSensors, UnsyncedSensors, VersionMismatchingSensors, RunComposition,
                ClassCompliance);
        }

        public NetworkHealthReport ChangeMediatorStatus(int mediatorStatus)
        {
            return new(IsConnected, HasProtocolVersionMismatch, mediatorStatus, MisalignedSensors, UnsyncedSensors, VersionMismatchingSensors, RunComposition,
                ClassCompliance);
        }

        public NetworkHealthReport ChangeMisalignedSensors(IEnumerable<WirelessNetworkAddress> misalignedSensors)
        {
            Guard.NotNull(misalignedSensors, nameof(misalignedSensors));

            return new NetworkHealthReport(IsConnected, HasProtocolVersionMismatch, MediatorStatus, misalignedSensors, UnsyncedSensors,
                VersionMismatchingSensors, RunComposition, ClassCompliance);
        }

        public NetworkHealthReport ChangeUnsyncedSensors(IEnumerable<WirelessNetworkAddress> unsyncedSensors)
        {
            Guard.NotNull(unsyncedSensors, nameof(unsyncedSensors));

            return new NetworkHealthReport(IsConnected, HasProtocolVersionMismatch, MediatorStatus, MisalignedSensors, unsyncedSensors,
                VersionMismatchingSensors, RunComposition, ClassCompliance);
        }

        public NetworkHealthReport ChangeVersionMismatchingSensors(IEnumerable<WirelessNetworkAddress> versionMismatchingSensors)
        {
            Guard.NotNull(versionMismatchingSensors, nameof(versionMismatchingSensors));

            return new NetworkHealthReport(IsConnected, HasProtocolVersionMismatch, MediatorStatus, MisalignedSensors, UnsyncedSensors,
                versionMismatchingSensors, RunComposition, ClassCompliance);
        }

        public NetworkHealthReport ChangeRunComposition(NetworkComposition? runComposition)
        {
            return new(IsConnected, HasProtocolVersionMismatch, MediatorStatus, MisalignedSensors, UnsyncedSensors, VersionMismatchingSensors, runComposition,
                ClassCompliance);
        }

        public NetworkHealthReport ChangeClassCompliance(IEnumerable<NetworkComplianceMismatch>? classCompliance)
        {
            return new(IsConnected, HasProtocolVersionMismatch, MediatorStatus, MisalignedSensors, UnsyncedSensors, VersionMismatchingSensors, RunComposition,
                classCompliance);
        }

        [Pure]
        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            using (var formatter = new ObjectFormatter(textBuilder, this))
            {
                formatter.Append(() => IsConnected, () => IsConnected);
                formatter.Append(() => HasProtocolVersionMismatch, () => HasProtocolVersionMismatch);
                formatter.Append(() => MediatorStatus, () => MediatorStatus);
                formatter.Append(() => MisalignedSensors.Count, () => MisalignedSensors);
                formatter.Append(() => UnsyncedSensors.Count, () => UnsyncedSensors);
                formatter.Append(() => VersionMismatchingSensors.Count, () => VersionMismatchingSensors);
                formatter.Append(() => RunComposition == null ? "null" : "...", () => RunComposition);
                formatter.Append(GetClassComplianceString, () => ClassCompliance);
            }

            return textBuilder.ToString();
        }

        private string GetClassComplianceString()
        {
            if (ClassCompliance == null)
            {
                return "null";
            }

            var textBuilder = new StringBuilder();
            textBuilder.Append('[');

            IEnumerable<string> items = ClassCompliance.Select(m => m.ToString());
            textBuilder.Append(string.Join(", ", items));

            textBuilder.Append(']');
            return textBuilder.ToString();
        }

        [Pure]
        public bool Equals(NetworkHealthReport? other)
        {
            return !ReferenceEquals(other, null) && IsConnected == other.IsConnected && HasProtocolVersionMismatch == other.HasProtocolVersionMismatch &&
                MediatorStatus == other.MediatorStatus && MisalignedSensors.SequenceEqual(other.MisalignedSensors) &&
                UnsyncedSensors.SequenceEqual(other.UnsyncedSensors) && VersionMismatchingSensors.SequenceEqual(other.VersionMismatchingSensors) &&
                RunCompositionEquals(other.RunComposition) && ClassComplianceEquals(other.ClassCompliance);
        }

        private bool RunCompositionEquals(NetworkComposition? other)
        {
            if (ReferenceEquals(RunComposition, other))
            {
                // Both null or same instance.
                return true;
            }

            if (RunComposition != null)
            {
                return RunComposition == other;
            }

            return false;
        }

        private bool ClassComplianceEquals(IEnumerable<NetworkComplianceMismatch>? other)
        {
            if (ReferenceEquals(ClassCompliance, other))
            {
                // Both null or same instance.
                return true;
            }

            if (ClassCompliance != null && other != null)
            {
                return ClassCompliance.SequenceEqual(other);
            }

            return false;
        }

        [Pure]
        public override bool Equals(object? obj)
        {
            return Equals(obj as NetworkHealthReport);
        }

        [Pure]
        public override int GetHashCode()
        {
            return IsConnected.GetHashCode() ^ HasProtocolVersionMismatch.GetHashCode() ^ MediatorStatus.GetHashCode() ^ MisalignedSensors.GetHashCode() ^
                UnsyncedSensors.GetHashCode() ^ VersionMismatchingSensors.GetHashCode() ^ (RunComposition?.GetHashCode() ?? 0) ^
                (ClassCompliance?.GetHashCode() ?? 0);
        }

        [Pure]
        public static bool operator ==(NetworkHealthReport? left, NetworkHealthReport? right)
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
        public static bool operator !=(NetworkHealthReport? left, NetworkHealthReport? right)
        {
            return !(left == right);
        }
    }
}
