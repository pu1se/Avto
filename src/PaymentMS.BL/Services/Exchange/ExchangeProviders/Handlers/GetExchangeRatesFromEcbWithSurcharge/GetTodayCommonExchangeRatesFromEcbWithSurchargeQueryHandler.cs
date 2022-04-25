using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Currency.ResponseModels;
using PaymentMS.BL.Services.Exchange.CurrencyConverter;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.API;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.GetExchangeRatesFromEcbWithSurcharge
{
    public class GetTodayCommonExchangeRatesFromEcbWithSurchargeQueryHandler : QueryHandler<EmptyQuery, CallListDataResult<CommonExchangeRatesFromEcbWithSurchargeResponse>>
    {
        private AppSettings AppSettings { get; }
        private EcbProviderApi EcbApi { get; }

        public GetTodayCommonExchangeRatesFromEcbWithSurchargeQueryHandler(
            Storage storage, 
            LogService logger,
            AppSettings appSettings,
            EcbProviderApi ecbApi) : base(storage, logger)
        {
            AppSettings = appSettings;
            EcbApi = ecbApi;
        }

        protected override async Task<CallListDataResult<CommonExchangeRatesFromEcbWithSurchargeResponse>> HandleCommandAsync(EmptyQuery command)
        {
            var baseCurrency = EcbApi.GetBaseCurrency();
            var currentDate = DateTime.UtcNow.Date;
            var exchangeRateEntitiesForCurrentDate = await Storage.CurrencyExchangeRates
                .Where(e => e.ExchangeDate == currentDate)
                .OrderByDescending(e => e.ExchangeDate)
                .ToListAsync();
            var exchangeCurrencyFunc = new ExchangeCurrencyFunc(
                baseCurrency, 
                exchangeRateEntitiesForCurrentDate, 
                AppSettings.Currency);
            var currencyList = EcbApi.GetSupportedCurrencies();

            var result = new List<CommonExchangeRatesFromEcbWithSurchargeResponse>();
            foreach (var fromCurrency in currencyList)
            {
                foreach (var toCurrency in currencyList)
                {
                    var exchangeRateItem = exchangeCurrencyFunc.From(1, fromCurrency).To(toCurrency);

                    result.Add(new CommonExchangeRatesFromEcbWithSurchargeResponse
                    {
                        FromCurrency = exchangeRateItem.FromCurrency,
                        ToCurrency = exchangeRateItem.ExchangeCurrency,
                        ExchangeRate = exchangeRateItem.ExchangeRate,
                        ExchangeDate = currentDate
                    });
                }
            }

            return SuccessListResult(result);
        }
    }
}
