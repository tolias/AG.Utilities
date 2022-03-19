using System;

namespace AG.Utilities.ErrorHandling
{
    public interface IDefaultErrorNotifier : IErrorNotifier<Exception, ErrorDetailsArgs<Exception>>
    {
    }
}
