using System;
using System.Collections.Generic;
using System.Text;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Stripe.ResponseModels
{
    public class SendPaymentCommandResponse
    {
        public Guid ReceiverOrganizationId { get; set; }

        public Guid SenderOrganizationId { get; set; }

        public Guid PaymentId { get; set; }

        public string PaymentRefundUrl { get; set; }

        public PaymentStatusType PaymentStatus { get; set; }

        public PaymentRefundCommand ToRefundModel()
        {
            return new PaymentRefundCommand
            {
                PaymentId = PaymentId,
                ReceiverOrganizationId = ReceiverOrganizationId,
                SenderOrganizationId = SenderOrganizationId
            };
        }
    }
}
