using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedCommonExchangeRates
{
    public class GetCalculatedCommonExchangeRatesQueryHandler : QueryHandler<GetCalculatedCommonExchangeRatesQuery, CallListDataResult<CalculatedCommonExchangeRateResponse>>
    {
        public GetCalculatedCommonExchangeRatesQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<CalculatedCommonExchangeRateResponse>> HandleCommandAsync(GetCalculatedCommonExchangeRatesQuery query)
        {
            var result = await this.Storage.CalculatedCurrencyExchangeRates
                .Where(e => e.ExchangeDate == query.ExchangeDate.Date && e.OrganizationId == null)
                .Select(e => new CalculatedCommonExchangeRateResponse
                {
                    FromCurrencyCode = e.FromCurrencyCode,
                    ToCurrencyCode = e.ToCurrencyCode,
                    ExchangeDate = e.ExchangeDate,
                    ExchangeRate = e.ExchangeRate
                })
                .ToListAsync();

            return SuccessListResult(result);
        }
    }
}
