using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avto.DAL.Entities;

namespace Avto.BL.Services.Balance.Models
{
    public class BalanceProviderOrganizationModel
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string StripePublicKey { get; set; }
    }

    public static partial class MappingHelper
    {
        public static BalanceProviderOrganizationModel ToModel(this OrganizationEntity entity)
        {
            var result = new BalanceProviderOrganizationModel
            {
                OrganizationId = entity.Id,
                OrganizationName = entity.Name,
            };

            if (entity.ReceivingWays.Any())
            {
                result.StripePublicKey = entity.ReceivingWays.First().StripePublicConfig.PublicKey;
            }

            return result;
        }
    }
}
