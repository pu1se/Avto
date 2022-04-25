using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace PaymentMS.BL._Core.ApiRequests
{
    public static class ApiRequestErrorHandler
    {
        public static CallResult HandleResponse(IRestResponse response)
        {
            var statusCode = (int)response.StatusCode;

            if (statusCode == 0)
            {
                return new CallResult("Service is unavailable or can not be reached.", ErrorType.ThirdPartyApiError417);
            }

            if (statusCode == 200)
            {
                return new CallResult();
            }

            if (statusCode == 404)
            {
                return new NotFoundResult(response.Content);
            }

            if (statusCode == 400)
            {
                return new CallResult(response.Content, ErrorType.ThirdPartyApiError417);
            }

            if (statusCode >= 500)
            {
                return new CallResult($"Status code: {statusCode}. Exception: {response.Content}", ErrorType.ThirdPartyApiError417);
            }

            return new CallResult(response.Content, ErrorType.ThirdPartyApiError417);
        }
    }
}
