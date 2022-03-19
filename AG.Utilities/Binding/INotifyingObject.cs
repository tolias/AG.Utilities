using System;
using System.Collections.Generic;

namespace AG.Utilities.Binding
{
    public interface INotifyingObject
    {
        event EventHandler ChildrenChanged;

        Dictionary<string, PropertyEventsPath> GetPropertiesEvents();
        void SetPropertiesEvents(Dictionary<string, PropertyEventsPath> propertiesEvents);

        void HandleWhenChanged<TProperty>(ChangedEventHandler<TProperty> propertyChangedHandler, int propertyPathPos, params string[] propertyPath);
    }
}
