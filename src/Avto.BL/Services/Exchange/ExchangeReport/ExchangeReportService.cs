using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.ExchangePrediction
{
    public class ExchangeReportService : BaseService
    {
        public ExchangeReportService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public async Task<CallListDataResult<CurrencyExchangeInfoItemResponse>> GetCurrencyExchangeInfo(GetExchangeReportItemQuery query)
        {
            var fromDate = DateTime.UtcNow.AddDays(-1 * query.PeriodInDays);
            var list = await this.Storage.CurrencyExchangeRates
                .Where(
                    x => 
                    x.ExchangeProvider == ExchangeProviderType.CurrencyLayer
                    &&
                    x.FromCurrencyCode == query.FromCurrency
                    &&
                    x.ToCurrencyCode == query.ToCurrency
                    &&
                    x.ExchangeDate >= fromDate)
                .Select(x => new CurrencyExchangeInfoItemResponse
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

            return Result.SuccessList(list);
        }
    }
}
