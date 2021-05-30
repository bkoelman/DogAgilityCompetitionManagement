using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.DeviceConfigurer
{
    /// <summary>
    /// Provides typed access to command-line arguments.
    /// </summary>
    public sealed class StartupArguments
    {
        public string ComPortName { get; }
        public WirelessNetworkAddress? OldAddress { get; }
        public WirelessNetworkAddress NewAddress { get; }
        public DeviceCapabilities? Capabilities { get; }

        private StartupArguments(string comPortName, WirelessNetworkAddress? oldAddress, WirelessNetworkAddress newAddress, DeviceCapabilities? capabilities)
        {
            ComPortName = comPortName;
            OldAddress = oldAddress;
            NewAddress = newAddress;
            Capabilities = capabilities;
        }

        public static StartupArguments? Parse(IEnumerable<string> args)
        {
            Guard.NotNull(args, nameof(args));

            string? comPortName = null;
            WirelessNetworkAddress? oldAddress = null;
            WirelessNetworkAddress? newAddress = null;
            DeviceCapabilities? capabilities = null;

            foreach (string arg in args)
            {
                if (arg.StartsWith("port=", StringComparison.OrdinalIgnoreCase))
                {
                    comPortName = arg.Substring("port=".Length).ToUpperInvariant();
                }

                if (arg.StartsWith("new=", StringComparison.OrdinalIgnoreCase))
                {
                    newAddress = ParseAddress(arg.Substring("new=".Length));
                }
                else if (arg.StartsWith("old=", StringComparison.OrdinalIgnoreCase))
                {
                    oldAddress = ParseAddress(arg.Substring("old=".Length));
                }
                else if (arg.StartsWith("cap=", StringComparison.OrdinalIgnoreCase))
                {
                    capabilities = ParseCapabilities(arg.Substring("cap=".Length));
                }
                else
                {
                    throw new Exception($"Invalid startup argument: '{arg}'");
                }
            }

            if (comPortName == null || newAddress == null)
            {
                string title = "Dog Agility Competition Management - Device Configurer" + AssemblyReader.GetInformationalVersion();
                string exeName = Path.GetFileName(Assembly.GetEntryAssembly()!.Location);

                Console.WriteLine(title);
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine($" {exeName} port=com-port [old=old-address] new=new-address [cap=capabilities]");
                Console.WriteLine();
                Console.WriteLine("Arguments:");
                Console.WriteLine(" com-port      RS-232 (COM) port to which the mediator is connected.");
                Console.WriteLine(" old-address   Network address currently assigned to device.");
                Console.WriteLine("               When omitted, 000000 is assumed.");
                Console.WriteLine(" new-address   Network address to assign to the new device.");
                Console.WriteLine(" capabilities  Comma-separated list of capabilities to assign to the new");
                Console.WriteLine("               device. When omitted, a mediator device is assumed.");
                Console.WriteLine();
                Console.WriteLine("See the CIRCE specification for details about capabilities. Allowed values:");
                Console.WriteLine(GetAllowedCapabilities());
                Console.WriteLine();
                return null;
            }

            return new StartupArguments(comPortName, oldAddress, newAddress, capabilities);
        }

        private static WirelessNetworkAddress ParseAddress(string value)
        {
            return new(value.ToUpperInvariant());
        }

        private static DeviceCapabilities? ParseCapabilities(string value)
        {
            return (DeviceCapabilities)Enum.Parse(typeof(DeviceCapabilities), value, true);
        }

        private static string GetAllowedCapabilities()
        {
            var textBuilder = new StringBuilder();

            for (int value = 1; value < (int)DeviceCapabilities.All; value *= 2)
            {
                if (textBuilder.Length > 0)
                {
                    textBuilder.AppendLine();
                }

                var capability = (DeviceCapabilities)value;
                textBuilder.Append(" ");
                textBuilder.Append(capability);
            }

            return textBuilder.ToString();
        }
    }
}
