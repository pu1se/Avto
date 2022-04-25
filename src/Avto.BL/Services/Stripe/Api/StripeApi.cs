using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avto.BL.Services.Stripe.Handlers.AddPaymentCard;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Entities;
using Stripe;

namespace Avto.BL.Services.Stripe.Api
{
    public static class StripeActionStatus
    {
        public const string Succeeded = "succeeded";
    }

    public class StripeApi : IStripeApi
    {
        public async Task<StripeCardModel> SaveDefaultCardAsync(
            ReceivingWayEntity receivingWay,
            List<SendingWayEntity> existSendingWays,
            AddEditSendingWayCommand model)
        {
            var stripeConnection = StripeConnection.For(receivingWay);
            
            Customer customer;

            if (!existSendingWays.Any())
            {
                customer = await stripeConnection.Customer.CreateAsync(new CustomerCreateOptions
                {
                    Email = model.SenderEmail,
                    Name = model.SenderName
                });
            }
            else
            {
                var defaultPaymentWay = existSendingWays
                    .OrderByDescending(x => x.LastUpdatedDateUtc)
                    .FirstOrDefault(x => x.IsDefault);
                if (defaultPaymentWay == null)
                {
                    defaultPaymentWay = existSendingWays.OrderByDescending(x => x.LastUpdatedDateUtc).First();
                }

                var defaultCard = defaultPaymentWay.Configuration.ToStripeCard();

                customer = new Customer
                {
                    Id = defaultCard.CustomerId
                };
            }

            var cardInfo = await stripeConnection.Card.CreateAsync(customer.Id, new CardCreateOptions
            {
                Source = new AnyOf<string, CardCreateNestedOptions>(model.Token),
            });

            await stripeConnection.Customer.UpdateAsync(customer.Id, new CustomerUpdateOptions
            {
                DefaultSource = cardInfo.Id,
                Name = model.SenderName
            });

            return new StripeCardModel
            {
                CardId = cardInfo.Id,
                CustomerId = cardInfo.CustomerId,
                Last4CardDigits = cardInfo.Last4,
                CardBrand = cardInfo.Brand,
                CardCountryCode = cardInfo.Country,
                CardType = cardInfo.Funding,
                ExpireMonth = cardInfo.ExpMonth,
                ExpireYear = cardInfo.ExpYear
            };
        }

        public async Task<string> SendPayment(StripeCardModel card, ReceivingWayEntity receivingWay, SendPaymentCommand paymentModel)
        {
            var stripeConnection = StripeConnection.For(receivingWay);
            var metadata = new Dictionary<string, string>();

            foreach (var (key, value) in paymentModel.ExternalMetadata)
            {
                var metadataValue = JsonConvert.SerializeObject(value);
                if (metadataValue.Length >= 500)
                {
                    metadataValue = metadataValue.Substring(0, 499);
                }
                metadata.Add(key, metadataValue);
            }

            var newCharge = new ChargeCreateOptions()
            {
                Amount = ConvertToStripeAmount(paymentModel),
                Currency = paymentModel.PaymentCurrency,
                Customer = card.CustomerId,
                Description = paymentModel.Description,
                Metadata = metadata
            };

            var charge = await stripeConnection.Charge.CreateAsync(newCharge);

            //todo: add additional check
            if (charge.Status != StripeActionStatus.Succeeded)
                throw new ThirdPartyApiException("Payment failed. Reason: " + charge.FailureMessage);

            return charge.StripeResponse.Content;
        }

        private static long ConvertToStripeAmount(SendPaymentCommand paymentModel)
        {
            const string NotSupportedCurrency = "STD";
            if (NotSupportedCurrency.Contains(paymentModel.PaymentCurrency))
            {
                throw new ThirdPartyApiException("We don't support this currency.");
            }

            const string currencyThatDoNotNeedMultiplier = "JPY,MGA,PYG,RWF,VND,VUV,XAF,XOF,XPF,BIF,CLP,DJF,GNF,KMF,KRW";
            var amount = paymentModel.PaymentAmount;

            if (!currencyThatDoNotNeedMultiplier.Contains(paymentModel.PaymentCurrency.ToUpper()))
            {
                amount = amount * 100;
            }

            return decimal.ToInt64(amount);
        }

        public async Task<bool> ValidateReceiverCredentials(StripeConfigForKeyVault config)
        {
            var stripeConnection = StripeConnection.For(config);

            await stripeConnection.Charge.ListAsync();

            return true;
        }

        public Task<string> PaymentRefund(ReceivingWayEntity receivingWay, PaymentEntity paymentEntity)
        {
            return PaymentRefund(receivingWay, paymentEntity.Stripe().Charge);
        }

        public async Task<string> PaymentRefund(ReceivingWayEntity receivingWay, StripeChargeTransactionModel charge)
        {
            var stripeConnection = StripeConnection.For(receivingWay);

            var refundOptions = new RefundCreateOptions
            {
                Charge = charge.Id
            };

            var refund = await stripeConnection.Refund.CreateAsync(refundOptions);

            if (refund.Status != StripeActionStatus.Succeeded)
                throw new ThirdPartyApiException("Payment failed. Reason: "+refund.FailureReason);

            return refund.StripeResponse.Content;
        }

        public async Task<string> AddPaymentCard(ReceivingWayEntity receivingWay, AddPaymentCardCommand model)
        {
            var stripeConnection = StripeConnection.For(receivingWay);

            var stripeTokenCreateOptions = new TokenCreateOptions
            {
                Card = new AnyOf<string, TokenCardOptions>(new TokenCardOptions
                {
                    Number = model.Number,
                    ExpMonth = model.ExpMonth,
                    ExpYear = model.ExpYear,
                    Cvc = model.Cvc,
                    Name = model.CardholderName,
                })
            };

            var stripeToken = await stripeConnection.Token.CreateAsync(stripeTokenCreateOptions);
            var token = stripeToken.Id;

            return token;
        }
    }
}
