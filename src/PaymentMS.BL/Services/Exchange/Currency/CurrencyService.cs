using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.Currency
{
    public class CurrencyService : BaseService
    {
        public CurrencyService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallListDataResult<AvailableCurrencyResponse>> GetAvailableCurrencies()
        {
            var handler = GetHandler<GetAvailableCurrenciesQueryHandler>();
            return handler.HandleAsync(EmptyQuery.Value);
        }
    }
}
