using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Mediator
{
    /// <summary />
    public sealed class MediatorConnectionStateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new connection state.
        /// </summary>
        public MediatorConnectionState State { get; }

        /// <summary>
        /// Gets the name of the COM port, if available.
        /// </summary>
        [CanBeNull]
        public string ComPort { get; }

        public MediatorConnectionStateEventArgs(MediatorConnectionState state, [CanBeNull] string comPort)
        {
            State = state;
            ComPort = comPort;
        }
    }
}
