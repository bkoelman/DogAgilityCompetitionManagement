using System.Threading;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.UI.Controls;

/// <summary />
public sealed class NetworkSetupEventArgs : EventArgs
{
    private Task? task;

    public WirelessNetworkAddress DestinationAddress { get; }
    public bool JoinNetwork { get; }
    public DeviceRoles Roles { get; }

    public Task? Task
    {
        get => task;
        set
        {
            Guard.NotNull(value, nameof(value));
            task = value;
        }
    }

    public CancellationToken CancelToken { get; }

    public NetworkSetupEventArgs(WirelessNetworkAddress destinationAddress, bool joinNetwork, DeviceRoles roles, CancellationToken cancelToken)
    {
        Guard.NotNull(destinationAddress, nameof(destinationAddress));

        DestinationAddress = destinationAddress;
        JoinNetwork = joinNetwork;
        Roles = roles;
        CancelToken = cancelToken;
    }
}
