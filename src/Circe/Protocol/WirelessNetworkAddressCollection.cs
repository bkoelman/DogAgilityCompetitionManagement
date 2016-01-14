using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// A collection of <see cref="WirelessNetworkAddress" /> objects that is tied to an operation.
    /// </summary>
    [Serializable]
    internal sealed class WirelessNetworkAddressCollection : ICollection<WirelessNetworkAddress>
    {
        [NotNull]
        private readonly Operation owner;

        private readonly ParameterType.NetworkAddress parameterType;
        private readonly int parameterId;

        public int Count => OwnerAddressParameters.Count();

        public bool IsReadOnly => false;

        [NotNull]
        [ItemNotNull]
        private IEnumerable<NetworkAddressParameter> OwnerAddressParameters
        {
            get
            {
                return owner.Parameters.Where(parameter => parameter.Id == parameterId).Cast<NetworkAddressParameter>();
            }
        }

        public WirelessNetworkAddressCollection([NotNull] Operation owner, ParameterType.NetworkAddress parameterType)
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

        [NotNull]
        [ItemNotNull]
        private IEnumerable<WirelessNetworkAddress> ToWirelessNetworkAddresses()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // Reason: The parameters this class wraps are always required parameters.
            return OwnerAddressParameters.Select(addressParameter => new WirelessNetworkAddress(addressParameter.Value));
        }

        public void Add([NotNull] WirelessNetworkAddress item)
        {
            Guard.NotNull(item, nameof(item));

            NetworkAddressParameter parameter = CreateAttachParameter();
            parameter.Value = item.Value;
        }

        [NotNull]
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

        public void CopyTo([ItemNotNull] WirelessNetworkAddress[] array, int arrayIndex)
        {
            WirelessNetworkAddress[] contents = ToWirelessNetworkAddresses().ToArray();
            Array.Copy(contents, 0, array, arrayIndex, contents.Length);
        }

        public bool Remove(WirelessNetworkAddress item)
        {
            Guard.NotNull(item, nameof(item));

            NetworkAddressParameter parameterToRemove =
                OwnerAddressParameters.FirstOrDefault(addressParameter => addressParameter.Value == item.Value);
            if (parameterToRemove != null)
            {
                owner.Parameters.Remove(parameterToRemove);
                return true;
            }
            return false;
        }
    }
}