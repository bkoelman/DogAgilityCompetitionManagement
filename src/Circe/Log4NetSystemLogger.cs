using JetBrains.Annotations;
using log4net;

namespace DogAgilityCompetition.Circe;

public sealed class Log4NetSystemLogger : ISystemLogger
{
    private readonly ILog log;

    public Log4NetSystemLogger(Type type)
    {
        Guard.NotNull(type, nameof(type));
        log = LogManager.GetLogger(type);
    }

    void ISystemLogger.Debug(object? message)
    {
        log.Debug(message);
    }

    [StringFormatMethod("format")]
    void ISystemLogger.Debug(object? message, Exception? exception)
    {
        log.Debug(message, exception);
    }

    void ISystemLogger.Error(object? message)
    {
        log.Error(message);
    }

    void ISystemLogger.Error(object? message, Exception? exception)
    {
        log.Error(message, exception);
    }

    void ISystemLogger.Info(object? message)
    {
        log.Info(message);
    }

    void ISystemLogger.Info(object? message, Exception? exception)
    {
        log.Info(message, exception);
    }

    void ISystemLogger.Warn(object? message)
    {
        log.Warn(message);
    }

    void ISystemLogger.Warn(object? message, Exception? exception)
    {
        log.Warn(message, exception);
    }
}
