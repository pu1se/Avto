using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentMS.UI.Web.ApiRequests;
using RestSharp;

namespace PaymentMS.UI.Web
{
    public class ApiRequestExecuter
    {
        public ApiRequestExecuter(string baseUrl, string apiAuthClientId, string apiAuthClientSecret)
        {
            this.BaseUrl = baseUrl;
            this.Token = GetToken(baseUrl, apiAuthClientId, apiAuthClientSecret);
        }

        private string BaseUrl { get; }

        private TokenModel Token { get; }

        private RestRequest GenerateRequest(Method httpMethod, string path, object data)
        {
            var request = new RestRequest(path, httpMethod, DataFormat.Json);

            if (this.Token != null)
            {
                request.AddHeader("authorization", this.Token.token_type + " " + this.Token.access_token);
            }

            if (data != null)
            {
                if (httpMethod == Method.GET)
                {
                    var propertyList = new List<PropertyInfo>(data.GetType().GetProperties());

                    foreach (var property in propertyList)
                    {
                        var propertyValue = property.GetValue(data);
                        var propertyName = property.Name;
                        request.AddParameter(propertyName, propertyValue);
                    }
                }
                else
                {
                    request.AddJsonBody(data);
                }
            }

            return request;
        }

        private async Task<ApiCallResult> SendRequestAsync(Method httpMethod, string path, object data)
        {
            if (path.Contains(this.BaseUrl))
            {
                path = path.Replace(this.BaseUrl, string.Empty);
            }

            var restClient = new RestClient(this.BaseUrl);
            var request = GenerateRequest(httpMethod, path, data);

            var response = await restClient.ExecuteAsync(request);
            var callResult = ApiErrorHandler.HandleResponse(response);
            return callResult;
        }

        private async Task<ApiCallDataResult<T>> SendRequestAsync<T>(Method httpMethod, string path, object data)
        {
            if (path.Contains(this.BaseUrl))
            {
                path = path.Replace(this.BaseUrl, string.Empty);
            }

            var restClient = new RestClient(this.BaseUrl);
            var request = GenerateRequest(httpMethod, path, data);

            var response = await restClient.ExecuteAsync<T>(request);
            var callResult = ApiErrorHandler.HandleResponse(response);
            return new ApiCallDataResult<T>(response.Data, callResult.ErrorMessage);
        }


        private TokenModel GetToken(string baseUrl, string apiAuthClientId, string apiAuthClientSecret)
        {
            var request = new RestRequest("token", Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", apiAuthClientId);
            request.AddParameter("client_secret", apiAuthClientSecret);

            var client = new RestClient(baseUrl);
            var response = client.Execute<TokenModel>(request);
            ApiErrorHandler.HandleResponse(response);
            return response.Data;
        }

        public Task<ApiCallDataResult<TResult>> GetAsync<TResult>(string path, object data = null) where TResult : class, new()
        {
            return SendRequestAsync<TResult>(Method.GET, path, data);
        }

        public Task<ApiCallResult> PutAsync(string path, object data)
        {
            return SendRequestAsync(Method.PUT, path, data);
        }

        public Task<ApiCallResult> PostAsync(string path, object data = null)
        {
            return SendRequestAsync(Method.POST, path, data);
        }

        public Task<ApiCallDataResult<TResult>> PostAsync<TResult>(string path, object data) where TResult : class, new()
        {
            return SendRequestAsync<TResult>(Method.POST, path, data);
        }

        private class TokenModel
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }
    }
}
