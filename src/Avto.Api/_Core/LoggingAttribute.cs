using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Avto.BL;
using Avto.BL._Core.Logger;

namespace Avto.Api._Core
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        private LogService Logger { get; }

        public LoggingAttribute(LogService logger)
        {
            Logger = logger;
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (Logger.IsInit())
            {
                var responseAsJson = string.Empty;
                if (context.HttpContext.Request.Method != "GET")
                {
                    var result = context.Result;
                    if (result is JsonResult json)
                    {
                        responseAsJson = json.Value.ToJson();
                    }
                }

                Logger.AddHttpInfo(new LogHttpInfo
                {
                    PathToAction = context.HttpContext.Request.Path,
                    ResponseCode = context.HttpContext.Response.StatusCode,
                    HttpMethod = context.HttpContext.Request.Method,
                    HttpResponseAsJson = responseAsJson
                });
                
            }
            base.OnResultExecuted(context);
        }
    }
}
