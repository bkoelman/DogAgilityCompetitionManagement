using System;
using System.Collections.Generic;
using System.Linq;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Defines under what conditions a competition class is allowed to be started.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class CompetitionClassRequirements : IEquatable<CompetitionClassRequirements>
    {
        [NotNull]
        public static readonly CompetitionClassRequirements Default = new(0, TimeSpan.Zero);

        public int IntermediateTimerCount { get; }

        public TimeSpan StartFinishMinDelayForSingleSensor { get; }

        private CompetitionClassRequirements(int intermediateTimerCount, TimeSpan startFinishMinDelayForSingleSensor)
        {
            IntermediateTimerCount = intermediateTimerCount;
            StartFinishMinDelayForSingleSensor = startFinishMinDelayForSingleSensor;
        }

        [NotNull]
        public CompetitionClassRequirements ChangeIntermediateTimerCount(int intermediateTimerCount)
        {
            Guard.InRangeInclusive(intermediateTimerCount, nameof(intermediateTimerCount), 0, 3);

            return new CompetitionClassRequirements(intermediateTimerCount, StartFinishMinDelayForSingleSensor);
        }

        [NotNull]
        public CompetitionClassRequirements ChangeStartFinishMinDelayForSingleSensor(TimeSpan startFinishMinDelayForSingleSensor)
        {
            AssertNotNegative(startFinishMinDelayForSingleSensor);

            return new CompetitionClassRequirements(IntermediateTimerCount, startFinishMinDelayForSingleSensor);
        }

        [AssertionMethod]
        private static void AssertNotNegative(TimeSpan startFinishMinDelayForSingleSensor)
        {
            if (startFinishMinDelayForSingleSensor < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(startFinishMinDelayForSingleSensor), startFinishMinDelayForSingleSensor,
                    "Minimum delay between passage of Start and Finish sensors cannot be negative.");
            }
        }

        [AssertionMethod]
        [NotNull]
        [ItemNotNull]
        public IList<NetworkComplianceMismatch> AssertComplianceWith([NotNull] NetworkComposition composition)
        {
            Guard.NotNull(composition, nameof(composition));

            var mismatches = new List<NetworkComplianceMismatch>();

            // Must have at least one device in role KEYPAD.
            if (!composition.GetDevicesInAnyRole(DeviceRoles.Keypad).Any())
            {
                mismatches.Add(NetworkComplianceMismatch.MissingKeypad);
            }

            // Must have at least one device in role START that can report times.
            if (!composition.GetDevicesInAnyRole(DeviceRoles.StartTimer).Any())
            {
                mismatches.Add(NetworkComplianceMismatch.MissingStartTimer);
            }

            // Must have at least one device in role FINISH that can report times.
            if (!composition.GetDevicesInAnyRole(DeviceRoles.FinishTimer).Any())
            {
                mismatches.Add(NetworkComplianceMismatch.MissingFinishTimer);
            }

            if (StartFinishMinDelayForSingleSensor == TimeSpan.Zero)
            {
                // A gate cannot be both START and FINISH sensor when no minimum delay between passage 
                // of Start and Finish sensors has been specified.
                if (composition.GetDeviceAddresses().Any(composition.IsStartFinishGate))
                {
                    mismatches.Add(NetworkComplianceMismatch.MissingDelayForStartFinishTimer);
                }
            }

            // Can have multiple INTERMEDIATE roles.
            const DeviceRoles intermediateTimers = DeviceRoles.IntermediateTimer1 | DeviceRoles.IntermediateTimer2 | DeviceRoles.IntermediateTimer3;

            foreach (WirelessNetworkAddress deviceAddress in composition.GetDevicesInAnyRole(intermediateTimers))
            {
                if (composition.HasCapability(deviceAddress, DeviceCapabilities.TimeSensor))
                {
                    // Gates cannot be both INTERMEDIATE and START/FINISH.
                    if (composition.IsInRoleStartTimer(deviceAddress))
                    {
                        mismatches.Add(NetworkComplianceMismatch.GateIsStartAndIntermediate);
                    }

                    if (composition.IsInRoleFinishTimer(deviceAddress))
                    {
                        mismatches.Add(NetworkComplianceMismatch.GateIsFinishAndIntermediate);
                    }

                    // Gates cannot be in more than one INTERMEDIATE role.
                    bool inRoleInt12 = composition.IsInRoleIntermediateTimer1(deviceAddress) && composition.IsInRoleIntermediateTimer2(deviceAddress);
                    bool inRoleInt23 = composition.IsInRoleIntermediateTimer2(deviceAddress) && composition.IsInRoleIntermediateTimer3(deviceAddress);
                    bool inRoleInt13 = composition.IsInRoleIntermediateTimer1(deviceAddress) && composition.IsInRoleIntermediateTimer3(deviceAddress);

                    if (inRoleInt12 || inRoleInt23 || inRoleInt13)
                    {
                        mismatches.Add(NetworkComplianceMismatch.GateInMultipleIntermediateTimerRoles);
                    }
                }
            }

            // Must possibly have various INTERMEDIATE roles (based on class requirements).
            if (IntermediateTimerCount >= 3 && !composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer3).Any())
            {
                mismatches.Add(NetworkComplianceMismatch.MissingIntermediate3Timer);
            }

            if (IntermediateTimerCount >= 2 && !composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer2).Any())
            {
                mismatches.Add(NetworkComplianceMismatch.MissingIntermediate2Timer);
            }

            if (IntermediateTimerCount >= 1 && !composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer1).Any())
            {
                mismatches.Add(NetworkComplianceMismatch.MissingIntermediate1Timer);
            }

            // Cannot have role INTERMEDIATE 3 without INTERMEDIATE 2 in the network.
            // Cannot have role INTERMEDIATE 2 without INTERMEDIATE 1 in the network.
            if (composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer3).Any())
            {
                if (!composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer2).Any())
                {
                    mismatches.Add(NetworkComplianceMismatch.IntermediateMissing32);
                }
            }

            if (composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer2).Any())
            {
                if (!composition.GetDevicesInAnyRole(DeviceRoles.IntermediateTimer1).Any())
                {
                    mismatches.Add(NetworkComplianceMismatch.IntermediateMissing21);
                }
            }

            // Can have multiple DISPLAY roles.

            return mismatches;
        }

        public bool Equals(CompetitionClassRequirements other)
        {
            return !ReferenceEquals(other, null) && IntermediateTimerCount == other.IntermediateTimerCount &&
                StartFinishMinDelayForSingleSensor == other.StartFinishMinDelayForSingleSensor;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CompetitionClassRequirements);
        }

        public override int GetHashCode()
        {
            return IntermediateTimerCount.GetHashCode() ^ StartFinishMinDelayForSingleSensor.GetHashCode();
        }

        public static bool operator ==([CanBeNull] CompetitionClassRequirements left, [CanBeNull] CompetitionClassRequirements right)
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

        public static bool operator !=([CanBeNull] CompetitionClassRequirements left, [CanBeNull] CompetitionClassRequirements right)
        {
            return !(left == right);
        }
    }
}
