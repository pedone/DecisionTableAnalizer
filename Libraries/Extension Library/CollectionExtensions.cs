using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ExtensionLibrary
{
    public static class CollectionExtensions
    {

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static Value GetValue<Key, Value>(this IDictionary<Key, Value> source, Key key)
        {
            Value result;
            if (source.TryGetValue(key, out result))
                return result;

            return default(Value);
        }

        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
        {
            List<TSource> sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }

        public static bool SequenceEquivalent<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            if (items == null)
                return false;

            List<T> itemsList = source.ToList();
            List<T> otherItemsList = items.ToList();
            if (itemsList.Count != otherItemsList.Count)
                return false;

            foreach (var item in source)
            {
                if (!items.Any(cur => item.Equals(cur)))
                    return false;
            }

            return true;
        }

    }
}
