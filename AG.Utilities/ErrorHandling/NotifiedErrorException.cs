using System;

namespace AG.Utilities.ErrorHandling
{
    public class NotifiedErrorException<TErrorDetails, TInnerException> : Exception
        where TInnerException : Exception
        where TErrorDetails : ErrorDetailsArgs<TInnerException>
    {
        public readonly TErrorDetails ErrorDetails;

        public NotifiedErrorException(TErrorDetails errorDetails)
            : base (errorDetails.ErrorMessage, errorDetails.ThrewnException)
        {
            ErrorDetails = errorDetails;
        }
    }
}
