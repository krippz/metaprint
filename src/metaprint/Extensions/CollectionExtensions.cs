using System.Collections.Generic;
using System.Linq;

namespace metaprint.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || !items.Any();
        }
    }
}