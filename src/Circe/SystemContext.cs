using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Provides overriding of system behavior for testing purposes.
    /// </summary>
    public static class SystemContext
    {
        [NotNull]
        public static Func<DateTime> Now { get; set; }

        [NotNull]
        public static Func<DateTime> UtcNow { get; set; }

        static SystemContext()
        {
            Now = () => DateTime.Now;
            UtcNow = () => DateTime.UtcNow;
        }
    }
}
