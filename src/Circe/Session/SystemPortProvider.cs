using System.ComponentModel;
using System.IO.Ports;
using System.Reflection;
using System.Security;

namespace DogAgilityCompetition.Circe.Session;

/// <summary>
/// Provides access to the serial port interface on this system.
/// </summary>
public static class SystemPortProvider
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    /// <summary>
    /// Gets the names of all serial ports on this system.
    /// </summary>
    /// <returns>
    /// The list of serial ports.
    /// </returns>
    public static IList<string> GetAllComPorts(bool inReverseOrder = false)
    {
        Exception error;

        try
        {
            IEnumerable<string> result = SerialPort.GetPortNames().Where(IsPortNameSupported);

            if (inReverseOrder)
            {
                result = result.OrderByDescending(x => x);
            }

            return result.ToList();
        }
        catch (Win32Exception ex)
        {
            error = ex;
        }
        catch (InvalidOperationException ex)
        {
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

        Log.Warn("Failed to enumerate system COM ports.", error);
        return new List<string>();
    }

    private static bool IsPortNameSupported(string portName)
    {
        // System.IO.Ports.SerialPort only supports port names like COMxxx.
        return portName.StartsWith("com", StringComparison.OrdinalIgnoreCase);
    }
}
