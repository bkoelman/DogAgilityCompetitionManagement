using System;
using System.Diagnostics.CodeAnalysis;

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

        void Debug(object? message);

        void Debug(object? message, Exception? exception);

        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
        void Error(object? message);

        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
        void Error(object? message, Exception? exception);

        void Info(object? message);

        void Info(object? message, Exception? exception);

        void Warn(object? message);

        void Warn(object? message, Exception? exception);

        // ReSharper restore UnusedMember.Global
    }
}
