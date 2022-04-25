using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Avto.BL;

namespace Avto.Api._Core
{
    public class ExceptionHandlerMiddleware
    {
        private RequestDelegate Next { get; }

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context, LogService logger)
        {
            try
            {
                await Next(context);
            }
            catch (Exception exception)
            {
                logger.WriteError(exception.ToFormattedString());
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Server error: {exception.ToFormattedString()}.{Environment.NewLine}" +
                                                  $"Log Id: {logger.LogId}.{Environment.NewLine}");
            }
        }
    }
}
