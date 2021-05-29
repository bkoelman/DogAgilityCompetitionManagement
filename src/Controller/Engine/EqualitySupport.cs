using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    public static class EqualitySupport
    {
        public static bool EqualsWithNulls<T>([CanBeNull] T first, [CanBeNull] T second, [NotNull] Func<T, T, bool> comparison)
        {
            if (ReferenceEquals(first, second))
            {
                // Both null or same instance.
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                // Only one of them is null.
                return false;
            }

            // Both are not null.
            return comparison(first, second);
        }
    }
}
