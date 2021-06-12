using System;
using System.Runtime.InteropServices;

namespace DogAgilityCompetition.Controller
{
    /// <summary>
    /// Enables playing a sound file.
    /// </summary>
    public static class SystemSound
    {
        public static void AsyncPlayDeviceConnect()
        {
            PlaySystemAsync(SystemEventLabels.DeviceConnect);
        }

        public static void AsyncPlayDeviceDisconnect()
        {
            PlaySystemAsync(SystemEventLabels.DeviceDisconnect);
        }

        private static void PlaySystemAsync(string systemName)
        {
            const NativeMethods.SoundFlags flags = NativeMethods.SoundFlags.SndAlias | NativeMethods.SoundFlags.SndAsync |
                NativeMethods.SoundFlags.SndNodefault;

            NativeMethods.PlaySound(systemName, UIntPtr.Zero, flags);
        }

        public static void PlayWaveFile(string? path)
        {
            PlayFileAsync(path);
        }

        private static void PlayFileAsync(string? path)
        {
            const NativeMethods.SoundFlags flags = NativeMethods.SoundFlags.SndFilename | NativeMethods.SoundFlags.SndAsync |
                NativeMethods.SoundFlags.SndNodefault;

            NativeMethods.PlaySound(path, UIntPtr.Zero, flags);
        }

        private static class NativeMethods
        {
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PlaySound([MarshalAs(UnmanagedType.LPWStr)] string? pszSound, UIntPtr hmod, SoundFlags fdwSound);

            [Flags]
            public enum SoundFlags
            {
                /// <summary>
                /// Play asynchronously.
                /// </summary>
                SndAsync = 0x0001,

                /// <summary>
                /// Silence (!default) if sound not found.
                /// </summary>
                SndNodefault = 0x0002,

                /// <summary>
                /// Parameter 'name' is a registry alias.
                /// </summary>
                SndAlias = 0x00010000,

                /// <summary>
                /// Parameter 'name' is a file name.
                /// </summary>
                SndFilename = 0x00020000
            }
        }

        private static class SystemEventLabels
        {
            // See HKEY_CURRENT_USER\AppEvents\EventLabels for a list of system sound names.

            public const string DeviceConnect = "DeviceConnect";
            public const string DeviceDisconnect = "DeviceDisconnect";
        }
    }
}
