﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AG.Utilities
{
    public static class ReflectionHelpers
    {

        /// <summary>
        /// Gets a all Type instances matching the specified class name with just non-namespace qualified class name.
        /// </summary>
        /// <param name="className">Name of the class sought.</param>
        /// <returns>Types that have the class name specified. They may not be in the same namespace.</returns>
        public static Type[] GetTypesByName(string className)
        {
            List<Type> returnVal = new List<Type>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].Name == className)
                    {
                        returnVal.Add(assemblyTypes[j]);
                    }
                }
            }

            return returnVal.ToArray();
        }
    }
}
