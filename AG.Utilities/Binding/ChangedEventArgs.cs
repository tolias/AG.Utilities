using System;

namespace AG.Utilities.Binding
{
    public class ChangedEventArgs<T> : EventArgs
    {
        public readonly T OldValue;
        public readonly T NewValue;

        public ChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
