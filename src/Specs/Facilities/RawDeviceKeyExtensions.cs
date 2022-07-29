using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Specs.Facilities;

internal static class RawDeviceKeyExtensions
{
    public static RawDeviceKeys Push(this RawDeviceKeys source, RawDeviceKeys key)
    {
        return source | key;
    }

    public static RawDeviceKeys Release(this RawDeviceKeys source, RawDeviceKeys key)
    {
        return source & ~key;
    }
}
