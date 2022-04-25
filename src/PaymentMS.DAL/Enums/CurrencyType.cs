using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PaymentMS.DAL.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CurrencyType
    {
        [Description("Euro")]
        EUR,

        [Description("US Dollar")]
        USD,

        [Description("Norwegian Krone")]
        NOK,

        [Description("Danish Krone")]
        DKK,

        [Description("Swedish Krona")]
        SEK, 

        [Description("Swiss Franc")]
        CHF,

        [Description("New Zealand Dollar")]
        NZD,

        [Description("Canadian Dollar")]
        CAD,

        [Description("British Pound")]
        GBP,

        [Description("Indonesian Rupiah")]
        IDR,

        [Description("Australian Dollar")]
        AUD,

        [Description("Brazilian Real")]
        BRL,

        [Description("Japanese Yen")]
        JPY,

        [Description("Czech Koruna")]
        CZK,

        [Description("Bulgarian Lev")]
        BGN,

        [Description("Polish zloty")]
        PLN,

        [Description("Hungarian Forint")]
        HUF,

        [Description("Romanian Lei")]
        RON,

        [Description("Croatian Kuna")]
        HRK,

        [Description("Chinese Yuan")]
        CNY,

        [Description("Israeli Shekel")]
        ILS,

        [Description("Indian rupee")]
        INR,

        [Description("South Korean Won")]
        KRW,

        [Description("Turkish Lira")]
        TRY,

        [Description("Singapore Dollar")]
        SGD,

        [Description("Hong Kong dollar")]
        HKD,

        [Description("Malaysian ringgit")]
        MYR,

        [Description("Thai baht")]
        THB,
    }
}
