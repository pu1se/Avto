using System.Threading.Tasks;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs
{
    public class GetOrganizationExchangeConfigsQueryHandler 
        : QueryHandler<GetOrganizationExchangeConfigsQuery, CallListDataResult<OrganizationExchangeConfigResponse>>
    {
        public GetOrganizationExchangeConfigsQueryHandler(
            Storage storage, 
            LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<OrganizationExchangeConfigResponse>> HandleCommandAsync(GetOrganizationExchangeConfigsQuery query)
        {
            var configList = await Storage.CurrencyExchangeConfis.Where(
                    e =>
                    e.OrganizationId == query.OrganizationId
                )
                .Select(OrganizationExchangeConfigResponse.Map())
                .ToListAsync();

            return SuccessListResult(configList);
        }
    }
}
