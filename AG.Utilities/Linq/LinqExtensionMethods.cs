using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AG.Utilities.Linq
{
    public static class LinqExtensionMethods
    {
        public static bool SequenceEqual(this IEnumerable thisSequence, IEnumerable anotherSequence, bool dontThrowIfNull = false, bool threatNullAsEmpty = true)
        {
            if (thisSequence == null)
            {
                if (dontThrowIfNull)
                {
                    if (anotherSequence == null)
                        return true;
                    if (threatNullAsEmpty)
                    {
                        return !anotherSequence.GetEnumerator().MoveNext();
                    }
                    return false;
                }
                throw new ArgumentNullException(nameof(thisSequence));
            }
            if (anotherSequence == null)
            {
                if (dontThrowIfNull)
                {
                    if (threatNullAsEmpty)
                    {
                        return !thisSequence.GetEnumerator().MoveNext();
                    }
                    return false;
                }
                throw new ArgumentNullException(nameof(anotherSequence));
            }
            IEnumerator enumerator = thisSequence.GetEnumerator();
            IEnumerator enumerator2 = anotherSequence.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (!enumerator2.MoveNext() || !enumerator.Current.EqualsWithNullHandling(enumerator2.Current))
                {
                    return false;
                }
            }
            if (enumerator2.MoveNext())
            {
                return false;
            }

            return true;
        }

        public static bool ContainsSequence<T>(this IEnumerable<T> superset, IEnumerable<T> subset)
        {
            var subsetArray = subset as T[] ?? subset.ToArray();
            if (!subsetArray.Any())
                return false;
            return !subsetArray.Except(superset).Any();
        }

        public static T MaxWithSelector<T, TProperty>(this IEnumerable<T> sequence, Func<T, TProperty> selector)
            where TProperty : IComparable<TProperty>
        {
            var enumerator = sequence.GetEnumerator();
            bool canMoveNext = enumerator.MoveNext();
            if (!canMoveNext)
                return default(T);

            T maxItem = enumerator.Current;
            TProperty maxItemProperty = selector(maxItem);

            while (canMoveNext)
            {
                canMoveNext = enumerator.MoveNext();
                if (!canMoveNext)
                    break;
                var itemProperty = selector(enumerator.Current);
                if (itemProperty.CompareTo(maxItemProperty) > 0)
                {
                    maxItem = enumerator.Current;
                    maxItemProperty = itemProperty;
                }
            }

            return maxItem;
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            return source.Skip(Math.Max(0, source.Count() - count));
        }

        public static TDest[] SelectArray<TSource, TDest>(this TSource[] sourceArray, Func<TSource, TDest> selector)
        {
            var length = sourceArray.Length;
            TDest[] destArray = new TDest[length];

            for (int i = 0; i < length; i++)
            {
                destArray[i] = selector(sourceArray[i]);
            }

            return destArray;
        }

        public static List<T>[] RemoveCommonItems<T>(params IEnumerable<T>[] sequences)
        {
            var itemsUsingsCount = new Dictionary<T, HashSet<int>>();

            var sequencesCount = sequences.Length;
            var sequencesResult = new List<T>[sequencesCount];

            for (int sequenceNumber = 0; sequenceNumber < sequencesCount; sequenceNumber++)
            {
                var sequence = sequences[sequenceNumber];
                sequencesResult[sequenceNumber] = new List<T>();
                if (sequence == null)
                    continue;

                foreach (var item in sequence)
                {
                    HashSet<int> itemUsings;
                    if (!itemsUsingsCount.TryGetValue(item, out itemUsings))
                    {
                        itemUsings = new HashSet<int>();
                        itemsUsingsCount[item] = itemUsings;
                    }
                    itemUsings.Add(sequenceNumber);
                }
            }

            foreach (var pair in itemsUsingsCount)
            {
                if (pair.Value.Count == sequencesCount)
                {
                    continue;
                }
                foreach (var sequenceNumber in pair.Value)
                {
                    sequencesResult[sequenceNumber].Add(pair.Key);
                }
            }

            return sequencesResult;
        }

        public static void Move(this IList list, int oldIndex, int newIndex)
        {
            // exit if possitions are equal
            if (oldIndex == newIndex)
                return;

            var listCount = list.Count;
            if (oldIndex < 0 || oldIndex >= listCount || newIndex < 0 || newIndex >= listCount)
            {
                throw new IndexOutOfRangeException($"{nameof(oldIndex)} and {nameof(oldIndex)} must not be outside of the list range. ({nameof(oldIndex)}={oldIndex}, {nameof(newIndex)}={newIndex}, {nameof(list.Count)}={list.Count})");
            }

            object itemToMove = list[oldIndex];
            list.RemoveAt(oldIndex);
            if (oldIndex < newIndex)
            {
                newIndex--;
            }
            list.Insert(newIndex, itemToMove);
        }

        public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns>True if key existed. Otherwise - false</returns>
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            var containsKey = dict.ContainsKey(key);
            if (containsKey)
            {
                dict.Remove(key);
            }
            return containsKey;
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
    }
}
