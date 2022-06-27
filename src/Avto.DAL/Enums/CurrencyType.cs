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
        SEK
    }
}
