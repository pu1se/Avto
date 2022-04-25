using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Balance.Models
{
    public class BalanceClientModel
    {
        public Guid Id { get;set; }

        public Guid ProviderOrganizationId { get; set; }

        public Guid ClientOrganizationId { get; set; }

        public string ClientOrganizationName { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public decimal CreditLimit { get; set; }

        public static Expression<Func<BalanceClientEntity, BalanceClientModel>> Map()
        {
            return entity => new BalanceClientModel
            {
                Id = entity.Id,
                ClientOrganizationId = entity.OrganizationId,
                ClientOrganizationName = entity.Organization.Name,
                ProviderOrganizationId = entity.BalanceProvider.OrganizationId,
                Currency = entity.BalanceProvider.Currency,
                CreditLimit = entity.BalanceProvider.CreditLimit,
                Amount = entity.Amount
            };
        }
    }
}
