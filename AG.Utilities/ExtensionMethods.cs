using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace AG.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type genericType)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (genericType == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static TValue GetValueOrAddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueGetter)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = valueGetter();
                dictionary.Add(key, value);
            }
            return value;
        }

        public static List<T> Merge<T>(this List<T> mergingList, List<T> listToMerge)
        {
            if (mergingList == null)
            {
                return listToMerge;
            }
            else if (listToMerge == null)
            {
                return mergingList;
            }
            else
            {
                mergingList.AddRange(listToMerge);
                return mergingList;
            }
        }

        public static string GetBodyMemberName<T>(this Expression<Func<T>> expressionWithPropertyName)
        {
            MemberExpression memberExpression = (MemberExpression)expressionWithPropertyName.Body;
            return memberExpression.Member.Name;
        }

        //public static void AddDictionary<TKey, TItem>(this Dictionary<TKey, IEnumerable<TItem>> dictionary, Dictionary<TKey, IEnumerable<TItem>> dictionaryToAdd)
        //{
        //    TValue value;
        //    if (!dictionary.TryGetValue(key, out value))
        //    {
        //        value = valueGetter();
        //        dictionary.Add(key, value);
        //    }
        //    return value;
        //}

        public static bool EqualsWithNullHandling<T>(this T thisObject, T anotherObject)
        {
            if (thisObject == null)
            {
                return anotherObject == null;
            }
            return thisObject.Equals(anotherObject);
        }

        //public static TDest Cast<TSource, TDest>(this TSource source)
        //    where TDest : class
        //{
        //    if (source == null)
        //        return null;
        //    var casted = source as TDest;
        //    if (casted == null)
        //        throw new InvalidCastException(string.Format("Can't cast \"{0}\" to \"{1}\"", typeof(TSource).FullName, typeof(TDest).FullName));
        //    return casted;
        //}

        public static TDest CastObject<TDest>(this object source)
            where TDest : class
        {
            if (source == null)
                return null;
            var casted = source as TDest;
            if (casted == null)
                throw new InvalidCastException(string.Format("Can't cast \"{0}\" to \"{1}\"", source.GetType().FullName, typeof(TDest).FullName));
            return casted;
        }

        public static TReturn IfNotNull<TObj, TReturn>(this TObj obj, Func<TObj, TReturn> selector)
            where TObj : class
        {
            if (obj == null)
                return default(TReturn);
            return selector(obj);
        }

        public static void IfNotNull<TObj>(this TObj obj, Action<TObj> selector)
            where TObj : class
        {
            if (obj == null)
                return;
            selector(obj);
        }

        public static string ReplaceCharsWith(this string s, string replaceWith, params char[] charsToReplace)
        {
            StringBuilder sb = new StringBuilder(s);

            foreach (var c in charsToReplace)
            {
                sb.Replace(c.ToString(), replaceWith);
            }

            return sb.ToString();
        }

        public static string JoinAsString<T>(this IEnumerable<T> objects, string separator)
        {
            return string.Join(separator, objects.Select(s => s.ToString()).ToArray());
        }

        public static string JoinAsStringWithComma<T>(this IEnumerable<T> objects)
        {
            return objects.JoinAsString(",");
        }

        public static void Remove<T>(this List<T> list, IEnumerable<T> itemsToRemove)
        {
            foreach (var itemToRemove in itemsToRemove)
            {
                list.Remove(itemToRemove);
            }
        }

        public static void Remove<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<TKey> itemsToRemove)
        {
            foreach (var itemToRemove in itemsToRemove)
            {
                dict.Remove(itemToRemove);
            }
        }

        public static string ToLowerFirstLetterIfItsUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var firstLetter = s[0];

            if (char.IsLower(firstLetter))
                return s;

            var words = s.Split(' ');

            var firstWord = words[0];

            var firstWordLength = s.Length;

            if (firstWordLength == 1)
            {
                return s.ToLower();
            }

            if (!firstWord.Skip(1).All(c => char.IsLetter(c) ? char.IsLower(c) : true))
            {
                //Don't lower case the 1st letter if all letters except 1st in the 1st word are upper case
                return s;
            }

            return char.ToLower(s[0]) + s.Substring(1);
        }

        public static bool EqualsNullOrEmpty(this string s, string anotherString)
        {
            if (string.IsNullOrEmpty(s))
                return string.IsNullOrEmpty(anotherString);
            return s == anotherString;
        }

        public static string Replace(this string s, string occurenceRegexPattern, Func<Match, string> occurenceReplaceAction)
        {
            if (s == null)
                return s;

            var occurenceRegex = new Regex(occurenceRegexPattern);

            var matches = occurenceRegex.Matches(s);

            if (matches.Count == 0)
                return s;

            var sbResult = new StringBuilder();

            int lastMatchEnd = 0;

            foreach (Match match in matches)
            {
                var curMatchStart = match.Index;
                if (lastMatchEnd != curMatchStart)
                {
                    var substringBetweenMatchesLength = curMatchStart - lastMatchEnd;
                    var substringBetweenMatches = s.Substring(lastMatchEnd, substringBetweenMatchesLength);
                    sbResult.Append(substringBetweenMatches);
                }

                var replacedOccurence = occurenceReplaceAction(match);
                sbResult.Append(replacedOccurence);

                lastMatchEnd = match.Index + match.Length;
            }

            if (lastMatchEnd < s.Length)
            {
                sbResult.Append(s.Substring(lastMatchEnd));
            }

            return sbResult.ToString();
        }
    }
}
