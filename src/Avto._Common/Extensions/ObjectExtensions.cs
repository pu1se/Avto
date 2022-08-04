using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Avto
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object objectForSerialization)
        {
            if (objectForSerialization == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(objectForSerialization);
        }

        public static string ToFormattedString(this Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            return $"Exception message: {exception.Message}. {Environment.NewLine}" +
                   $"Stack-trace: {exception.StackTrace}.";
        }

        public static IEnumerable<DateTime> EnumerateTo(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }

        public static decimal ToRoundedRate(this decimal rate)
        {
            var result = Math.Round(rate, 10, MidpointRounding.AwayFromZero);
            return result/1.000000000000000000000000000000000m;
        }
    }
}
