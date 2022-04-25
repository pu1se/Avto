using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Stripe.Handlers.Queries.Payments
{
    public class PaymentQueryResponse
    {
        public Guid PaymentId { get; set; }
        public PaymentStatusType PaymentStatus { get; set; }
        public string SentFromOrganizationName { get; set; }
        public string ReceivedByOrganizationName { get; set; }
        public Guid ExternalId { get; set; }
        public string Description { get; set; }
        public string ExternalMetadata { get; set; }
        public StripeCardModel CardInfo { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentCurrency { get; set; }
        public DateTime CreatedDate { get; set; }


        public static Expression<Func<PaymentEntity, PaymentQueryResponse>> Map()
        {
            return entity => new PaymentQueryResponse
            {
                PaymentId = entity.Id,
                PaymentStatus = entity.Status,
                SentFromOrganizationName = entity.SendingWay.Organization.Name,
                ReceivedByOrganizationName = entity.SendingWay.ReceivingWay.Organization.Name,
                CardInfo = entity.SendingWay.Configuration.ToStripeCard(),
                PaymentAmount = entity.PaymentAmount,
                PaymentCurrency = entity.PaymentCurrency,
                CreatedDate = entity.CreatedDateUtc,
                Description = entity.Description,
                ExternalId = entity.ExternalId,
                ExternalMetadata = entity.ExternalMetadata
            };
        }
    }

}
