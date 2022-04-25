using System.Collections.Generic;
using PaymentMS.BL.Services.Currency.ResponseModels;
using PaymentMS.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization;
using PaymentMS.DAL.Enums;

namespace PaymentMS.UI.Front.Currency
{
    public class CurrencyConverter
    {
        public List<RateResponse> ExchangeRateList { get; }

        public CurrencyConverter(List<RateResponse> exchangeRateList)
        {
            ExchangeRateList = exchangeRateList;
        }

        public CurrencyConverterChainBuilder From(decimal originalAmount, CurrencyType originalCurrency)
        {
            return new CurrencyConverterChainBuilder(ExchangeRateList, originalAmount, originalCurrency);
        } 
    }
}
