using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox
{
    internal static class IEnumerableExtensions
    {
        public static void Swap<TSource>(this TSource[] array, long index1, long index2)
        {
            var tmp = array[index1];
            array[index1] = array[index2];
            array[index2] = tmp;
        }

        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            var sourceArray = source.ToArray();
            var length = sourceArray.Length;
            var rng = new Random();
            for (var i = 0; i < length; ++i)
            {
                var j = rng.Next(length);
                sourceArray.Swap(i, j);
            }

            return sourceArray;
        }

        public static TSource Max<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return source.Min(keySelector, (first, second) => -first.CompareTo(second));
        }

        public static TSource Max<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Comparer<TKey> comparer)
        {
            return source.Min(keySelector, (first, second) => -comparer.Compare(first, second));
        }

        public static TSource Min<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return source.Min(keySelector, (first, second) => first.CompareTo(second));
        }

        public static TSource Min<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Comparer<TKey> comparer)
        {
            return source.Min(keySelector, (first, second) => comparer.Compare(first, second));
        }

        private static TSource Min<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Comparison<TKey> comparison)
        {
            var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new ArgumentException("The sequence contains no elements");

            var first = enumerator.Current;
            var minKey = keySelector(first);
            var minValue = first;

            while (enumerator.MoveNext())
            {
                var currentElement = enumerator.Current;
                var currentKey = keySelector(currentElement);

                if (comparison(currentKey, minKey) < 0)
                {
                    minKey = currentKey;
                    minValue = currentElement;
                }
            }

            return minValue;
        }
    }
}