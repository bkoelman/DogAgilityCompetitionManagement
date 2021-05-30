namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary />
    public static class BooleanExtensions
    {
        public static bool? TrueOrNull(this bool value)
        {
            return value ? true : null;
        }
    }
}
