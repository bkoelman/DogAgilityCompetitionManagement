using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Storage.FileFormats
{
    /// <summary>
    /// The exception that is thrown when unable to parse data from a delimited file.
    /// </summary>
    [Serializable]
    public sealed class DelimitedValuesParseException : Exception
    {
        public DelimitedValuesParseException([CanBeNull] string message)
            : base(message)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private DelimitedValuesParseException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}