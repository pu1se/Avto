using System;
using System.Collections.Generic;
using System.Linq;
using PaymentMS.BL.Services.Currency.ResponseModels;
using PaymentMS.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization;
using PaymentMS.DAL.Enums;

namespace PaymentMS.UI.Front.Currency
{
    public class CurrencyConverterChainBuilder
    {
        private decimal FromAmount { get; }
        private CurrencyType FromCurrency { get; }
        private List<RateResponse> ExchangeRateList { get; }

        public CurrencyConverterChainBuilder(List<RateResponse> exchangeRateList, decimal fromAmount,
            CurrencyType fromCurrency)
        {
            ExchangeRateList = exchangeRateList;
            FromAmount = fromAmount;
            FromCurrency = fromCurrency;
        }

        public ConvertedPriceAndCurrency To(CurrencyType toCurrency)
        {
            var rate = ExchangeRateList.FirstOrDefault(x => x.FromCurrency == FromCurrency && x.ToCurrency == toCurrency);

            if (rate == null)
            {
                return new ConvertedPriceAndCurrency
                {
                    OriginalAmount = FromAmount,
                    OriginalCurrency = FromCurrency,
                    ConvertedCurrency = FromCurrency,
                    ConvertedAmount = FromAmount
                };
            }

            var convertedAmount = rate.ExchangeRateWithSurcharge * FromAmount;
            convertedAmount = Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero);

            return new ConvertedPriceAndCurrency
            {
                OriginalAmount = FromAmount,
                OriginalCurrency = FromCurrency,
                ConvertedCurrency = toCurrency,
                ConvertedAmount = convertedAmount
            };
        }
    }
}
