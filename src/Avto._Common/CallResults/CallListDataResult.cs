using System;
using System.Collections.Generic;
using System.Text;

namespace Avto
{
    public class CallListDataResult<T> : CallDataResult<IEnumerable<T>>
    {
        public CallListDataResult(IEnumerable<T> data)
            : base(data)
        {
        }

        public CallListDataResult(string errorMessage, ErrorType errorType) 
            : base(errorMessage, errorType)
        {
        }

        public CallListDataResult() : base()
        {
        }
    }
}
