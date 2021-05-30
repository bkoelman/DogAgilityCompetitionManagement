using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
            where T : class?
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static Collection<T> ToCollection<T>(this IEnumerable<T>? source)
            where T : class?
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
