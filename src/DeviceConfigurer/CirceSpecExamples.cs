using System;
using System.Collections.Generic;
using System.Text;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.DeviceConfigurer
{
    /// <summary>
    /// Code generation for examples appendix in CIRCE protocol specification.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // Reason: Used only for rendering example dumps as a result of a CIRCE protocol change.
    internal static class CirceSpecExamples
    {
        public static void DumpScenarios()
        {
            var examplesToDisplay = new Func<IEnumerable<Operation>>[]
            {
                AssigningUniqueNetworkAddresses,
                FormingLogicalNetwork,
                SynchronizingClocks,
                ReportingEventsDuringACompetitionRun
                //GetLargestPossibleOperation
            };

            foreach (Func<IEnumerable<Operation>> displayExample in examplesToDisplay)
            {
                Console.WriteLine();
                foreach (Operation operation in displayExample())
                {
                    Console.WriteLine(HumanReadablePacketFormatter.FormatOperation(operation));
                }
            }
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Operation> AssigningUniqueNetworkAddresses()
        {
            var mediator = new WirelessNetworkAddress("123456");
            var device = new WirelessNetworkAddress("222222");

            Console.WriteLine("Controller <-connect-> Mediator");
            Console.WriteLine("--> Login");
            yield return new LoginOperation();

            Console.WriteLine("<-- Keep Alive (login response indicates unconfigured)");
            yield return new KeepAliveOperation(Version.Parse("1.2.34"), 1);

            Console.WriteLine("--> Device setup (assigns address 123456 to new mediator)");
            yield return new DeviceSetupOperation(mediator);

            Console.WriteLine("<-- Keep Alive (indicates configured mediator)");
            yield return new KeepAliveOperation(Version.Parse("1.2.34"), 0);

            Console.WriteLine(
                "--> Device setup (assigns address 222222 and capability TimeSensor to new wireless device)");
            yield return new DeviceSetupOperation(device) { Capabilities = DeviceCapabilities.TimeSensor };

            Console.WriteLine("<-- Notify Status (indicates configured wireless device)");
            yield return new NotifyStatusOperation(device, false, DeviceCapabilities.TimeSensor, DeviceRoles.None, 150);

            Console.WriteLine("--> Logout");
            yield return new LogoutOperation();
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Operation> FormingLogicalNetwork()
        {
            var remote = new WirelessNetworkAddress("AAAAAA");
            var gate1 = new WirelessNetworkAddress("E1E1E1");
            var gate2 = new WirelessNetworkAddress("E2E2E2");

            Console.WriteLine("Controller <-connect-> Mediator");
            Console.WriteLine("--> Login");
            yield return new LoginOperation();

            Console.WriteLine("<-- Keep Alive (login response)");
            yield return new KeepAliveOperation(Version.Parse("1.2.34"), 0);

            Console.WriteLine("<-- Notify Status (remote control)");
            yield return
                new NotifyStatusOperation(remote, false, DeviceCapabilities.ControlKeypad, DeviceRoles.None, 200);

            Console.WriteLine("<-- Notify Status (first gate)");
            yield return new NotifyStatusOperation(gate1, false, DeviceCapabilities.TimeSensor, DeviceRoles.None, 150);

            Console.WriteLine("<-- Notify Status (second gate)");
            yield return new NotifyStatusOperation(gate2, false, DeviceCapabilities.TimeSensor, DeviceRoles.None, 130);

            Console.WriteLine("--> Alert (first gate)");
            yield return new AlertOperation(gate1);

            Console.WriteLine("--> Network Setup (remote control joins logical network)");
            yield return new NetworkSetupOperation(remote, true, DeviceRoles.Keypad);

            Console.WriteLine("--> Network Setup (gate1 joins logical network as start timer)");
            yield return new NetworkSetupOperation(gate1, true, DeviceRoles.StartTimer);

            Console.WriteLine("--> Network Setup (gate2 joins logical network as finish timer)");
            yield return new NetworkSetupOperation(gate2, true, DeviceRoles.FinishTimer);
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Operation> SynchronizingClocks()
        {
            var remote = new WirelessNetworkAddress("AAAAAA");
            var gate1 = new WirelessNetworkAddress("E1E1E1");

            Console.WriteLine("Controller <-connect-> Mediator");
            Console.WriteLine("--> Login");
            yield return new LoginOperation();

            Console.WriteLine("<-- Keep Alive (login response)");
            yield return new KeepAliveOperation(Version.Parse("1.2.34"), 0);

            Console.WriteLine("<-- Notify Status (remote control)");
            yield return
                new NotifyStatusOperation(remote, false,
                    DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad | DeviceCapabilities.StartSensor |
                        DeviceCapabilities.FinishSensor | DeviceCapabilities.IntermediateSensor,
                    DeviceRoles.Keypad | DeviceRoles.StartTimer, 200);

            Console.WriteLine("<-- Notify Status (gate, asking for sync)");
            yield return
                new NotifyStatusOperation(gate1, false, DeviceCapabilities.TimeSensor, DeviceRoles.FinishTimer, 150)
                {
                    ClockSynchronization = ClockSynchronizationStatus.RequiresSync
                };

            Console.WriteLine("--> Synchronize clocks");
            yield return new SynchronizeClocksOperation();

            Console.WriteLine("<-- Notify Status (remote control, synced)");
            yield return
                new NotifyStatusOperation(remote, false,
                    DeviceCapabilities.ControlKeypad | DeviceCapabilities.NumericKeypad | DeviceCapabilities.StartSensor |
                        DeviceCapabilities.FinishSensor | DeviceCapabilities.IntermediateSensor,
                    DeviceRoles.Keypad | DeviceRoles.StartTimer, 200)
                {
                    ClockSynchronization = ClockSynchronizationStatus.SyncSucceeded
                };

            Console.WriteLine("<-- Notify Status (gate, synced)");
            yield return
                new NotifyStatusOperation(gate1, false, DeviceCapabilities.TimeSensor, DeviceRoles.FinishTimer, 150)
                {
                    ClockSynchronization = ClockSynchronizationStatus.SyncSucceeded
                };
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Operation> ReportingEventsDuringACompetitionRun()
        {
            var remote = new WirelessNetworkAddress("AAAAAA");
            var gate1 = new WirelessNetworkAddress("E1E1E1");
            var gate2 = new WirelessNetworkAddress("E2E2E2");

            Console.WriteLine("Controller <-connect-> Mediator");
            Console.WriteLine("--> Login");
            yield return new LoginOperation();

            Console.WriteLine("<-- Keep Alive (login response)");
            yield return new KeepAliveOperation(Version.Parse("1.2.34"), 0);

            Console.WriteLine("<-- Notify Action (Ready key pressed)");
            yield return new NotifyActionOperation(remote) { InputKeys = RawDeviceKeys.Ready };
            yield return new NotifyActionOperation(remote) { InputKeys = RawDeviceKeys.None };

            Console.WriteLine("<-- Notify Action (gate1 start timer)");
            yield return new NotifyActionOperation(gate1) { SensorTime = TimeSpan.FromSeconds(5) };

            Console.WriteLine("<-- Notify Action (gate2 finish timer)");
            yield return new NotifyActionOperation(gate2) { SensorTime = TimeSpan.FromSeconds(32) };
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<Operation> GetLargestPossibleOperation()
        {
            var destinations = new[]
            {
                //new WirelessNetworkAddress("AAAAAA"),
                //new WirelessNetworkAddress("BBBBBB"),
                new WirelessNetworkAddress("CCCCCC")
            };

            var operation = new VisualizeOperation(destinations)
            {
                CurrentCompetitorNumber = 999,
                NextCompetitorNumber = 999,
                StartTimer = true,
                PrimaryTimerValue = TimeSpan.FromSeconds(999).Add(TimeSpan.FromMilliseconds(987)),
                SecondaryTimerValue = TimeSpan.FromSeconds(999).Add(TimeSpan.FromMilliseconds(987)),
                FaultCount = 99,
                RefusalCount = 99,
                Eliminated = true,
                PreviousPlacement = 999
            };

            byte[] buffer = PacketWriter.Write(operation, true);
            Console.WriteLine($"*** Maximum operation length in bytes: {buffer.Length}\n");
            yield break;
        }

        private static class HumanReadablePacketFormatter
        {
            [NotNull]
            private static readonly string StartOfText = new string((char) PacketFormatDelimiters.StartOfText, 1);

            [NotNull]
            private static readonly string EndOfText = new string((char) PacketFormatDelimiters.EndOfText, 1);

            [NotNull]
            private static readonly string Tab = new string((char) PacketFormatDelimiters.Tab, 1);

            [NotNull]
            public static string FormatOperation([NotNull] Operation operation)
            {
                Guard.NotNull(operation, nameof(operation));

                byte[] packet = PacketWriter.Write(operation, false);
                return FormatPacket(packet);
            }

            [NotNull]
            private static string FormatPacket([NotNull] byte[] packet)
            {
                string text = new string(Encoding.ASCII.GetChars(packet));
                text = text.Replace(StartOfText, "<STX>").Replace(EndOfText, "<ETX>").Replace(Tab, "<TAB>");

                if (text.EndsWith("<ETX>", StringComparison.Ordinal) &&
                    !text.EndsWith("<CC><ETX>", StringComparison.Ordinal))
                {
                    text = text.Replace("<ETX>", "<CC><ETX>");
                }

                return text;
            }
        }
    }
}