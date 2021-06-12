using System;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public abstract class DeviceEventArgs : EventArgs
    {
        public WirelessNetworkAddress Source { get; }

        protected DeviceEventArgs(WirelessNetworkAddress source)
        {
            Guard.NotNull(source, nameof(source));

            Source = source;
        }
    }
}
