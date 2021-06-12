using System;
using System.Collections.Generic;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary>
    /// Captures log messages and exposes them for inspection in unit tests.
    /// </summary>
    public sealed class FakeSystemLogger : ISystemLogger
    {
        private readonly List<string> debugMessages = new();
        private readonly List<string> errorMessages = new();
        private readonly List<string> infoMessages = new();
        private readonly List<string> warningMessages = new();

        public IReadOnlyList<string> DebugMessages => debugMessages.AsReadOnly();
        public IReadOnlyList<string> ErrorMessages => errorMessages.AsReadOnly();
        public IReadOnlyList<string> InfoMessages => infoMessages.AsReadOnly();
        public IReadOnlyList<string> WarningMessages => warningMessages.AsReadOnly();

        void ISystemLogger.Debug(object? message)
        {
            debugMessages.Add(FormatMessage(message));
        }

        [StringFormatMethod("format")]
        void ISystemLogger.Debug(object? message, Exception? exception)
        {
            debugMessages.Add(FormatMessage(message, exception));
        }

        void ISystemLogger.Error(object? message)
        {
            errorMessages.Add(FormatMessage(message));
        }

        void ISystemLogger.Error(object? message, Exception? exception)
        {
            errorMessages.Add(FormatMessage(message, exception));
        }

        void ISystemLogger.Info(object? message)
        {
            infoMessages.Add(FormatMessage(message));
        }

        void ISystemLogger.Info(object? message, Exception? exception)
        {
            infoMessages.Add(FormatMessage(message, exception));
        }

        void ISystemLogger.Warn(object? message)
        {
            warningMessages.Add(FormatMessage(message));
        }

        void ISystemLogger.Warn(object? message, Exception? exception)
        {
            warningMessages.Add(FormatMessage(message, exception));
        }

        private static string FormatMessage(object? message)
        {
            return message?.ToString() ?? string.Empty;
        }

        private static string FormatMessage(object? message, Exception? exception)
        {
            return message?.ToString() + exception;
        }
    }
}
