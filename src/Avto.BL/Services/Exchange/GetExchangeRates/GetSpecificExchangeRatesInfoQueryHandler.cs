using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.Exchange.GetExchangeRates
{
    public class GetSpecificExchangeRatesInfoQueryHandler : QueryHandler<GetSpecificExchangeRatesInfoQuery, CallListResult<GetSpecificExchangeRatesInfoQueryResponse>>
    {
        public GetSpecificExchangeRatesInfoQueryHandler(Storage storage, LogService logService) : base(storage, logService)
        {
        }

        protected override async Task<CallListResult<GetSpecificExchangeRatesInfoQueryResponse>> HandleCommandAsync(GetSpecificExchangeRatesInfoQuery query)
        {
            LogService.WriteInfo("Get rates");
            var list = await this.Storage.ExchangeRates
                .Where(
                    x => 
                        x.FromCurrencyCode == query.FromCurrency
                        &&
                        x.ToCurrencyCode == query.ToCurrency
                        &&
                        x.ExchangeDate >= query.FromDate
                        &&
                        x.ExchangeDate <= query.ToDate
                )
                .Select(x => new GetSpecificExchangeRatesInfoQueryResponse
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
