using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Stripe.Handlers.AddPaymentCard;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Stripe.Api
{
    public interface IStripeApi
    {
        Task<StripeCardModel> SaveDefaultCardAsync(ReceivingWayEntity receivingWay, List<SendingWayEntity> existSendingWays, AddEditSendingWayCommand model);
        Task<string> SendPayment(StripeCardModel card, ReceivingWayEntity receivingWay, SendPaymentCommand paymentModel);
        Task<bool> ValidateReceiverCredentials(StripeConfigForKeyVault config);
        Task<string> PaymentRefund(ReceivingWayEntity receivingWay, StripeChargeTransactionModel charge);
        Task<string> PaymentRefund(ReceivingWayEntity receivingWay, PaymentEntity paymentEntity);
        Task<string> AddPaymentCard(ReceivingWayEntity receivingWay, AddPaymentCardCommand model);
    }
}