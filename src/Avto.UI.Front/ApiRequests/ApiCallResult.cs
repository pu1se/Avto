﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avto.UI.Front.ApiRequests
{
    public class ApiCallResult
    {
        public bool IsSuccess => ErrorMessage.IsNullOrEmpty();
        public string ErrorMessage { get; protected set; }

        public ApiCallResult(string errorMessage = null)
        {
            ErrorMessage = errorMessage;
        }
    }
}
