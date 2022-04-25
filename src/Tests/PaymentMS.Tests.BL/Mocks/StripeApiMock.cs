using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Api;
using PaymentMS.BL.Services.Stripe.Handlers.AddPaymentCard;
using PaymentMS.DAL.Entities;

namespace PaymentMS.Tests.BL.Mocks
{
    public class StripeApiMock : IStripeApi
    {
        private static Dictionary<string, AddPaymentCardCommand> _cardDict = new Dictionary<string, AddPaymentCardCommand>();

        public Task<StripeCardModel> SaveDefaultCardAsync(ReceivingWayEntity receivingWay, List<SendingWayEntity> existSendingWays,
            AddEditSendingWayCommand model)
        {
            if (!_cardDict.ContainsKey(model.Token))
            {
                throw new ThirdPartyApiException("Card token was not found.");
            }

            var card = _cardDict[model.Token];
            return Task.FromResult(new StripeCardModel
            {
                ExpireYear = card.ExpYear,
                ExpireMonth = card.ExpMonth,
                CardBrand = "MasterCard",
            });
        }

        public Task<string> SendPayment(StripeCardModel card, ReceivingWayEntity receivingWay,
            SendPaymentCommand paymentModel)
        {
            return Task.FromResult("");
        }
        
        public Task<bool> ValidateReceiverCredentials(StripeConfigForKeyVault config)
        {
            return Task.FromResult(true);
        }

        public Task<string> PaymentRefund(ReceivingWayEntity receivingWay, StripeChargeTransactionModel charge)
        {
            return Task.FromResult("");
        }

        public Task<string> PaymentRefund(ReceivingWayEntity receivingWay, PaymentEntity paymentEntity)
        {
            return Task.FromResult("");
        }

        public Task<string> AddPaymentCard(ReceivingWayEntity receivingWayEntity, AddPaymentCardCommand model)
        {
            var token = Guid.NewGuid().ToString();
            _cardDict.Add(token, model);
            return Task.FromResult(token);
        }
    }
}
