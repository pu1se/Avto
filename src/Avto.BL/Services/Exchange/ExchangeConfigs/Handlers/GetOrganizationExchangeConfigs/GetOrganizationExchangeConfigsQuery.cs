using System;

namespace Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs
{
    public class GetOrganizationExchangeConfigsQuery : Query
    {
        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }
    }
}
