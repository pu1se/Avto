﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Avto.BL;
using RestSharp;

namespace Avto.UI.Front.ApiRequests
{
    public static class ApiErrorHandler
    {
        public static ApiCallResult HandleResponse(RestResponse response)
        {
            var statusCode = (int)response.StatusCode;

            if (statusCode == 200)
            {
                return new ApiCallResult();
            }

            if (statusCode == 404)
            {
                return new ApiCallResult("Not Found");
            }

            if (statusCode == 400)
            {
                return new ApiCallResult($"Api error Uri: {response.ResponseUri.AbsolutePath}. Validation error: {response.Content.Replace(Environment.NewLine, "<br/>")}");
            }
            

            if (statusCode >= 500)
            {
                throw new ThirdPartyApiException($"Api exception Uri: {response.ResponseUri.AbsolutePath}. Exeption: {response.Content.Replace(Environment.NewLine, "<br/>")}");
            }

            return new ApiCallResult($"Error Uri: {response.Request.Resource}. Response: {response.Content.Replace(Environment.NewLine, "<br/>")}");
        }
    }
}
