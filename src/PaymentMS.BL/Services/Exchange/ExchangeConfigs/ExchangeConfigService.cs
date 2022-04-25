using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.AddEditExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.DeleteExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeConfigs
{
    public class ExchangeConfigService : BaseService
    {
        public ExchangeConfigService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallListDataResult<OrganizationExchangeConfigResponse>> GetOrganizationExchangeConfigs(Guid organizationId)
        {
            var query = new GetOrganizationExchangeConfigsQuery{OrganizationId = organizationId};
            return GetHandler<GetOrganizationExchangeConfigsQueryHandler>().HandleAsync(query);
        }

        public Task<CallResult> AddEditOrganizationExchangeConfig(AddEditExchangeConfigCommand command)
        {
            return GetHandler<AddEditExchangeConfigCommandHandler>().HandleAsync(command);
        }

        public Task<CallResult> DeleteOrganizationExchangeConfig(DeleteExchangeConfigCommand command)
        {
            return GetHandler<DeleteExchangeConfigCommandHandler>().HandleAsync(command);
        }

        public Task<CallListDataResult<RateResponse>> GetTodayRatesConfiguredByOrganization(Guid organizationId)
        {
            var query = new GetRatesQuery{OrganizationId = organizationId};
            return GetHandler<GetRatesQueryHandler>().HandleAsync(query);
        }
    }
}
