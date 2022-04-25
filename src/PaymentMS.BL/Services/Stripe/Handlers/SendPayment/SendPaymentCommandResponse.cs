using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Stripe.ResponseModels
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
