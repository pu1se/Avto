using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.Exchange.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesQueryHandler : QueryHandler<EmptyQuery, CallListDataResult<AvailableCurrencyResponse>>
    {
        public GetAvailableCurrenciesQueryHandler(Storage storage, LogService logService) : base(storage, logService)
        {
        }

        protected override async Task<CallListDataResult<AvailableCurrencyResponse>> HandleCommandAsync(EmptyQuery query)
        {
            LogService.WriteInfo("Get currencies");
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
