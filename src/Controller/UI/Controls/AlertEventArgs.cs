using System;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary />
    public sealed class AlertEventArgs : EventArgs
    {
        [NotNull]
        public WirelessNetworkAddress DestinationAddress { get; }

        [CanBeNull]
        public Task Task { get; set; }

        public CancellationToken CancelToken { get; }

        public AlertEventArgs([NotNull] WirelessNetworkAddress destinationAddress, CancellationToken cancelToken)
        {
            Guard.NotNull(destinationAddress, nameof(destinationAddress));

            DestinationAddress = destinationAddress;
            CancelToken = cancelToken;
        }
    }
}
