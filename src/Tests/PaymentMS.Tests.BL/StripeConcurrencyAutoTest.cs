using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Handlers.AddPaymentCard;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class StripeConcurrencyAutoTest : BaseServiceTests<StripeService>
    {
        [TestMethod]
        public async Task StripeAccountConcurrencyTest()
        {
            var receiverCollection = new List<ReceiverInfo>();
            receiverCollection.Add(new ReceiverInfo(
                TestData.Stripe_PublicKey_1,
                TestData.Stripe_SecretKey_1,
                1,
                CurrencyType.EUR
            ));
            receiverCollection.Add(new ReceiverInfo(
                TestData.Stripe_PublicKey_2,
                TestData.Stripe_SecretKey_2,
                2,
                CurrencyType.USD
            ));
            receiverCollection.Add(new ReceiverInfo(
                TestData.Stripe_PublicKey_3,
                TestData.Stripe_SecretKey_3,
                30,
                CurrencyType.SEK
            ));
            receiverCollection.Add(new ReceiverInfo(
                TestData.Stripe_PublicKey_4,
                TestData.Stripe_SecretKey_4,
                400,
                CurrencyType.NOK
            ));


            foreach (var receiverItem in receiverCollection)
            {
                await CreateReceiverAndSenderOrganizations(receiverItem);
            }

            await Task.Delay(1000);

            var taskList = new List<Task>();

            for (var i = 0; i < 4; i++)
            {
                var task = Task.Run(async () =>
                {
                    var randomReceiver = GetRandomReceiver(receiverCollection);
                    await SuccessPayAndRefund(randomReceiver);
                });
                taskList.Add(task);
            }

            await Task.WhenAll(taskList);

            await DeleteTestDataFromDatabase(receiverCollection);
        }


        private async Task SuccessPayAndRefund(ReceiverInfo receiverInfo)
        {
            var sendPaymentModel = new SendPaymentCommand
            {
                PaymentCurrency = CurrencyType.EUR.ToString(),
                PaymentAmount = 5,
                ReceiverOrganizationId = receiverInfo.ReceiverOrganizationId,
                SenderOrganizationId = receiverInfo.SenderOrganizationId
            };
            var paymentCallResult = await Service.SendPayment(sendPaymentModel);
            Assert.IsNotNull(paymentCallResult);
            Assert.IsTrue(paymentCallResult.IsSuccess);

            var paymentRefundModel = paymentCallResult.Data.ToRefundModel();
            var refundResult = await Service.PaymentRefund(paymentRefundModel);
            Assert.IsTrue(refundResult != null);
            Assert.IsTrue(refundResult.IsSuccess);
        }



        private static readonly Random _random = new Random();
        private ReceiverInfo GetRandomReceiver(List<ReceiverInfo> receiverCollection)
        {
            var randomNumber = _random.Next(0, receiverCollection.Count - 1);
            return receiverCollection[randomNumber];
        }


        private async Task CreateReceiverAndSenderOrganizations(ReceiverInfo receiverInfo)
        {
            var receiverModel = new AddReceivingWayCommand()
            {
                PublicKey = receiverInfo.PublicKey,
                SecretKey = receiverInfo.SecretKey,
                ReceiverOrganizationId = receiverInfo.ReceiverOrganizationId,
                ReceiverOrganizationName = "unit tests receiver for concurrent payment " + receiverInfo.OrganizationIndex
            };
            var createReceivingWayResult = await Service.AddReceivingWay(receiverModel);
            Assert.IsTrue(createReceivingWayResult.IsSuccess);

            var card = new AddPaymentCardCommand
            {
                Number = "5555555555554444",
                ExpMonth = 5,
                ExpYear = 30,
                CardholderName = "Concurrent Unit test",
                Cvc = "123",
                ReceiverOrganizationId = receiverInfo.ReceiverOrganizationId
            };
            var cardTokenResult = await Service.AddPaymentCardToReceiver(card);
            Assert.IsTrue(cardTokenResult.IsSuccess);


            var senderModel = new AddEditSendingWayCommand
            {
                SenderName = "unit test sender for payment",
                ReceiverOrganizationId = receiverInfo.ReceiverOrganizationId,
                SenderEmail = "tmp@tmp.tmp",
                SenderOrganizationId = receiverInfo.SenderOrganizationId,
                Token = cardTokenResult.Data.Token,
                SenderOrganizationName = "unit tests sender for concurrent payment " + receiverInfo.OrganizationIndex,                
            };
            var callResult = await Service.AddEditSendingWay(senderModel);
            Assert.IsTrue(callResult.IsSuccess);

            var sendingWayCallResult = await Service.GetDefaultSendingWay(new GetDefaultSendingWayQuery
            {
                SenderOrganizationId = receiverInfo.SenderOrganizationId,
                ReceiverOrganizationId = receiverInfo.ReceiverOrganizationId
            });
            Assert.IsNotNull(sendingWayCallResult);
            Assert.IsTrue(sendingWayCallResult.IsSuccess);
            Assert.IsNotNull(sendingWayCallResult.Data);
            Assert.IsTrue(sendingWayCallResult.Data.SenderOrganizationId == receiverInfo.SenderOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.ReceiverOrganizationId == receiverInfo.ReceiverOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.Card != null);
            Assert.IsTrue(sendingWayCallResult.Data.Card.ExpireMonth == card.ExpMonth);
        }

        private async Task DeleteTestDataFromDatabase(IEnumerable<ReceiverInfo> receiverCollection)
        {
            var receiverOrganizationIdList = receiverCollection.Select(x => x.ReceiverOrganizationId);
            var paymentsForDeleting = await Storage.Payments
                .Where(e => receiverOrganizationIdList.Contains(e.SendingWay.ReceivingWay.OrganizationId))
                .ToListAsync();
            foreach (var item in paymentsForDeleting)
            {
                Storage.Payments.Delete(item);
            }

            var sendersForDeleting = await Storage.PaymentSendingWays
                .Where(e => receiverOrganizationIdList.Contains(e.ReceivingWay.OrganizationId))
                .ToListAsync();
            foreach (var item in sendersForDeleting)
            {
                Storage.PaymentSendingWays.Delete(item);
            }

            var receiversForDeleting = await Storage.StripeReceivingWays
                .Where(e => receiverOrganizationIdList.Contains(e.OrganizationId))
                .ToListAsync();
            foreach (var item in receiversForDeleting)
            {
                Storage.StripeReceivingWays.Delete(item);
            }

            var organizationsForDeleting = await Storage.Organizations
                .Where(e => receiverOrganizationIdList.Contains(e.Id) ||
                            receiverCollection.Select(x => x.SenderOrganizationId).Contains(e.Id))
                .ToListAsync();
            foreach (var item in organizationsForDeleting)
            {
                Storage.Organizations.Delete(item);
            }

            await Storage.SaveChangesAsync();
        }

        private class ReceiverInfo
        {
            public string PublicKey { get; }
            public string SecretKey { get; }
            public uint PaymentAmount { get; }
            public CurrencyType PaymentCurrency { get; }
            public Guid ReceiverOrganizationId { get; }
            public Guid SenderOrganizationId { get; }
            public uint OrganizationIndex { get; }

            public ReceiverInfo(
                string publicKey,
                string secretKey,
                uint amount,
                CurrencyType currency)
            {
                PublicKey = publicKey;
                SecretKey = secretKey;
                PaymentAmount = amount;
                PaymentCurrency = currency;
                ReceiverOrganizationId = Guid.NewGuid();
                SenderOrganizationId = Guid.NewGuid();
                OrganizationIndex = PaymentAmount;
            }
        }
    }
}
