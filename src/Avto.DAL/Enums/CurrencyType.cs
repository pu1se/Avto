using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Avto.DAL.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CurrencyType
    {
        [Description("Euro")]
        EUR,

        [Description("US Dollar")]
        USD,
        
        [Description("Polish zloty")]
        PLN,

        [Description("Belarusian Ruble")]
        BYR,

        [Description("Russian Ruble")]
        RUB,

        [Description("Swedish Krona")]
        SEK,

        [Description("Ukrainian Hryvnia")]
        UAH,

        [Description("Gold (troy ounce)")]
        XAU,

        [Description("Silver (troy ounce)")]
        XAG,

        [Description("Norwegian Krone")]
        NOK,
        
        [Description("Swiss Franc")]
        CHF,

        [Description("British Pound")]
        GBP,

        [Description("Japanese Yen")]
        JPY,

        [Description("Turkish Lira")]
        TRY,
    }
}
