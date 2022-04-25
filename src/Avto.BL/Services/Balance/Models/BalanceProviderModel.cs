using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Entities;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Balance.Models
{
    public class BalanceProviderModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool IsWireTransferIncomeEnabled { get; set; }
        public bool IsStripeCardIncomeEnabled { get; set; }
        public string StripePublicKey { get; set; }
        public string StripeSecretKey { get; set; }
        public string Currency { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public static partial class MappingHelper{

        public static BalanceProviderModel ToProviderModel(this BalanceProviderEntity entity)
        {
            var result = new BalanceProviderModel
            {
                Id = entity.Id,
                OrganizationId = entity.OrganizationId,
                OrganizationName = entity.Organization.Name,
                IsStripeCardIncomeEnabled = entity.IsStripeCardIncomeEnabled,
                IsWireTransferIncomeEnabled = entity.IsWireTransferIncomeEnabled,
                Currency = entity.Currency,
                CreditLimit = entity.CreditLimit,
            };

            if (result.IsStripeCardIncomeEnabled && entity.ReceivingWays.Any())
            {
                result.StripePublicKey = entity.ReceivingWays.First().StripePublicConfig.PublicKey;
            }

            return result;
        }
    }
}
