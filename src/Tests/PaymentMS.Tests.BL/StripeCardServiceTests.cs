using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Api;
using PaymentMS.BL.Services.Stripe.Handlers.AddPaymentCard;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using PaymentMS.DAL;
using PaymentMS.DAL.Enums;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class StripeCardServiceTests : BaseServiceTests<StripeService>
    {        
        [TestMethod]
        public async Task SuccessCreateNewReceivingWayCreateNewSendingWayAndPayByItAndMakePaymentRefund()
        {
            var receivingOrganizationId = Guid.NewGuid();

            var receiverModel = new AddReceivingWayCommand()
            {
                PublicKey = TestData.Stripe_PublicKey_1,
                SecretKey = TestData.Stripe_SecretKey_1,
                ReceiverOrganizationId = receivingOrganizationId,
                ReceiverOrganizationName = "unit tests receiver for payment"
            };
            var createReceivingWayResult = await Service.AddReceivingWay(receiverModel);
            Assert.IsTrue(createReceivingWayResult.IsSuccess);
            var receivingWayId = createReceivingWayResult.Data.ReceivingWayId;

            var cardTokenResult = await Service.AddPaymentCardToReceiver(new AddPaymentCardCommand
            {
                Number = "4242424242424242",
                ExpMonth = 5,
                ExpYear = 30,
                CardholderName = "Unit test",
                Cvc = "123",
                ReceiverOrganizationId = receivingOrganizationId
            });
            Assert.IsTrue(cardTokenResult.IsSuccess);


            var sendingOrganizationId = Guid.NewGuid();

            var senderModel = new AddEditSendingWayCommand
            {                
                SenderName = "unit test sender for payment",
                ReceiverOrganizationId = receivingOrganizationId,
                SenderEmail = "tmp@tmp.tmp",
                SenderOrganizationId = sendingOrganizationId,
                Token = cardTokenResult.Data.Token,
                SenderOrganizationName = "unit test sender for payment"
            };
            var callResult = await Service.AddEditSendingWay(senderModel);
            Assert.IsTrue(callResult.IsSuccess);

            var sendingWayCallResult = await Service.GetDefaultSendingWay(new GetDefaultSendingWayQuery
            {
                SenderOrganizationId = sendingOrganizationId, 
                ReceiverOrganizationId = receivingOrganizationId
            });
            Assert.IsNotNull(sendingWayCallResult);
            Assert.IsTrue(sendingWayCallResult.IsSuccess);
            Assert.IsNotNull(sendingWayCallResult.Data);
            Assert.IsTrue(sendingWayCallResult.Data.SenderOrganizationId == sendingOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.ReceiverOrganizationId == receivingOrganizationId);

            var sendPaymentModel = new SendPaymentCommand
            {
                PaymentCurrency = CurrencyType.EUR.ToString(),
                PaymentAmount = 5,
                ReceiverOrganizationId = receivingOrganizationId,
                SenderOrganizationId = sendingOrganizationId
            };
            var paymentCallResult = await Service.SendPayment(sendPaymentModel);
            Assert.IsNotNull(paymentCallResult);
            Assert.IsTrue(paymentCallResult.IsSuccess);

            var payment = await Storage.Payments.GetAsync(e => e.SendingWayId == sendingWayCallResult.Data.Id);
            Assert.IsTrue(payment != null);
            Assert.IsTrue(sendPaymentModel.PaymentAmount == payment.PaymentAmount);
            Assert.IsTrue(sendPaymentModel.PaymentCurrency == payment.PaymentCurrency);
            Assert.IsTrue(sendingWayCallResult.Data.Id == payment.SendingWayId);
            Assert.IsTrue(payment.Status == PaymentStatusType.Success);

            var paymentRefundModel = paymentCallResult.Data.ToRefundModel();
            var refundResult = await Service.PaymentRefund(paymentRefundModel);
            Assert.IsTrue(refundResult != null);
            Assert.IsTrue(refundResult.IsSuccess);

            CleanStorageCache();
            var refundPayment = await Storage.Payments
                .GetAsync(e => e.Id == paymentRefundModel.PaymentId);
            Assert.IsTrue(refundPayment != null);
            Assert.IsTrue(sendPaymentModel.PaymentAmount == refundPayment.PaymentAmount);
            Assert.IsTrue(sendPaymentModel.PaymentCurrency == refundPayment.PaymentCurrency);
            Assert.IsTrue(sendingWayCallResult.Data.Id == refundPayment.SendingWayId);
            Assert.IsTrue(refundPayment.Status == PaymentStatusType.Refund);

            var organization = await Storage.Organizations.GetAsync(e=>e.Id == sendingOrganizationId);
            Assert.IsNotNull(organization);
            Assert.IsTrue(organization.Id == sendingOrganizationId);

            await Storage.Payments.DeleteAsync(payment.Id);
            await Storage.PaymentSendingWays.DeleteAsync(sendingWayCallResult.Data.Id);
            await Storage.Organizations.DeleteAsync(sendingOrganizationId);
            await Storage.StripeReceivingWays.DeleteAsync(receivingWayId);
            await Storage.Organizations.DeleteAsync(receivingOrganizationId);
        }

        [TestMethod]
        public async Task SuccessCreateNewReceivingWay()
        {
            var organizationId = Guid.NewGuid();
            var receivingWayId = Guid.NewGuid();

            var model = new AddReceivingWayCommand
            {
                ReceiverOrganizationId = organizationId,
                ReceiverOrganizationName = $"Unit tests for organization {organizationId}",
                PublicKey = TestData.Stripe_PublicKey_1,
                SecretKey = TestData.Stripe_SecretKey_1,
                ReceivingWayId = receivingWayId
            };
            var callResult = await Service.AddReceivingWay(model);
            Assert.IsTrue(callResult.IsSuccess);

            var receivingWay = await Storage.StripeReceivingWays.GetAsync(e=>e.Id == receivingWayId);
            Assert.IsNotNull(receivingWay);
            Assert.IsTrue(receivingWay.Id == receivingWayId);
            Assert.IsTrue(receivingWay.StripePublicConfig.PublicKey == TestData.Stripe_PublicKey_1);
            Assert.IsTrue(receivingWay.StripePrivateConfig.PublicKey == TestData.Stripe_PublicKey_1);
            Assert.IsTrue(receivingWay.StripePrivateConfig.SecretKey == TestData.Stripe_SecretKey_1);
            Assert.IsTrue(receivingWay.PaymentMethod == PaymentMethodType.StripeCard);

            var organization = await Storage.Organizations.GetAsync(e=>e.Id==organizationId);
            Assert.IsNotNull(organization);
            Assert.IsTrue(organization.Id == organizationId);

            Storage.StripeReceivingWays.Delete(receivingWay);
            Storage.Organizations.Delete(organization);
            await Storage.SaveChangesAsync();
        }

        [TestMethod]
        public async Task FailAddExistingReceivingWay()
        {
            var model = new AddReceivingWayCommand
            {
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId,
                ReceiverOrganizationName = $"Unit tests for FailAddExistingReceivingWay",
                PublicKey = "another key",
                SecretKey = "another key",
                ReceivingWayId = TestData.PaymentReceiverOrganizationId
            };
            var callResult = await Service.AddReceivingWay(model);
            Assert.IsTrue(callResult.IsSuccess == false);
            Assert.IsTrue(callResult.ErrorType == ErrorType.ValidationError400);
        }

        [TestMethod]
        public async Task AddAndChangeDefaultPaymentCardAndPayWithIt()
        {
            if (!TestIsRunningOnLocalPC)
            {
                return;
            }

            var cardTokenResult = await Service.AddPaymentCardToReceiver(new AddPaymentCardCommand
            {
                Number = "4242424242424242",
                ExpMonth = 5,
                ExpYear = 30,
                CardholderName = "Unit test",
                Cvc = "123",
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId
            });
            Assert.IsTrue(cardTokenResult.IsSuccess);


            var senderModel = new AddEditSendingWayCommand
            {                
                SenderName = "unit test sender default card",
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId,
                SenderEmail = "tmp@tmp.tmp",
                SenderOrganizationId = TestData.PaymentSenderOrganizationId,
                Token = cardTokenResult.Data.Token,
                SenderOrganizationName = "unit test sender for payment"
            };
            var callResult = await Service.AddEditSendingWay(senderModel);
            Assert.IsTrue(callResult.IsSuccess);

            var sendingWayCallResult = await Service.GetDefaultSendingWay(new GetDefaultSendingWayQuery
            {
                SenderOrganizationId = TestData.PaymentSenderOrganizationId, 
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId
            });
            Assert.IsNotNull(sendingWayCallResult);
            Assert.IsTrue(sendingWayCallResult.IsSuccess);
            Assert.IsNotNull(sendingWayCallResult.Data);
            Assert.IsTrue(sendingWayCallResult.Data.SenderOrganizationId == TestData.PaymentSenderOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.ReceiverOrganizationId == TestData.PaymentReceiverOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.Card.Last4CardDigits == "4242");
            Assert.IsTrue(sendingWayCallResult.Data.Card.ExpireMonth == 5);
            Assert.IsTrue(sendingWayCallResult.Data.Card.ExpireYear == 2030);



            cardTokenResult = await Service.AddPaymentCardToReceiver(new AddPaymentCardCommand
            {
                Number = "5555555555554444",
                ExpMonth = 10,
                ExpYear = 28,
                CardholderName = "Unit test",
                Cvc = "123",
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId
            });
            Assert.IsTrue(cardTokenResult.IsSuccess);


            senderModel = new AddEditSendingWayCommand
            {                
                SenderName = "unit test sender default card",
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId,
                SenderEmail = "tmp@tmp.tmp",
                SenderOrganizationId = TestData.PaymentSenderOrganizationId,
                Token = cardTokenResult.Data.Token,
                SenderOrganizationName = "unit test sender for payment"
            };
            callResult = await Service.AddEditSendingWay(senderModel);
            Assert.IsTrue(callResult.IsSuccess);

            sendingWayCallResult = await Service.GetDefaultSendingWay(new GetDefaultSendingWayQuery
            {
                SenderOrganizationId = TestData.PaymentSenderOrganizationId, 
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId
            });
            Assert.IsNotNull(sendingWayCallResult);
            Assert.IsTrue(sendingWayCallResult.IsSuccess);
            Assert.IsNotNull(sendingWayCallResult.Data);
            Assert.IsTrue(sendingWayCallResult.Data.SenderOrganizationId == TestData.PaymentSenderOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.ReceiverOrganizationId == TestData.PaymentReceiverOrganizationId);
            Assert.IsTrue(sendingWayCallResult.Data.Card.Last4CardDigits == "4444");
            Assert.IsTrue(sendingWayCallResult.Data.Card.ExpireMonth == 10);
            Assert.IsTrue(sendingWayCallResult.Data.Card.ExpireYear == 2028);



            var sendPaymentModel = new SendPaymentCommand
            {
                PaymentCurrency = CurrencyType.EUR.ToString(),
                PaymentAmount = 5,
                ReceiverOrganizationId = TestData.PaymentReceiverOrganizationId,
                SenderOrganizationId = TestData.PaymentSenderOrganizationId
            };
            var paymentCallResult = await Service.SendPayment(sendPaymentModel);
            Assert.IsNotNull(paymentCallResult);
            Assert.IsTrue(paymentCallResult.IsSuccess);

            var payment = await Storage.Payments
                .Include(e => e.SendingWay)
                .Include(e => e.SendingWay.ReceivingWay)
                .GetAsync(
                e => 
                e.SendingWayId == sendingWayCallResult.Data.Id &&
                e.Id == paymentCallResult.Data.PaymentId);

            Assert.IsTrue(payment != null);
            Assert.IsTrue(sendPaymentModel.PaymentAmount == payment.PaymentAmount);
            Assert.IsTrue(sendPaymentModel.PaymentCurrency == payment.PaymentCurrency);
            Assert.IsTrue(sendingWayCallResult.Data.Id == payment.SendingWayId);
            Assert.IsTrue(payment.Status == PaymentStatusType.Success);
            Assert.IsTrue(paymentCallResult.Data.PaymentId == payment.Id);
            Assert.IsTrue(paymentCallResult.Data.SenderOrganizationId == payment.SendingWay.OrganizationId);
            Assert.IsTrue(paymentCallResult.Data.ReceiverOrganizationId == payment.SendingWay.ReceivingWay.OrganizationId);
        }

    }
}
