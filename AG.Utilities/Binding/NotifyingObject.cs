using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using AG.Utilities;
using AG.Utilities.Binding;

namespace AG.Utilities.Binding
{
    public class NotifyingObject<TNotifyingObject> : INotifyingObject
        where TNotifyingObject : NotifyingObject<TNotifyingObject>
    {
        private Dictionary<string, PropertyEventsPath> _propertiesEvents;

        public event EventHandler ChildrenChanged;

        protected void OnChildrenChanged(object sender, EventArgs e)
        {
            var handler = ChildrenChanged;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        public Dictionary<string, PropertyEventsPath> GetPropertiesEvents() { return _propertiesEvents; }

        public void SetPropertiesEvents(Dictionary<string, PropertyEventsPath> propertiesEvents)
        {
            _propertiesEvents = propertiesEvents;
        }

        public void HandleWhenChanged<TProperty>(Expression<Func<TNotifyingObject, TProperty>> propertyPathExpression,
            ChangedEventHandler<TProperty> propertyChangedHandler)
        {
            List<string> propertyPathList = new List<string>();
            MemberExpression memberExpression = (MemberExpression)propertyPathExpression.Body;
            int count = 0;
            while (memberExpression != null)
            {
                propertyPathList.Add(memberExpression.Member.Name);
                count++;
                memberExpression = memberExpression.Expression as MemberExpression;
            }
            string[] propertyPath = new string[count];
            for (int i = 0, j = count - 1; i < count; i++, j--)
            {
                propertyPath[i] = propertyPathList[j];
            }
            HandleWhenChanged(propertyChangedHandler, 0, propertyPath);
        }

        public void HandleWhenChanged<TProperty>(ChangedEventHandler<TProperty> propertyChangedHandler, params string[] propertyPath)
        {
            HandleWhenChanged(propertyChangedHandler, 0, propertyPath);
        }

        public void HandleWhenChanged<TProperty>(ChangedEventHandler<TProperty> propertyChangedHandler, int propertyPathPos, params string[] propertyPath)
        {
            int pathCount = propertyPath.Length - propertyPathPos;
            string currentpathPropertyName = propertyPath[propertyPathPos];
            if (pathCount > 1)
            {
                var propertyInfo = this.GetType().GetProperty(currentpathPropertyName);
                var propertyType = propertyInfo.PropertyType;
                Type iNotifyingObjectType = typeof(INotifyingObject);
                //var notifyingObjectType = typeof(NotifyingObject<>);
                if (iNotifyingObjectType.IsAssignableFrom(propertyType))
                {
                    INotifyingObject childNotifyingObject = (INotifyingObject)propertyInfo.GetValue(this, null);
                    if (childNotifyingObject != null)
                    {
                        childNotifyingObject.HandleWhenChanged(propertyChangedHandler, propertyPathPos + 1, propertyPath);
                    }
                    else
                    {
                        PropertyEventsPath currentPropertyEventsPath;
                        if (_propertiesEvents == null)
                        {
                            _propertiesEvents = new Dictionary<string, PropertyEventsPath>();
                            currentPropertyEventsPath = new PropertyEventsPath();
                            _propertiesEvents.Add(currentpathPropertyName, currentPropertyEventsPath);
                        }
                        else
                        {
                            currentPropertyEventsPath = _propertiesEvents.GetValueOrAddIfNotExists(currentpathPropertyName, () => new PropertyEventsPath());
                        }
                        for (int pathPos = propertyPathPos, i = 1; i < pathCount; pathPos++, i++)
                        {
                            PropertyEventsPath child;
                            if (currentPropertyEventsPath.PropertiesEvents == null)
                            {
                                currentPropertyEventsPath.PropertiesEvents = new Dictionary<string, PropertyEventsPath>();
                                child = new PropertyEventsPath();
                                currentPropertyEventsPath.PropertiesEvents.Add(propertyPath[pathPos], child);
                            }
                            else
                            {
                                child = currentPropertyEventsPath.PropertiesEvents.GetValueOrAddIfNotExists(propertyPath[pathPos], () => new PropertyEventsPath());
                            }
                            currentPropertyEventsPath = child;
                        }
                        if (currentPropertyEventsPath.Events == null)
                            currentPropertyEventsPath.Events = new List<Delegate>();
                        currentPropertyEventsPath.Events.Add(propertyChangedHandler);
                    }
                }
                else
                {
                    throw new InvalidOperationException(string.Format(
                        "You can't specify a property that has a parent type which is not derived from NotifyingObject<{0}>. "
                        + "Details: The type \"{0}\" is not derived from NotifyingObject<{0}>", propertyType.FullName));
                }
            }
            else
            {
                PropertyEventsPath currentPropertyEventsPath;
                if (_propertiesEvents == null)
                {
                    _propertiesEvents = new Dictionary<string, PropertyEventsPath>();
                    currentPropertyEventsPath = new PropertyEventsPath();
                    currentPropertyEventsPath.Events = new List<Delegate>();
                    _propertiesEvents.Add(currentpathPropertyName, currentPropertyEventsPath);
                }
                else
                {
                    currentPropertyEventsPath = _propertiesEvents.GetValueOrAddIfNotExists(currentpathPropertyName, () => new PropertyEventsPath());
                    if (currentPropertyEventsPath.Events == null)
                    {
                        currentPropertyEventsPath.Events = new List<Delegate>();
                    }
                }
                currentPropertyEventsPath.Events.Add(propertyChangedHandler);
            }
        }

        /// <summary>
        /// This method must be used in derived objects next way: set { SetNotifyingProperty(() => PropertyName, ref fieldForPropertyName, value); }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionWithPropertyName"></param>
        /// <param name="fieldForPropertyName"></param>
        /// <param name="value"></param>
        protected void SetNotifyingProperty<T>(Expression<Func<T>> expressionWithPropertyName, ref T fieldForPropertyName, T value)
        {
            if (!fieldForPropertyName.EqualsWithNullHandling(value))
            {
                T oldValue = fieldForPropertyName;
                fieldForPropertyName = value;
                string propertyName;
                PropertyEventsPath currentPropertyEventsPath;
                if (_propertiesEvents != null)
                {
                    propertyName = expressionWithPropertyName.GetBodyMemberName();
                    if (_propertiesEvents.TryGetValue(propertyName, out currentPropertyEventsPath))
                    {
                        if (oldValue == null)
                        {
                            if (currentPropertyEventsPath.PropertiesEvents != null)
                            {
                                try
                                {
                                    INotifyingObject newValNotifyingObj = (INotifyingObject)value;
                                    newValNotifyingObj.SetPropertiesEvents(MergePropertyEventsPaths(newValNotifyingObj.GetPropertiesEvents(), currentPropertyEventsPath.PropertiesEvents));
                                }
                                catch (InvalidCastException ex)
                                {
                                    throw new InvalidOperationException(string.Format("Early handlers were subscribed to child properties: \"{0}\" "
                                    + "but it's a violating operation because  a parent property \"{1}\" doesn't implement \"{3}\"",
                                    string.Join(", ", currentPropertyEventsPath.PropertiesEvents.Select(pair => string.Format("{0}: {1}", pair.Key, pair.Value)).ToArray()),
                                    propertyName, typeof(INotifyingObject), ex));
                                }
                                currentPropertyEventsPath.PropertiesEvents = null;
                                if (currentPropertyEventsPath.Events == null)
                                {
                                    _propertiesEvents.Remove(propertyName);
                                }
                            }
                        }

                        if (currentPropertyEventsPath.Events != null)
                        {
                            ChangedEventArgs<T> e = new ChangedEventArgs<T>(oldValue, value);
                            foreach (ChangedEventHandler<T> changedEventHandler in currentPropertyEventsPath.Events)
                            {
                                changedEventHandler(this, e);
                            }
                            return;
                        }
                    }
                }
                else
                {
                    currentPropertyEventsPath = null;
                    propertyName = null;
                }

                INotifyingObject oldValNotifyingObj = oldValue as INotifyingObject;
                if(oldValNotifyingObj != null)
                {
                    oldValNotifyingObj.ChildrenChanged -= OnChildrenChanged;
                }
                if (value != null)
                {
                    INotifyingObject newValNotifyingObj = value as INotifyingObject;
                    if (newValNotifyingObj != null)
                    {
                        newValNotifyingObj.ChildrenChanged += OnChildrenChanged;
                    }
                }
                else
                {
                    var oldValNotifyingObjPropertiesEvents = oldValNotifyingObj.GetPropertiesEvents();
                    if (oldValNotifyingObjPropertiesEvents != null)
                    {
                        if(currentPropertyEventsPath == null)
                        {
                            currentPropertyEventsPath = new PropertyEventsPath{
                                PropertiesEvents = oldValNotifyingObjPropertiesEvents
                            };
                            if (propertyName == null)
                                propertyName = expressionWithPropertyName.GetBodyMemberName();
                            _propertiesEvents.Add(propertyName, currentPropertyEventsPath);
                        }
                        else
                        {
                            currentPropertyEventsPath.PropertiesEvents = MergePropertyEventsPaths(currentPropertyEventsPath.PropertiesEvents, oldValNotifyingObjPropertiesEvents);
                        }
                    }
                }
                OnChildrenChanged(this, null);
            }
        }

        private static Dictionary<string, PropertyEventsPath> MergePropertyEventsPaths(Dictionary<string, PropertyEventsPath> mergingPath, Dictionary<string, PropertyEventsPath> pathToMerge)
        {
            if (mergingPath == null)
                return pathToMerge;
            else if (pathToMerge == null)
                return mergingPath;
            else
            {
                foreach (var pair in pathToMerge)
                {
                    PropertyEventsPath existingPropertyEventsPath;
                    if (mergingPath.TryGetValue(pair.Key, out existingPropertyEventsPath))
                    {
                        existingPropertyEventsPath.Events = existingPropertyEventsPath.Events.Merge(pair.Value.Events);
                        pair.Value.PropertiesEvents = MergePropertyEventsPaths(pair.Value.PropertiesEvents, existingPropertyEventsPath.PropertiesEvents);
                    }
                    else
                    {
                        mergingPath.Add(pair.Key, pair.Value);
                    }
                }
                return mergingPath;
            }
        }

        //public void Set<T>(Expression<Func<SettingsType, T>> expressionWithPropertyName, T newValue)
        //{
        //    MemberExpression propertyMemberExpression = (MemberExpression)expressionWithPropertyName.Body;
        //    MemberExpression parentMemberExpression = (MemberExpression)propertyMemberExpression.Expression;
        //    var parentGetter = parentMemberExpression.Bo

        //    //memberExpression.Member;
        //}
        //public void Set(Expression<Action<SettingsType>> expressionWithPropertyName)
        //{

        //}

    }
}
