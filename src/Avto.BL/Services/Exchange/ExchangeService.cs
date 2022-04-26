using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public Task<CallResult> RefreshExchangeRates()
        {
            return GetHandler<RefreshExchangeRatesCommandHandler>().HandleAsync(EmptyCommand.Value);
        }

        public Task<CallListDataResult<GetExchangeRatesQueryResponse>> GetExchangeRates(GetExchangeRatesQuery query)
        {
            return GetHandler<GetExchangeRatesQueryHandler>().HandleAsync(query);
        }
    }
}
