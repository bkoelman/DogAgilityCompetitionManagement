using System;
using System.Globalization;
using System.Linq;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage
{
    /// <summary>
    /// Provides access to convenience settings stored in the Windows Registry.
    /// </summary>
    public static class RegistrySettingsProvider
    {
        private const string RegistryPath = @"Software\DogAgilityCompetitionManagement\MediatorEmulator";
        private const string MruRegistryPath = "MruList";
        private const int MruMaxLength = 6;
        private const string LastUsedAddressPath = "LastAddressUsed";

        [NotNull]
        public static MostRecentlyUsedContainer GetMruList()
        {
            return IgnoreErrors(() =>
            {
                using (RegistryKey appSettingsKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    var container = new MostRecentlyUsedContainer();

                    if (appSettingsKey != null)
                    {
                        ImportContainerFromKey(appSettingsKey, container);
                    }

                    return container;
                }
            }, new MostRecentlyUsedContainer());
        }

        private static void ImportContainerFromKey([NotNull] RegistryKey appSettingsKey, [NotNull] MostRecentlyUsedContainer container)
        {
            string valueList = appSettingsKey.GetValue(MruRegistryPath) as string;

            if (valueList != null)
            {
                string[] values = valueList.Split(new[]
                {
                    ';'
                }, StringSplitOptions.RemoveEmptyEntries);

                container.Import(values.Take(MruMaxLength));
            }
        }

        public static void SaveMruList([NotNull] MostRecentlyUsedContainer container)
        {
            Guard.NotNull(container, nameof(container));

            IgnoreErrors(() =>
            {
                using (RegistryKey appSettingsKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    if (appSettingsKey != null)
                    {
                        ExportContainerToKey(container, appSettingsKey);
                    }
                }
            });
        }

        private static void ExportContainerToKey([NotNull] MostRecentlyUsedContainer container, [NotNull] RegistryKey appSettingsKey)
        {
            string valueList = string.Join(";", container.Items.Take(MruMaxLength));
            appSettingsKey.SetValue(MruRegistryPath, valueList);
        }

        public static int GetLastUsedAddress()
        {
            return IgnoreErrors(() =>
            {
                using (RegistryKey appSettingsKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    return appSettingsKey != null ? ParseAddressFromKey(appSettingsKey) : 0;
                }
            }, 0);
        }

        private static int ParseAddressFromKey([NotNull] RegistryKey appSettingsKey)
        {
            object value = appSettingsKey.GetValue(LastUsedAddressPath);
            return (int?)value ?? 0;
        }

        public static void SaveLastUsedAddress(int address)
        {
            IgnoreErrors(() =>
            {
                using (RegistryKey appSettingsKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    if (appSettingsKey != null)
                    {
                        StoreAddressInKey(address, appSettingsKey);
                    }
                }
            });
        }

        private static void StoreAddressInKey(int address, [NotNull] RegistryKey appSettingsKey)
        {
            string value = address.ToString(CultureInfo.InvariantCulture);
            appSettingsKey.SetValue(LastUsedAddressPath, value, RegistryValueKind.DWord);
        }

        private static void IgnoreErrors([NotNull] Action action)
        {
            object tempInstance = new();

            Func<object> callback = () =>
            {
                action();
                return tempInstance;
            };

            IgnoreErrors(callback, tempInstance);
        }

        [NotNull]
        private static T IgnoreErrors<T>([NotNull] Func<T> callback, [NotNull] T defaultValue)
        {
            try
            {
                return callback();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            // Reason: This is for convenience only; failure must not require user attention.
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
