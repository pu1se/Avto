using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesQueryHandler : QueryHandler<EmptyQuery, CallListDataResult<AvailableCurrencyResponse>>
    {
        public GetAvailableCurrenciesQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<AvailableCurrencyResponse>> HandleCommandAsync(EmptyQuery query)
        {
            var list = await Storage.Currencies
                .Select(e => new AvailableCurrencyResponse
                {
                    Code = e.Code,
                    Name = e.Name
                })
                .ToListAsync();
            return SuccessListResult(list);
        }
    }
}
