using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.BL.Services.Organization.Handlers.Queries.Organization
{
    public class GetOrganizationQuery : Query
    {
        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }
    }
}
