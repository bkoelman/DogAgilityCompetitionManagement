using System;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary />
    public sealed class NetworkSetupEventArgs : EventArgs
    {
        [CanBeNull]
        private Task task;

        [NotNull]
        public WirelessNetworkAddress DestinationAddress { get; }

        public bool JoinNetwork { get; }
        public DeviceRoles Roles { get; }

        [CanBeNull]
        public Task Task
        {
            get => task;
            set
            {
                Guard.NotNull(value, nameof(value));
                task = value;
            }
        }

        public CancellationToken CancelToken { get; }

        public NetworkSetupEventArgs([NotNull] WirelessNetworkAddress destinationAddress, bool joinNetwork, DeviceRoles roles, CancellationToken cancelToken)
        {
            Guard.NotNull(destinationAddress, nameof(destinationAddress));

            DestinationAddress = destinationAddress;
            JoinNetwork = joinNetwork;
            Roles = roles;
            CancelToken = cancelToken;
        }
    }
}
