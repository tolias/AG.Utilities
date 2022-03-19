using System;

namespace AG.Utilities.ErrorHandling
{
    public class ErrorDetailsArgs<TException> : EventArgs
        where TException : Exception
    {
        private readonly string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                if(_errorMessage == null && ThrewnException != null)
                {
                    return ThrewnException.Message;
                }
                return _errorMessage;
            }
        }
        public readonly TException ThrewnException;
        public ErrorActions Action;

        public ErrorDetailsArgs(string errorMessage, TException threwnException = null)
        {
            _errorMessage = errorMessage;
            ThrewnException = threwnException;
        }

        public bool ErrorMessageIsNotSet { get { return _errorMessage == null; } }
    }
}
