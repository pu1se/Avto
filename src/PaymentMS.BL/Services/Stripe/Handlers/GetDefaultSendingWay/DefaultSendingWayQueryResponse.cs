using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay
{
    public class DefaultSendingWayQueryResponse
    {
        public Guid SenderOrganizationId { get; set; }
        public Guid ReceiverOrganizationId { get; set; }
        public Guid ReceivingWayId { get; set; }
        public StripeCardModel Card { get; set; }
        public Guid Id { get; set; }
    }

    public static partial class MappingHelper{

        public static DefaultSendingWayQueryResponse ToSendingWayModel(this SendingWayEntity entity)
        {
            return new DefaultSendingWayQueryResponse
            {
                Id = entity.Id,
                SenderOrganizationId = entity.OrganizationId,
                ReceiverOrganizationId = entity.ReceivingWay.OrganizationId,
                ReceivingWayId = entity.ReceivingWay.Id,
                Card = entity.Configuration.ToStripeCard()
            };
        }
    }
}