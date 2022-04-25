using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentMS.UI.Web.ApiRequests
{
    public class ApiCallDataResult<T> : ApiCallResult
    {
        public T Data { get; }

        public ApiCallDataResult(T data, string errorMessage = null) : base(errorMessage)
        {
            Data = errorMessage.IsNullOrEmpty() ? data : default(T);
        }
    }
}
