using System.Collections.Generic;
using Avto.BL.Services.Currency.ResponseModels;
using Avto.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization;
using Avto.DAL.Enums;

namespace Avto.UI.Front.Currency
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
