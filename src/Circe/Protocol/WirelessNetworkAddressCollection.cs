using System.Collections;
using DogAgilityCompetition.Circe.Protocol.Parameters;

namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// A collection of <see cref="WirelessNetworkAddress" /> objects that is tied to an operation.
/// </summary>
internal sealed class WirelessNetworkAddressCollection : ICollection<WirelessNetworkAddress>
{
    private readonly Operation owner;
    private readonly ParameterType.NetworkAddress parameterType;
    private readonly int parameterId;

    private IEnumerable<NetworkAddressParameter> OwnerAddressParameters
    {
        get
        {
            return owner.Parameters.Where(parameter => parameter.Id == parameterId).Cast<NetworkAddressParameter>();
        }
    }

    public int Count => OwnerAddressParameters.Count();

    public bool IsReadOnly => false;

    public WirelessNetworkAddressCollection(Operation owner, ParameterType.NetworkAddress parameterType)
    {
        Guard.NotNull(owner, nameof(owner));

        this.owner = owner;
        this.parameterType = parameterType;

        NetworkAddressParameter prototypeParameter = ParameterFactory.Create(parameterType, true);
        parameterId = prototypeParameter.Id;
    }

    public IEnumerator<WirelessNetworkAddress> GetEnumerator()
    {
        IEnumerable<WirelessNetworkAddress> wirelessNetworkAddresses = ToWirelessNetworkAddresses();
        return wirelessNetworkAddresses.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<WirelessNetworkAddress> ToWirelessNetworkAddresses()
    {
        // Justification for nullable suppression: The parameters this class wraps are always required parameters.
        return OwnerAddressParameters.Select(addressParameter => new WirelessNetworkAddress(addressParameter.Value!));
    }

    public void Add(WirelessNetworkAddress item)
    {
        Guard.NotNull(item, nameof(item));

        NetworkAddressParameter parameter = CreateAttachParameter();
        parameter.Value = item.Value;
    }

    internal NetworkAddressParameter CreateAttachParameter()
    {
        NetworkAddressParameter parameter = ParameterFactory.Create(parameterType, true);
        owner.Parameters.Add(parameter);
        return parameter;
    }

    public void Clear()
    {
        List<NetworkAddressParameter> parametersToRemove = OwnerAddressParameters.ToList();

        foreach (NetworkAddressParameter parameterToRemove in parametersToRemove)
        {
            owner.Parameters.Remove(parameterToRemove);
        }
    }

    public bool Contains(WirelessNetworkAddress item)
    {
        Guard.NotNull(item, nameof(item));

        return OwnerAddressParameters.Any(addressParameter => addressParameter.Value == item.Value);
    }

    public void CopyTo(WirelessNetworkAddress[] array, int arrayIndex)
    {
        WirelessNetworkAddress[] contents = ToWirelessNetworkAddresses().ToArray();
        Array.Copy(contents, 0, array, arrayIndex, contents.Length);
    }

    public bool Remove(WirelessNetworkAddress item)
    {
        Guard.NotNull(item, nameof(item));

        NetworkAddressParameter? parameterToRemove = OwnerAddressParameters.FirstOrDefault(addressParameter => addressParameter.Value == item.Value);

        if (parameterToRemove != null)
        {
            owner.Parameters.Remove(parameterToRemove);
            return true;
        }

        return false;
    }
}
