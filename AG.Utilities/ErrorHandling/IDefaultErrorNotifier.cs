using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Utilities.ErrorHandling
{
    public interface IDefaultErrorNotifier : IErrorNotifier<Exception, ErrorDetailsArgs<Exception>>
    {
    }
}
