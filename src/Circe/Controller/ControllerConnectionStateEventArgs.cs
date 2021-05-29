using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary />
    public sealed class ControllerConnectionStateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new connection state.
        /// </summary>
        public ControllerConnectionState State { get; }

        /// <summary>
        /// Gets the name of the COM port, if available.
        /// </summary>
        [CanBeNull]
        public string ComPort { get; }

        public ControllerConnectionStateEventArgs(ControllerConnectionState state, [CanBeNull] string comPort)
        {
            State = state;
            ComPort = comPort;
        }
    }
}
