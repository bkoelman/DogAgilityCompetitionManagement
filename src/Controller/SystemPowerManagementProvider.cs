using System.Runtime.InteropServices;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller;

/// <summary>
/// Provides access to power management support in the operating system.
/// </summary>
public static class SystemPowerManagementProvider
{
    /// <summary>
    /// Creates an execution scope in which the system does not enter any of the sleep states on idle.
    /// </summary>
    public static IDisposable InStateDisabled
    {
        get
        {
            Disable();
            return new DisposableHolder(Restore);
        }
    }

    /// <summary>
    /// Prevents the system to enter any of the sleep states on idle.
    /// </summary>
    /// <remarks>
    /// This includes dimming and deactivation of display device. Does not affect activation of screen saver.
    /// </remarks>
    public static void Disable()
    {
        const NativeMethods.ExecutionStates flags = NativeMethods.ExecutionStates.Continuous | NativeMethods.ExecutionStates.SystemRequired |
            NativeMethods.ExecutionStates.DisplayRequired;

        NativeMethods.ExecutionStates previousState = NativeMethods.SetThreadExecutionState(flags);
        ThrowWhenFailed(previousState);
    }

    /// <summary>
    /// Resets system idle timers and enables power management to function as configured in Windows.
    /// </summary>
    public static void Restore()
    {
        NativeMethods.ExecutionStates previousState = NativeMethods.SetThreadExecutionState(NativeMethods.ExecutionStates.Continuous);
        ThrowWhenFailed(previousState);
    }

    [AssertionMethod]
    private static void ThrowWhenFailed(NativeMethods.ExecutionStates previousState)
    {
        if (previousState == 0)
        {
            throw new Exception("Failed to suppress/restore system power management.");
        }
    }

    private static class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern ExecutionStates SetThreadExecutionState(ExecutionStates flags);

        [Flags]
        public enum ExecutionStates : uint
        {
            SystemRequired = 0x00000001,
            DisplayRequired = 0x00000002,
            Continuous = 0x80000000
        }
    }
}
