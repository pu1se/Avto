using System;
using System.Threading.Tasks;
using Avto.BL.Services.Currency.ResponseModels;
using Avto.BL.Services.Exchange.CalculatedExchangeRates.Handlers.CalculateAllTodayExchangeRates;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.Commands.RefreshAllProvidersTodayRates;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.GetExchangeRatesFromEcbWithSurcharge;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.RefreshAllProvidersTodayRates;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders;
using Avto.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.ExchangeProviders
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
