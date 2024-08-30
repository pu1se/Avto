using System;
using System.Collections.Generic;
using System.Text;

namespace Avto
{
    public class CallListResult<T> : CallResult<IEnumerable<T>>
    {
        public CallListResult(IEnumerable<T> data) : base(data ?? Array.Empty<T>())
        {
        }

        public CallListResult(string errorMessage, ErrorType errorType) 
            : base(errorMessage, errorType)
        {
        }

        public CallListResult() : base(Array.Empty<T>())
        {
        }
    }
}
