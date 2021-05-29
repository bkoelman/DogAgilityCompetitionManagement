using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Performs a single attempt to find an available serial port on the system and returns a connection for it. A specific port can be selected by
    /// specifying "port=COMx" as command line parameter.
    /// </summary>
    public static class ComPortSelector
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        public static CirceComConnection GetConnection([CanBeNull] Action<CirceComConnection> attachHandlersCallback = null,
            [CanBeNull] Action<CirceComConnection> detachHandlersCallback = null, [CanBeNull] string specificComPort = null)
        {
            IList<string> selectablePortNames = SystemPortProvider.GetAllComPorts(true);

            string displayPortNames = string.Join(", ", selectablePortNames);
            Log.Debug($"System COM ports: {displayPortNames}");

            if (!selectablePortNames.Any())
            {
                throw new SerialConnectionException("This system contains no serial ports.");
            }

            string forcePortTo = specificComPort ?? TryGetPortFromStartupArguments();

            if (forcePortTo != null)
            {
                if (selectablePortNames.Contains(forcePortTo, StringComparer.OrdinalIgnoreCase))
                {
                    CirceComConnection specificConnection = TryCreateOpenedConnection(forcePortTo, attachHandlersCallback, detachHandlersCallback);

                    if (specificConnection != null)
                    {
                        return specificConnection;
                    }

                    throw new SerialConnectionException($"Failed to open serial port {forcePortTo}.");
                }

                throw new SerialConnectionException($"Serial port {forcePortTo} is not available on this system.");
            }

            foreach (string portName in selectablePortNames)
            {
                CirceComConnection nextConnection = TryCreateOpenedConnection(portName, attachHandlersCallback, detachHandlersCallback);

                if (nextConnection != null)
                {
                    return nextConnection;
                }
            }

            throw new SerialConnectionException($"None of the available serial ports ({displayPortNames}) could be opened.");
        }

        [CanBeNull]
        private static CirceComConnection TryCreateOpenedConnection([NotNull] string portName, [CanBeNull] Action<CirceComConnection> attachHandlersCallback,
            [CanBeNull] Action<CirceComConnection> detachHandlersCallback)
        {
            CirceComConnection connection = null;
            Exception error;

            try
            {
                connection = new CirceComConnection(portName);

                attachHandlersCallback?.Invoke(connection);

                connection.Open();
                return connection;
            }
            catch (InvalidOperationException ex)
            {
                // This may happen when you unplug the serial port cable while writing to the port.
                // The exception indicates that the port has been closed, so writing fails.
                error = ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                error = ex;
            }
            catch (SecurityException ex)
            {
                error = ex;
            }
            catch (IOException ex)
            {
                error = ex;
            }

            Log.Debug($"Failed to open port {portName}: {error.GetType()}: {error.Message}");

            if (connection != null)
            {
                detachHandlersCallback?.Invoke(connection);

                connection.Dispose();
            }

            return null;
        }

        [CanBeNull]
        private static string TryGetPortFromStartupArguments()
        {
            const string search = "port=";

            IEnumerable<string> query =
                from argument in Environment.GetCommandLineArgs()
                where argument.StartsWith(search, StringComparison.OrdinalIgnoreCase)
                select argument.Substring(search.Length).ToUpperInvariant();

            return query.FirstOrDefault(value => value.Length > 0);
        }
    }
}
