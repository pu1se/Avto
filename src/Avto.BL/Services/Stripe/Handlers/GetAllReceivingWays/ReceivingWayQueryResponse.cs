using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;
using Avto.DAL.Entities;

namespace Avto.BL.Services.Stripe.Handlers.Queries.AllReceivingWays
{
    public class ReceivingWayQueryResponse
    {
        public Guid ReceiverOrganizationId { get; set; }
        public string ReceiverOrganizationName { get; set; }
        public string PublicKey { get; set; }

        public static Expression<Func<ReceivingWayEntity, ReceivingWayQueryResponse>> Map()
        {
            return entity => new ReceivingWayQueryResponse
            {
                ReceiverOrganizationId = entity.OrganizationId,
                ReceiverOrganizationName = entity.Organization.Name,
                PublicKey = entity.StripePrivateConfig.PublicKey
            };
        }
    }

    public static partial class MappingHelper{

        public static ReceivingWayQueryResponse ToReceiverModel(this ReceivingWayEntity entity)
        {
            return new ReceivingWayQueryResponse
            {
                ReceiverOrganizationId = entity.OrganizationId,
                ReceiverOrganizationName = entity.Organization.Name,
                PublicKey = entity.StripePublicConfig.PublicKey
            };
        }

    }
}

