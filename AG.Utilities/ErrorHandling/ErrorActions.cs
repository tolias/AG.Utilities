using System;
using System.Collections.Generic;
using System.Text;

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
