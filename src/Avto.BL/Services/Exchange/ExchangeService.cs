using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.GetAllExchangeRatesForToday;
using Avto.BL.Services.Exchange.GetAvailableCurrencies;
using Avto.BL.Services.Exchange.GetExchangeRates;
using Avto.BL.Services.Exchange.RefreshExchangeRates;
using Avto.DAL;

namespace Avto.BL.Services.Exchange
{
    public class ExchangeService : BaseService
    {
        public ExchangeService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }


        public Task<CallListDataResult<AvailableCurrencyResponse>> GetAvailableCurrencies()
        {
            return GetHandler<GetAvailableCurrenciesQueryHandler>().HandleAsync(EmptyQuery.Value);
        }

        public Task<CallResult> RefreshExchangeRates()
        {
            return GetHandler<RefreshExchangeRatesCommandHandler>().HandleAsync(EmptyCommand.Value);
        }

        public Task<CallListDataResult<GetSpecificExchangeRatesInfoQueryResponse>> GetSpecificExchangeRatesInfo(GetSpecificExchangeRatesInfoQuery infoQuery)
        {
            return GetHandler<GetSpecificExchangeRatesInfoQueryHandler>().HandleAsync(infoQuery);
        }

        public Task<CallListDataResult<GetAllExchangeRatesForTodayQueryResponse>> GetAllExchangeRatesForToday()
        {
            return GetHandler<GetAllExchangeRatesForTodayQueryHandler>().HandleAsync(EmptyQuery.Value);
        }
    }
}
