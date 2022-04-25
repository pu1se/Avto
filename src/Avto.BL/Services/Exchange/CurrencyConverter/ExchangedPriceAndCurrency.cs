using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.CurrencyConverter
{
    public class ExchangedPriceAndCurrency
    {
        public decimal FromAmount { get; set; }
        public CurrencyType FromCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public CurrencyType ExchangeCurrency { get; set; }
    }
}
