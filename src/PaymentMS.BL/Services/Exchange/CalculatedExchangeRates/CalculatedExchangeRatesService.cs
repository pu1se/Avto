using System;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates.Handlers.CalculateAllTodayExchangeRates;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedCommonExchangeRates;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedConfiguredByOrganizationExchangeRates;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.CalculatedExchangeRates
{
    public class CalculatedExchangeRatesService : BaseService
    {
        public CalculatedExchangeRatesService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallResult> RefreshAndCalculateAllTodayExchangeRates()
        {
            return GetHandler<CalculateAllTodayExchangeRatesCommandHandler>().HandleAsync(EmptyCommand.Value);
        }

        public Task<CallListDataResult<CalculatedCommonExchangeRateResponse>> GetCalculatedCommonExchangeRates(DateTime exchangeDate)
        {
            return GetHandler<GetCalculatedCommonExchangeRatesQueryHandler>().HandleAsync(new GetCalculatedCommonExchangeRatesQuery
            {
                ExchangeDate = exchangeDate
            });
        }

        public Task<CallListDataResult<CalculatedConfiguredByOrganizationExchangeRateResponse>> GetCalculatedConfiguredByOrganizationExchangeRates(Guid organizationId, DateTime exchangeDate)
        {
            return GetHandler<GetCalculatedConfiguredByOrganizationExchangeRatesQueryHandler>().HandleAsync(new GetCalculatedConfiguredByOrganizationExchangeRatesQuery
            {
                OrganizationId = organizationId,
                ExchangeDate = exchangeDate
            });
        }
    }
}
