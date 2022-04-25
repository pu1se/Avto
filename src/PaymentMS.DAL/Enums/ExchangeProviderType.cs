using System.ComponentModel;

namespace PaymentMS.DAL.Enums
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