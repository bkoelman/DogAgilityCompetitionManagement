using System;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary />
    public sealed class AlertEventArgs : EventArgs
    {
        public WirelessNetworkAddress DestinationAddress { get; }
        public Task? Task { get; set; }
        public CancellationToken CancelToken { get; }

        public AlertEventArgs(WirelessNetworkAddress destinationAddress, CancellationToken cancelToken)
        {
            Guard.NotNull(destinationAddress, nameof(destinationAddress));

            DestinationAddress = destinationAddress;
            CancelToken = cancelToken;
        }
    }
}
