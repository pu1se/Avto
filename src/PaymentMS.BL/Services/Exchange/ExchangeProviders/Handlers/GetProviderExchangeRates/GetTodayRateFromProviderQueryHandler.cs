using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Converter;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.API;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates
{
    public class GetTodayRateFromProviderQueryHandler : QueryHandler<GetTodayRateFromProviderQuery, CallDataResult<TodayProviderExchangeRateResponse>>
    {
        private ProviderFactory ProviderFactory { get; }
        public GetTodayRateFromProviderQueryHandler(
            Storage storage, 
            LogService logger,
            ProviderFactory providerFactory
            ) : base(storage, logger)
        {
            ProviderFactory = providerFactory;
        }

        protected override async Task<CallDataResult<TodayProviderExchangeRateResponse>> HandleCommandAsync(GetTodayRateFromProviderQuery query)
        {
            Guid? organizationId = null;
            if (query.Provider.IsCustom())
            {
                organizationId = query.OrganizationId;
            }

            var result = await Storage.CurrencyExchangeRates
                .Where(
                    e => 
                    e.ExchangeProvider == query.Provider.Value &&
                    e.FromCurrencyCode == query.FromCurrency.Value.ToString() &&
                    e.ToCurrencyCode == query.ToCurrency.Value.ToString() &&
                    e.OrganizationId == organizationId
                )
                .OrderByDescending(e => e.ExchangeDate)
                .GetAsync();


            if (result == null)
            {
                return NotFoundResult<TodayProviderExchangeRateResponse>($"Rate for provider {query.Provider}, from currency {query.FromCurrency} to currency {query.ToCurrency} was not found.");
            }

            return SuccessResult(new TodayProviderExchangeRateResponse
            {
                FromCurrency = result.FromCurrencyCode.AsEnum<CurrencyType>(),
                ToCurrency = result.ToCurrencyCode.AsEnum<CurrencyType>(),
                ExchangeDate = result.ExchangeDate,
                ExchangeProvider = result.ExchangeProvider,
                ExchangeRate = result.Rate
            });
        }
    }
}
