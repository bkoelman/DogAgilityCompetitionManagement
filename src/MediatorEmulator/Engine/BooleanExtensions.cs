using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary />
    public static class BooleanExtensions
    {
        [CanBeNull]
        public static bool? TrueOrNull(this bool value)
        {
            return value ? true : (bool?) null;
        }
    }
}