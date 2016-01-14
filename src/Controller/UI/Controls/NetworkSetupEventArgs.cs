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
        public WirelessNetworkAddress DestinationAddress { get; private set; }

        public bool JoinNetwork { get; private set; }
        public DeviceRoles Roles { get; private set; }

        [CanBeNull]
        public Task Task
        {
            get
            {
                return task;
            }
            set
            {
                Guard.NotNull(value, nameof(value));
                task = value;
            }
        }

        public CancellationToken CancelToken { get; private set; }

        public NetworkSetupEventArgs([NotNull] WirelessNetworkAddress destinationAddress, bool joinNetwork,
            DeviceRoles roles, CancellationToken cancelToken)
        {
            Guard.NotNull(destinationAddress, nameof(destinationAddress));

            DestinationAddress = destinationAddress;
            JoinNetwork = joinNetwork;
            Roles = roles;
            CancelToken = cancelToken;
        }
    }
}