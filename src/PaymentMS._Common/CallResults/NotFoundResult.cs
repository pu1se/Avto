using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS
{
    public class NotFoundResult : CallResult
    {
        public NotFoundResult()
        {
            ErrorType = ErrorType.NotFoundError404;
        }

        public NotFoundResult(string errorMessage)
        {
            ErrorType = ErrorType.NotFoundError404;
            ErrorMessage = errorMessage;
        }
    }

    public class NotFoundResult<T> : CallDataResult<T>
    {
        public NotFoundResult(string errorMessage) : base(default(T))
        {
            ErrorType = ErrorType.NotFoundError404;
            ErrorMessage = errorMessage;
        }
    }
}
