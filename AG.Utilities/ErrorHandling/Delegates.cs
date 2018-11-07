using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Utilities.ErrorHandling
{
    public delegate void ErrorHandler<TException, TErrorDetails>(object sender, TErrorDetails e)
        where TErrorDetails : ErrorDetailsArgs<TException>
        where TException : Exception;

    public delegate Exception ExceptionThrower(string exceptionMessage, Exception innerException);
}
