using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedConfiguredByOrganizationExchangeRates
{
    public class GetCalculatedConfiguredByOrganizationExchangeRatesQueryHandler 
        : QueryHandler<GetCalculatedConfiguredByOrganizationExchangeRatesQuery, CallListDataResult<CalculatedConfiguredByOrganizationExchangeRateResponse>>
    {
        public GetCalculatedConfiguredByOrganizationExchangeRatesQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<CalculatedConfiguredByOrganizationExchangeRateResponse>> HandleCommandAsync(GetCalculatedConfiguredByOrganizationExchangeRatesQuery query)
        {
            var result = await this.Storage.CalculatedCurrencyExchangeRates
                .Where(e => e.ExchangeDate == query.ExchangeDate.Date && e.OrganizationId == query.OrganizationId)
                .Select(e => new CalculatedConfiguredByOrganizationExchangeRateResponse
                {
                    FromCurrencyCode = e.FromCurrencyCode,
                    ToCurrencyCode = e.ToCurrencyCode,
                    ExchangeDate = e.ExchangeDate,
                    ExchangeRate = e.ExchangeRate,
                    ProvidedByOrganizationId = e.OrganizationId.Value
                })
                .ToListAsync();

            return SuccessListResult(result);
        }
    }
}
