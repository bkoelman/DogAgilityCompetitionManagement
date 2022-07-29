using System.Reflection;

namespace DogAgilityCompetition.Circe.Session;

/// <summary>
/// Enumerates the system COM ports, restarting at the first when the last port has been reached.
/// </summary>
public sealed class ComPortRotator
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private string? activePort;

    /// <summary>
    /// Gets the name of the next system COM port.
    /// </summary>
    /// <returns>
    /// The name of the next system COM port, or <c>null</c> when no ports are available.
    /// </returns>
    public string? GetNextPortName()
    {
        IList<string> systemPorts = SystemPortProvider.GetAllComPorts();

        if (systemPorts.Count == 0)
        {
            activePort = null;
        }
        else
        {
            string displayPortNames = string.Join(", ", systemPorts);
            Log.Debug($"System COM ports: {displayPortNames}");

            int index = activePort == null ? -1 : systemPorts.IndexOf(activePort);

            if (index == -1 || index == systemPorts.Count - 1)
            {
                activePort = systemPorts[0];
            }
            else
            {
                activePort = systemPorts[index + 1];
            }
        }

        return activePort;
    }
}
