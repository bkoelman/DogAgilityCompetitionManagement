﻿using System;
using System.Runtime.Serialization;
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

        private SerialConnectionException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
