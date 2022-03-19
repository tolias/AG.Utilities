using System;

namespace AG.Utilities.ErrorHandling
{
    public abstract class DefaultErrorNotifier : ErrorNotifier<Exception, ErrorDetailsArgs<Exception>>, IDefaultErrorNotifier
    {
        protected virtual ErrorActions NotifyAboutError(string errorMessage = null, Exception threwnException = null,
            ErrorHandler<Exception, ErrorDetailsArgs<Exception>> errorHandler = null, ExceptionThrower exceptionThrower = null)
        {
            return NotifyAboutError(new ErrorDetailsArgs<Exception>(errorMessage, threwnException), errorHandler, exceptionThrower);
        }
    }
}
