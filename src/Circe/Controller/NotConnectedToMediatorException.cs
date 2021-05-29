using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Controller
{
    /// <summary>
    /// Represents the error that is thrown when attempting to send an operation to a mediator while no connection is available.
    /// </summary>
    [Serializable]
    public sealed class NotConnectedToMediatorException : Exception
    {
        public NotConnectedToMediatorException()
        {
        }

        private NotConnectedToMediatorException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
