namespace AG.Utilities.Binding
{
    public delegate void PropertyChangedEventHandler<TSender>(TSender sender, IPropertyChangedExtendedEventArgs e);
    public delegate void ChangedEventHandler<T>(object sender, ChangedEventArgs<T> e);
}
