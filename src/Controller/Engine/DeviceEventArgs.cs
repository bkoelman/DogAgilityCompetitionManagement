using System;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary />
    public abstract class DeviceEventArgs : EventArgs
    {
        [NotNull]
        public WirelessNetworkAddress Source { get; }

        protected DeviceEventArgs([NotNull] WirelessNetworkAddress source)
        {
            Guard.NotNull(source, nameof(source));

            Source = source;
        }
    }
}
