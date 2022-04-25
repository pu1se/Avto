using System.ComponentModel;

namespace Avto.DAL.Enums
{
    public enum ExchangeProviderType
    {
        [Description("European Central Bank")]
        ECB,

        [Description("Currency Layer")]
        CurrencyLayer,

        Custom,
    }
}