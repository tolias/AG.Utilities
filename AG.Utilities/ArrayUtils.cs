using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AG.Utilities
{
    public static class ArrayUtils
    {
        public static T[] MergeArrays<T>(T[] arr1, T[] arr2)
        {
            var arr1Length = arr1.LongLength;
            var arr2Length = arr2.LongLength;
            var mergedArray = new T[arr1Length + arr2Length];

            Array.Copy(arr1, mergedArray, arr1Length);
            Array.Copy(arr2, 0, mergedArray, arr1Length, arr2Length);
            return mergedArray;
        }
    }
}
