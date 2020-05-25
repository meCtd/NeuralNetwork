using System;
using System.Collections.Generic;

namespace NNCore
{
    static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in items)
            {
                action(item, i);
                i++;
            }
        }
    }
}
