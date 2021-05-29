using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

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

        private static void PlaySystemAsync([NotNull] string systemName)
        {
            const NativeMethods.SoundFlags flags = NativeMethods.SoundFlags.SndAlias | NativeMethods.SoundFlags.SndAsync |
                NativeMethods.SoundFlags.SndNodefault;

            NativeMethods.PlaySound(systemName, UIntPtr.Zero, flags);
        }

        public static void PlayWaveFile([CanBeNull] string path)
        {
            PlayFileAsync(path);
        }

        private static void PlayFileAsync([CanBeNull] string path)
        {
            const NativeMethods.SoundFlags flags = NativeMethods.SoundFlags.SndFilename | NativeMethods.SoundFlags.SndAsync |
                NativeMethods.SoundFlags.SndNodefault;

            NativeMethods.PlaySound(path, UIntPtr.Zero, flags);
        }

        private static class NativeMethods
        {
            [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PlaySound([MarshalAs(UnmanagedType.LPWStr)] [CanBeNull]
                string pszSound, UIntPtr hmod, SoundFlags fdwSound);

            [Flags]
#pragma warning disable CA2217 // Do not mark Enum with FlagsAttribute
            public enum SoundFlags
#pragma warning restore CA2217 // Do not mark Enum with FlagsAttribute
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
