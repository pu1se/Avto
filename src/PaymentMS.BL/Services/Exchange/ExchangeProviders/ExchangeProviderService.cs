using System;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Currency.ResponseModels;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates.Handlers.CalculateAllTodayExchangeRates;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Commands.RefreshAllProvidersTodayRates;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.GetExchangeRatesFromEcbWithSurcharge;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.RefreshAllProvidersTodayRates;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders
{
    public class ExchangeProviderService : BaseService
    {
        public ExchangeProviderService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallListDataResult<ExchangeProvidersResponse>> GetExchangeProviders(CurrencyType? filterByCurrency = null)
        {
            var query = new GetProvidersQuery{FilterByCurrency = filterByCurrency};
            return GetHandler<GetProvidersQueryHandler>().HandleAsync(query);
        }

        public Task<CallDataResult<TodayProviderExchangeRateResponse>> GetTodayRateFromProvider(GetTodayRateFromProviderQuery query)
        {
            return GetHandler<GetTodayRateFromProviderQueryHandler>().HandleAsync(query);
        }

        public Task<CallResult> RefreshTodayRatesForProvider(ExchangeProviderType provider)
        {
            var command = new RefreshTodayRatesForProviderCommand
            {
                RefreshForProvider = provider
            };
            return GetHandler<RefreshTodayRatesForProviderCommandHandler>().HandleAsync(command);
        }

        public Task<CallResult> RefreshTodayRatesForAllProviders()
        {
            return GetHandler<RefreshTodayRatesForProviderCommandHandler>().HandleAsync(new RefreshTodayRatesForProviderCommand());
        }

        public Task<CallListDataResult<CommonExchangeRatesFromEcbWithSurchargeResponse>> GetTodayCommonExchangeRatesFromEcbWithSurcharge()
        {
            return GetHandler<GetTodayCommonExchangeRatesFromEcbWithSurchargeQueryHandler>().HandleAsync(EmptyQuery.Value);
        }
    }
}
