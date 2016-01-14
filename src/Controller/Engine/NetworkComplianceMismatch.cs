using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Lists various logical network composition problems that prevent from starting a competition run.
    /// </summary>
    public sealed class NetworkComplianceMismatch
    {
        [NotNull]
        public string Name { get; }

        [NotNull]
        public string Message { get; }

        [CanBeNull]
        public WirelessNetworkAddress DeviceAddress { get; }

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingKeypad = new NetworkComplianceMismatch("MissingKeypad",
            "Network must contain a device in role Keypad.");

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingStartTimer =
            new NetworkComplianceMismatch("MissingStartTimer", "Network must contain a device in role StartTimer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingIntermediate1Timer =
            new NetworkComplianceMismatch("MissingIntermediate1Timer",
                "Network must contain a device in role Intermediate 1 Timer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingIntermediate2Timer =
            new NetworkComplianceMismatch("MissingIntermediate2Timer",
                "Network must contain a device in role Intermediate 2 Timer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingIntermediate3Timer =
            new NetworkComplianceMismatch("MissingIntermediate3Timer",
                "Network must contain a device in role Intermediate 3 Timer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingFinishTimer =
            new NetworkComplianceMismatch("MissingFinishTimer", "Network must contain a device in role FinishTimer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch MissingDelayForStartFinishTimer =
            new NetworkComplianceMismatch("MissingDelayForStartFinishTimer",
                "Must specify minimum delay between passage of Start and Finish when using single Start/Finish gate.");

        [NotNull]
        public static readonly NetworkComplianceMismatch GateIsStartAndIntermediate =
            new NetworkComplianceMismatch("GateIsStartAndIntermediate",
                "Gate cannot be in both roles Start Timer and Intermediate Timer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch GateIsFinishAndIntermediate =
            new NetworkComplianceMismatch("GateIsFinishAndIntermediate",
                "Gate cannot be in both roles Intermediate Timer and Finish Timer.");

        [NotNull]
        public static readonly NetworkComplianceMismatch GateInMultipleIntermediateTimerRoles =
            new NetworkComplianceMismatch("GateInMultipleIntermediateTimerRoles",
                "Gate cannot be in role Intermediate 1/2/3 Timer at the same time.");

        [NotNull]
        public static readonly NetworkComplianceMismatch IntermediateMissing32 =
            new NetworkComplianceMismatch("IntermediateMissing32",
                "Network cannot contain a device in role Intermediate 3 without another device in role Intermediate 2.");

        [NotNull]
        public static readonly NetworkComplianceMismatch IntermediateMissing21 =
            new NetworkComplianceMismatch("IntermediateMissing21",
                "Network cannot contain a device in role Intermediate 2 without another device in role Intermediate 1.");

        private NetworkComplianceMismatch([NotNull] string name, [NotNull] string message,
            [CanBeNull] WirelessNetworkAddress deviceAddress = null)
        {
            Guard.NotNullNorEmpty(name, nameof(name));
            Guard.NotNullNorEmpty(message, nameof(message));

            Name = name;
            Message = message;
            DeviceAddress = deviceAddress;
        }

        [NotNull]
        public static NetworkComplianceMismatch CreateForOfflineDevice([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            return new NetworkComplianceMismatch("DeviceOffline", $"Device {deviceAddress} has gone off-line.",
                deviceAddress);
        }

        [Pure]
        public override string ToString() => DeviceAddress != null ? $"{Name} ({DeviceAddress})" : Name;
    }
}