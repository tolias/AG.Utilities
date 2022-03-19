namespace AG.Utilities.ErrorHandling
{
    public enum ErrorActions
    {
        ThrowException,
        TryAnotherHandler,
        Retry,
        Ignore,
        Cancel
    }
}
