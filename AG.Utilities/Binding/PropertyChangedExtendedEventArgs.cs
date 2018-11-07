using System.ComponentModel;

namespace AG.Utilities.Binding
{
    public class PropertyChangedExtendedEventArgs<T> : PropertyChangedEventArgs, IPropertyChangedExtendedEventArgs
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public object OldValueObj { get { return OldValue; } }

        public object NewValueObj { get { return NewValue; } }

        public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
