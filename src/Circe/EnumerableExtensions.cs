using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public static class EnumerableExtensions
    {
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<T> EmptyIfNull<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        [NotNull]
        [ItemCanBeNull]
        public static Collection<T> ToCollection<T>([CanBeNull] [ItemCanBeNull] this IEnumerable<T> source)
        {
            var result = new Collection<T>();

            if (source != null)
            {
                foreach (T item in source)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}