using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Avto.BL.Services.Balance;
using Avto.BL.Services.Balance.Models;
using Avto.BL.Services.Organization;
using Avto.BL.Services.Organization.Handlers.Commands.AddOrganization;
using Avto.BL.Services.Stripe;
using Avto.BL.Services.Stripe.Handlers;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL;
using Avto.DAL.Enums;
using Avto.Tests.BL.Base;

namespace Avto.Tests.BL
{
    [TestClass]
    public class BalanceServiceTests : BaseServiceTests<BalanceService>
    {
        [TestMethod]
        public async Task SuccessCreateBalanceProviderWithWireTransferPayments()
        {
            var organizationId = Guid.NewGuid();
            var creatingProvider = new AddEditBalanceProviderCommand
            {
                IsStripeCardIncomeEnabled = false,
                IsWireTransferIncomeEnabled = true,
                ProviderOrganizationId = organizationId,
                ProviderOrganizationName = "unit test provider1",
                Currency = CurrencyType.EUR.ToString(),
                CreditLimit = 100,
            };

            var creationResult = await Service.AddEditProvider(creatingProvider);
            Assert.IsTrue(creationResult.IsSuccess);

            var getProviderResult = await Service.GetProvider(organizationId);
            Assert.IsTrue(getProviderResult.IsSuccess);
            Assert.IsTrue(getProviderResult.Data != null);

            var createdProvider = getProviderResult.Data;
            Assert.IsTrue(creatingProvider.ProviderOrganizationId == createdProvider.OrganizationId);
            Assert.IsTrue(creatingProvider.ProviderOrganizationName == createdProvider.OrganizationName);
            Assert.IsTrue(creatingProvider.IsStripeCardIncomeEnabled == createdProvider.IsStripeCardIncomeEnabled);
            Assert.IsTrue(creatingProvider.IsWireTransferIncomeEnabled == createdProvider.IsWireTransferIncomeEnabled);
            Assert.IsTrue(creatingProvider.Currency == createdProvider.Currency);
            Assert.IsTrue(creatingProvider.CreditLimit == createdProvider.CreditLimit);

            await Storage.BalanceProviders.DeleteAsync(createdProvider.Id);
            await Storage.Organizations.DeleteAsync(organizationId);
        }

        [TestMethod]
        public async Task FailCreateBalanceProviderWithoutAnyPayment()
        {
            var organizationId = Guid.NewGuid();
            var creatingProvider = new AddEditBalanceProviderCommand
            {
                IsStripeCardIncomeEnabled = false,
                IsWireTransferIncomeEnabled = false,
                ProviderOrganizationId = organizationId,
                ProviderOrganizationName = "unit test provider2",
                Currency = CurrencyType.EUR.ToString(),
                CreditLimit = 100,
            };

            var creationResult = await Service.AddEditProvider(creatingProvider);
            Assert.IsTrue(creationResult.IsSuccess == false);
            Assert.IsTrue(creationResult.ErrorType == ErrorType.ValidationError400);
        }

        [TestMethod]
        public async Task SuccessCreateBalanceProviderWithStripePayments()
        {
            var organizationId = Guid.NewGuid();
            var creatingProvider = new AddEditBalanceProviderCommand
            {
                IsStripeCardIncomeEnabled = true,
                IsWireTransferIncomeEnabled = false,
                ProviderOrganizationId = organizationId,
                ProviderOrganizationName = "unit test provider3",
                Currency = CurrencyType.EUR.ToString(),
                CreditLimit = 100,
            };

            var stripeService = Resolve<StripeService>();
            var createStripeReceivingWayResult = await stripeService.AddReceivingWay(new AddReceivingWayCommand
            {
                ReceiverOrganizationId = creatingProvider.ProviderOrganizationId,
                ReceiverOrganizationName = creatingProvider.ProviderOrganizationName,
                PublicKey = TestData.Stripe_PublicKey_1,
                SecretKey = TestData.Stripe_SecretKey_1,
            });
            Assert.IsTrue(createStripeReceivingWayResult.IsSuccess);
            Assert.IsTrue(createStripeReceivingWayResult.ErrorType == ErrorType.NotError);


            var creationResult = await Service.AddEditProvider(creatingProvider);
            Assert.IsTrue(creationResult.IsSuccess);

            var createResult = await Service.GetProvider(organizationId);
            Assert.IsTrue(createResult.IsSuccess);
            Assert.IsTrue(createResult.Data != null);

            var createdProvider = createResult.Data;
            Assert.IsTrue(creatingProvider.ProviderOrganizationId == createdProvider.OrganizationId);
            Assert.IsTrue(creatingProvider.ProviderOrganizationName == createdProvider.OrganizationName);
            Assert.IsTrue(creatingProvider.IsStripeCardIncomeEnabled == createdProvider.IsStripeCardIncomeEnabled);
            Assert.IsTrue(creatingProvider.IsWireTransferIncomeEnabled == createdProvider.IsWireTransferIncomeEnabled);
            Assert.IsTrue(creatingProvider.Currency == createdProvider.Currency);
            Assert.IsTrue(creatingProvider.CreditLimit == createdProvider.CreditLimit);

            var organization = await Storage.Organizations
                .Include(e=>e.ReceivingWays)
                .Include(e=>e.BalanceProviders)
                .GetAsync(e=>e.Id==organizationId);
            await Storage.StripeReceivingWays.DeleteAsync(organization.ReceivingWays.First().Id);
            await Storage.BalanceProviders.DeleteAsync(organization.BalanceProviders.First().Id);
            await Storage.Organizations.DeleteAsync(organization.Id);
        }

        [TestMethod]
        public async Task SuccessUpdateBalanceProviderWithStripePayments()
        {
            var organizationId = TestData.PaymentReceiverOrganizationId;
            var updatingProvider = new AddEditBalanceProviderCommand
            {
                IsStripeCardIncomeEnabled = true,
                IsWireTransferIncomeEnabled = true,
                ProviderOrganizationId = organizationId,
                ProviderOrganizationName = "create balance provider unit test",
                Currency = CurrencyType.NOK.ToString(),
                CreditLimit = 1000,
            };
            var updateResult = await Service.AddEditProvider(updatingProvider);
            Assert.IsTrue(updateResult.IsSuccess);

            var getUpdatedResult = await Service.GetProvider(organizationId);
            Assert.IsTrue(getUpdatedResult.IsSuccess);
            Assert.IsTrue(getUpdatedResult.Data != null);

            var updatedProvider = getUpdatedResult.Data;
            Assert.IsTrue(updatingProvider.ProviderOrganizationId == updatedProvider.OrganizationId);
            Assert.IsTrue(updatingProvider.IsStripeCardIncomeEnabled == updatedProvider.IsStripeCardIncomeEnabled);
            Assert.IsTrue(updatingProvider.IsWireTransferIncomeEnabled == updatedProvider.IsWireTransferIncomeEnabled);
            Assert.IsTrue(updatingProvider.Currency == updatedProvider.Currency);
            Assert.IsTrue(updatingProvider.CreditLimit == updatedProvider.CreditLimit);


            updatingProvider = new AddEditBalanceProviderCommand
            {
                IsStripeCardIncomeEnabled = false,
                IsWireTransferIncomeEnabled = true,
                ProviderOrganizationId = organizationId,
                ProviderOrganizationName = "Default Balance provider",
                Currency = CurrencyType.EUR.ToString(),
                CreditLimit = 5000,
            };
            updateResult = await Service.AddEditProvider(updatingProvider);
            Assert.IsTrue(updateResult.IsSuccess);
        }

        [TestMethod]
        public async Task SuccessCreateBalanceClient()
        {
            var clientOrganizationId = Guid.NewGuid();
            var createOrganizationResult = await Resolve<OrganizationService>().AddOrganization(new AddOrganizationCommand
            {
                OrganizationId = clientOrganizationId,
                OrganizationName = "SuccessCreateBalanceClient"
            });
            Assert.IsTrue(createOrganizationResult.IsSuccess);

            var creatingClient = new AddBalanceClientCommand
            {
                ProviderOrganizationId = TestData.BalanceProviderOrganizationId,
                ClientOrganizationId = clientOrganizationId
            };

            var creationResult = await Service.AddClient(creatingClient);
            Assert.IsTrue(creationResult.IsSuccess);

            var getCreatedClientResult = await Service.GetClient(creatingClient.ProviderOrganizationId, creatingClient.ClientOrganizationId);
            Assert.IsTrue(getCreatedClientResult.IsSuccess);

            var createdClient = getCreatedClientResult.Data;
            Assert.IsTrue(creatingClient.ClientOrganizationId == createdClient.ClientOrganizationId);
            Assert.IsTrue(creatingClient.ProviderOrganizationId == createdClient.ProviderOrganizationId);

            var getClientList = await Service.GetClients(creatingClient.ProviderOrganizationId);
            Assert.IsTrue(getClientList.IsSuccess);
            Assert.IsTrue(getClientList.Data.Any(x => x.ClientOrganizationId == creatingClient.ClientOrganizationId &&
                                                      x.ProviderOrganizationId == creatingClient.ProviderOrganizationId));

            await Storage.BalanceClients.DeleteAsync(createdClient.Id);
            await Storage.Organizations.DeleteAsync(creatingClient.ClientOrganizationId);
        }

        [TestMethod]
        public async Task SuccessIncreaseBalanceByWireTransfer()
        {
            var clientResult = await Service.GetClient(TestData.BalanceProviderOrganizationId,
                TestData.BalanceClientOrganizationId);
            Assert.IsTrue(clientResult.IsSuccess);
            var client = clientResult.Data;

            var increaseModel = new IncreaseBalaceByWireTransferCommand
            {
                AddAmount = 100,
                ClientOrganizationId = client.ClientOrganizationId,
                ProviderOrganizationId = client.ProviderOrganizationId
            };
            var increaseResult = await Service.IncreaseByWireTransfer(increaseModel);
            Assert.IsTrue(increaseResult.IsSuccess);

            clientResult = await Resolve<BalanceService>().GetClient(TestData.BalanceProviderOrganizationId,
                TestData.BalanceClientOrganizationId);
            Assert.IsTrue(clientResult.IsSuccess);
            var increasedBalanceClient = clientResult.Data;

            Assert.IsTrue(client.Amount+increaseModel.AddAmount == increasedBalanceClient.Amount);
            Assert.IsTrue(client.Currency == increasedBalanceClient.Currency);
            Assert.IsTrue(client.ClientOrganizationId == increasedBalanceClient.ClientOrganizationId);
            Assert.IsTrue(client.ProviderOrganizationId == increasedBalanceClient.ProviderOrganizationId);
        }

        [TestMethod]
        public async Task SuccessPayByBalance()
        {
            var clientResult = await Service.GetClient(
                TestData.BalanceProviderOrganizationId,
                TestData.BalanceClientOrganizationId);
            Assert.IsTrue(clientResult.IsSuccess);
            var client = clientResult.Data;

            var payModel = new SendBalancePaymentCommand
            {
                PaymentAmount = 100,
                ClientOrganizationId = client.ClientOrganizationId,
                ProviderOrganizationId = client.ProviderOrganizationId,
                ExternalId = Guid.NewGuid(),
                Description = "some description",
            };
            var paymentResult = await Service.SendPayment(payModel);
            Assert.IsTrue(paymentResult.IsSuccess);

            clientResult = await Resolve<BalanceService>().GetClient(
                TestData.BalanceProviderOrganizationId,
                TestData.BalanceClientOrganizationId);
            Assert.IsTrue(clientResult.IsSuccess);
            var clientAfterPayment = clientResult.Data;

            Assert.IsTrue(client.Amount-payModel.PaymentAmount == clientAfterPayment.Amount);
            Assert.IsTrue(client.Currency == clientAfterPayment.Currency);
            Assert.IsTrue(client.ClientOrganizationId == clientAfterPayment.ClientOrganizationId);
            Assert.IsTrue(client.ProviderOrganizationId == clientAfterPayment.ProviderOrganizationId);
        }

        [TestMethod]
        public async Task SuccessGetPossibleBalanceProviders()
        {
            var possibleProviders = await Service.GetPossibleProviderOrganzations();
            Assert.IsTrue(possibleProviders.IsSuccess);
            Assert.IsTrue(possibleProviders.Data != null);
            Assert.IsTrue(possibleProviders.Data.Any());
            Assert.IsTrue(possibleProviders.Data.First().OrganizationName.IsNullOrEmpty() == false);
            Assert.IsTrue(possibleProviders.Data.First().OrganizationId != Guid.Empty);

        }
    }
}
