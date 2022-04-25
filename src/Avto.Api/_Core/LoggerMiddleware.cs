using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Avto.BL;
using Avto.BL._Core.Logger;

namespace Avto.Api._Core
{
    public class LoggerMiddleware
    {
        private RequestDelegate Next { get; }

        public LoggerMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context, LogService logger)
        {
            var timer = new Stopwatch();
            if (context.Request.Method == "GET")
            {
                timer.Start();
                await Next(context);
                timer.Stop();

                if (logger.IsInit())
                {
                    await SaveLogs(context, logger, timer);
                }
            }
            else
            {
                timer.Start();
                await Next(context);
                timer.Stop();

                await SaveLogs(context, logger, timer);
            }
        }

        private static async Task SaveLogs(HttpContext context, LogService logger, Stopwatch timer)
        {
            logger.AddHttpInfo(new LogHttpInfo
            {
                PathToAction = context.Request.Path,
                ResponseCode = context.Response.StatusCode,
                HttpMethod = context.Request.Method,
                ExecutionTimeInMilliSec = timer.ElapsedMilliseconds
            });
            await logger.SaveAsync();
        }
    }
}
