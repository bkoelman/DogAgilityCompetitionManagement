using System.Collections.Generic;
using System.Globalization;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Contains some predefined values for <see cref="KeepAliveOperation.MediatorStatus" />.
    /// </summary>
    public static class KnownMediatorStatusCode
    {
        public const int Normal = 0;
        public const int MediatorUnconfigured = 1;
        public const int FailedToSendNetworkPacket = 2;

        [NotNull]
        public static string GetNameFor(int mediatorStatusCode)
        {
            switch (mediatorStatusCode)
            {
                case Normal:
                    return "0 (Normal)";
                case MediatorUnconfigured:
                    return "1 (Mediator Unconfigured)";
                case FailedToSendNetworkPacket:
                    return "2 (Failed to send network packet)";
                default:
                    return mediatorStatusCode.ToString(CultureInfo.InvariantCulture);
            }
        }

        [NotNull]
        public static IEnumerable<int> GetAll()
        {
            return new[] { Normal, MediatorUnconfigured, FailedToSendNetworkPacket };
        }
    }
}