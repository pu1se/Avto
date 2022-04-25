using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PaymentMS.DAL.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentStatusType
    {
        Unknown = 0,
        Success,
        Fail,
        Refund
    }
}
