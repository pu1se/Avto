using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Organization.Handlers.Queries.Organization
{
    public class GetOrganizationQueryResponse
    {
        public Guid OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public static Expression<Func<OrganizationEntity, GetOrganizationQueryResponse>> Map()
        {
            return entity => new GetOrganizationQueryResponse
            {
                OrganizationId = entity.Id,
                OrganizationName = entity.Name
            };
        }
    }
}
