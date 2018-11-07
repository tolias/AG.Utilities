using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Utilities.ErrorHandling
{
    public interface IErrorNotifier<TException, TErrorDetails> : IDisposable
        where TErrorDetails : ErrorDetailsArgs<TException>
        where TException : Exception
    {
        event ErrorHandler<TException, TErrorDetails> ErrorOccured;
    }
}
