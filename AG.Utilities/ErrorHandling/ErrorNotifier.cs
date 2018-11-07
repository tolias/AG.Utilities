using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Utilities.ErrorHandling
{
    public abstract class ErrorNotifier<TException, TErrorDetails> : IErrorNotifier<TException, TErrorDetails>
        where TErrorDetails : ErrorDetailsArgs<TException>
        where TException : Exception
    {
        public event ErrorHandler<TException, TErrorDetails> ErrorOccured;

        protected virtual ErrorActions NotifyAboutError(TErrorDetails errorDetails, ErrorHandler<TException, TErrorDetails> errorHandler = null, ExceptionThrower exceptionThrower = null)
        {
            if(errorHandler != null)
            {
                var errorAction = NotifyAboutError(errorDetails, errorHandler, exceptionThrower, this);
                if(errorAction != ErrorActions.TryAnotherHandler)
                {
                    return errorAction;
                }
            }
            return NotifyAboutError(errorDetails, ErrorOccured, exceptionThrower, this);
        }

        public static ErrorActions NotifyAboutError(TErrorDetails errorDetails,
            ErrorHandler<TException, TErrorDetails> errorHandler, ExceptionThrower exceptionThrower = null, object sender = null)
        {
            ErrorActions errorAction;
            if (errorHandler != null)
            {
                errorHandler(sender, errorDetails);
                errorAction = errorDetails.Action;
            }
            else
            {
                errorAction = ErrorActions.ThrowException;
            }
            if (errorAction == ErrorActions.ThrowException)
            {
                if(exceptionThrower == null)
                {
                    if (errorDetails.ErrorMessageIsNotSet && errorDetails.ThrewnException != null)
                    {
                        throw errorDetails.ThrewnException;
                    }
                    else
                    {
                        throw new NotifiedErrorException<TErrorDetails, TException>(errorDetails);
                    }
                }
                else
                {
                    throw exceptionThrower(errorDetails.ErrorMessage, errorDetails.ThrewnException);
                }
            }
            return errorAction;
        }

        public virtual void Dispose()
        {
            ErrorOccured = null;
        }
    }
}
