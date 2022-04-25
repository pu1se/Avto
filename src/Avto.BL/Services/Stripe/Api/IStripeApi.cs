using System.Collections.Generic;
using System.Threading.Tasks;
using Avto.BL.Services.Stripe.Handlers.AddPaymentCard;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Entities;

namespace Avto.BL.Services.Stripe.Api
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