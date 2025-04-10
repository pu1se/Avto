﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avto.BL.Services.ParseWebPage
{
    public static class Extensions
    {
        public static string NormalizeCssSelector(this string cssSelector)
        {
            cssSelector = cssSelector.Trim().Replace("\"", "'");
            if (!cssSelector.StartsWith("@"))
            {
                return cssSelector;
            }

            var key = cssSelector.Substring(1);
            var selector = "[autotest='" + key + "']";
            return selector;
        }

        public static string ToFormattedExceptionDescription(this Exception exception)
        {
            var stackTrace = exception.StackTrace;
            var errorMessage = exception.Message;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                errorMessage += "; " + exception.Message;
            }

            // todo: add screenshot 
            return $"Exception message: {errorMessage}. {Environment.NewLine}" +
                   $"Stack-trace: {stackTrace}. {Environment.NewLine}";
        }
    }
}
