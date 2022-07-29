using System.Globalization;
using DogAgilityCompetition.Circe;
using Microsoft.Win32;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage;

/// <summary>
/// Provides access to convenience settings stored in the Windows Registry.
/// </summary>
public static class RegistrySettingsProvider
{
    private const string RegistryPath = @"Software\DogAgilityCompetitionManagement\MediatorEmulator";
    private const string MruRegistryPath = "MruList";
    private const int MruMaxLength = 6;
    private const string LastUsedAddressPath = "LastAddressUsed";

    public static MostRecentlyUsedContainer GetMruList()
    {
        return IgnoreErrors(() =>
        {
            using RegistryKey? appSettingsKey = Registry.CurrentUser.OpenSubKey(RegistryPath);
            var container = new MostRecentlyUsedContainer();

            if (appSettingsKey != null)
            {
                ImportContainerFromKey(appSettingsKey, container);
            }

            return container;
        }, new MostRecentlyUsedContainer());
    }

    private static void ImportContainerFromKey(RegistryKey appSettingsKey, MostRecentlyUsedContainer container)
    {
        if (appSettingsKey.GetValue(MruRegistryPath) is string valueList)
        {
            string[] values = valueList.Split(new[]
            {
                ';'
            }, StringSplitOptions.RemoveEmptyEntries);

            container.Import(values.Take(MruMaxLength));
        }
    }

    public static void SaveMruList(MostRecentlyUsedContainer container)
    {
        Guard.NotNull(container, nameof(container));

        IgnoreErrors(() =>
        {
            using RegistryKey appSettingsKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            ExportContainerToKey(container, appSettingsKey);
        });
    }

    private static void ExportContainerToKey(MostRecentlyUsedContainer container, RegistryKey appSettingsKey)
    {
        string valueList = string.Join(";", container.Items.Take(MruMaxLength));
        appSettingsKey.SetValue(MruRegistryPath, valueList);
    }

    public static int GetLastUsedAddress()
    {
        return IgnoreErrors(() =>
        {
            using RegistryKey? appSettingsKey = Registry.CurrentUser.OpenSubKey(RegistryPath);
            return appSettingsKey != null ? ParseAddressFromKey(appSettingsKey) : 0;
        }, 0);
    }

    private static int ParseAddressFromKey(RegistryKey appSettingsKey)
    {
        object? value = appSettingsKey.GetValue(LastUsedAddressPath);
        return (int?)value ?? 0;
    }

    public static void SaveLastUsedAddress(int address)
    {
        IgnoreErrors(() =>
        {
            using RegistryKey appSettingsKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            StoreAddressInKey(address, appSettingsKey);
        });
    }

    private static void StoreAddressInKey(int address, RegistryKey appSettingsKey)
    {
        string value = address.ToString(CultureInfo.InvariantCulture);
        appSettingsKey.SetValue(LastUsedAddressPath, value, RegistryValueKind.DWord);
    }

    private static void IgnoreErrors(Action action)
    {
        object tempInstance = new();

        Func<object> callback = () =>
        {
            action();
            return tempInstance;
        };

        IgnoreErrors(callback, tempInstance);
    }

    private static T IgnoreErrors<T>(Func<T> callback, T defaultValue)
    {
        try
        {
            return callback();
        }
        // ReSharper disable once EmptyGeneralCatchClause
        // Justification: This is for convenience only; failure must not require user attention.
        catch (Exception)
        {
            return defaultValue;
        }
    }
}
