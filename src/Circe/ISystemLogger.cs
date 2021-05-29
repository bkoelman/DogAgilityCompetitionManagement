using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Defines the contract for writing diagnostic information.
    /// </summary>
    /// <remarks>
    /// Logging convention: Entering/Leaving XXX for methods, Starting/Finished XXX for activities.
    /// </remarks>
    public interface ISystemLogger
    {
        // ReSharper disable UnusedMember.Global

        void Debug([CanBeNull] object message);

        void Debug([CanBeNull] object message, [CanBeNull] Exception exception);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error([CanBeNull] object message);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error([CanBeNull] object message, [CanBeNull] Exception exception);

        void Info([CanBeNull] object message);

        void Info([CanBeNull] object message, [CanBeNull] Exception exception);

        void Warn([CanBeNull] object message);

        void Warn([CanBeNull] object message, [CanBeNull] Exception exception);

        // ReSharper restore UnusedMember.Global
    }
}
