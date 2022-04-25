using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Organization.Handlers.Queries.Organization
{
    public class GetOrganizationQueryHandler 
        : QueryHandler<GetOrganizationQuery, CallDataResult<GetOrganizationQueryResponse>>
    {
        public GetOrganizationQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallDataResult<GetOrganizationQueryResponse>> HandleCommandAsync(GetOrganizationQuery query)
        {
            var organization = await Storage.Organizations
                .Where(e => e.Id == query.OrganizationId)
                .Select(GetOrganizationQueryResponse.Map())
                .GetAsync();
            if (organization == null)
            {
                return NotFoundResult<GetOrganizationQueryResponse>("Organization was not found.");
            }
            return SuccessResult(organization);
        }
    }
}
