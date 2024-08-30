using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.Exchange.GetAllExchangeRatesForToday
{
    public class GetAllExchangeRatesForTodayQueryHandler : QueryHandler<EmptyQuery, CallListResult<GetAllExchangeRatesForTodayQueryResponse>>
    {
        public GetAllExchangeRatesForTodayQueryHandler(Storage storage, LogService logService) : base(storage, logService)
        {
        }

        protected override async Task<CallListResult<GetAllExchangeRatesForTodayQueryResponse>> HandleCommandAsync(EmptyQuery query)
        {
            var list = await this.Storage.ExchangeRates
                .Where(x => x.ExchangeDate == DateTime.UtcNow.Date)
                .Select(x => new GetAllExchangeRatesForTodayQueryResponse
                {
                    FromCurrency = x.FromCurrencyCode,
                    ToCurrency = x.ToCurrencyCode,
                    Rate = x.Rate,
                    ExchangeDate = x.ExchangeDate,
                    OpenDayRate = x.OpenDayRate,
                    MinDayRate = x.MinDayRate,
                    MaxDayRate = x.MaxDayRate
                })
                .ToListAsync();

            return SuccessListResult(list);
        }
    }
}
