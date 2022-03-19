using System;
using System.Linq.Expressions;

namespace AG.Utilities.Binding
{
    public abstract class BindingObjectBase
    {
        public event PropertyChangedEventHandler<BindingObjectBase> PropertyChanged;

        /// <summary>
        /// Using: set { SetNotifyingProperty(() => PropertyName, ref fieldForPropertyName, value); }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionWithPropertyName"></param>
        /// <param name="fieldForPropertyName"></param>
        /// <param name="value"></param>
        protected void SetNotifyingProperty<T>(Expression<Func<T>> expressionWithPropertyName, ref T fieldForPropertyName, T value)
        {
            if (fieldForPropertyName == null && value != null || !fieldForPropertyName.Equals(value))
            {
                PropertyChangedEventHandler<BindingObjectBase> handler = PropertyChanged;
                if (handler != null)
                {
                    T oldValue = fieldForPropertyName;
                    fieldForPropertyName = value;
                    string propertyName = expressionWithPropertyName.GetBodyMemberName();
                    handler(this, new PropertyChangedExtendedEventArgs<T>(propertyName, oldValue, value));
                }
                else
                {
                    fieldForPropertyName = value;
                }
            }
        }
    }
}
