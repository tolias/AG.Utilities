﻿using System;
using System.Collections.Generic;

namespace AG.Utilities.Binding
{
    public class PropertyEventsPath
    {
        //public string Name;

        //public PropertyEventsPath Child;

        /// <summary>
        /// property name / PropertyEventsPath
        /// </summary>
        public Dictionary<string, PropertyEventsPath> PropertiesEvents;

        public List<Delegate> Events;

        //public PropertyEventsPath(string name)
        //{
        //    Name = name;
        //}
    }
}
