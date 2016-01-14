using System;
using System.Runtime.InteropServices;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller
{
    /// <summary>
    /// Provides access to power management support in the operating system.
    /// </summary>
    public static class SystemPowerManagementProvider
    {
        /// <summary>
        /// Creates an execution scope in which the system does not enter any of the sleep states on idle.
        /// </summary>
        [NotNull]
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
            const NativeMethods.ExecutionState flags =
                NativeMethods.ExecutionState.Continuous | NativeMethods.ExecutionState.SystemRequired |
                    NativeMethods.ExecutionState.DisplayRequired;
            NativeMethods.ExecutionState previousState = NativeMethods.SetThreadExecutionState(flags);
            ThrowWhenFailed(previousState);
        }

        /// <summary>
        /// Resets system idle timers and enables power management to function as configured in Windows.
        /// </summary>
        public static void Restore()
        {
            NativeMethods.ExecutionState previousState =
                NativeMethods.SetThreadExecutionState(NativeMethods.ExecutionState.Continuous);
            ThrowWhenFailed(previousState);
        }

        [AssertionMethod]
        private static void ThrowWhenFailed(NativeMethods.ExecutionState previousState)
        {
            if (previousState == 0)
            {
                throw new Exception("Failed to suppress/restore system power management.");
            }
        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern ExecutionState SetThreadExecutionState(ExecutionState flags);

            [Flags]
#pragma warning disable CA2217 // Do not mark Enum with FlagsAttribute
            public enum ExecutionState : uint
#pragma warning restore CA2217 // Do not mark Enum with FlagsAttribute
            {
                SystemRequired = 0x00000001,
                DisplayRequired = 0x00000002,
                Continuous = 0x80000000
            }
        }
    }
}