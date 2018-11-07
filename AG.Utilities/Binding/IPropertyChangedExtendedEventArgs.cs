namespace AG.Utilities.Binding
{
    public interface IPropertyChangedExtendedEventArgs
    {
        object OldValueObj { get; }
        object NewValueObj { get; }
        string PropertyName { get; }
    }
}
