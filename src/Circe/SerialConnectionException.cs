using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// The error that occurs while attempting to open an available COM port.
    /// </summary>
    [Serializable]
    public sealed class SerialConnectionException : Exception
    {
        public SerialConnectionException([CanBeNull] string message)
            : base(message)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private SerialConnectionException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}