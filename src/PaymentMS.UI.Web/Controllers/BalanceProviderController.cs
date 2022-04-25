using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Enums;
using PaymentMS.UI.Web.ApiRequests;
using PaymentMS.UI.Web.ViewModels;
using PaymentMS.UI.Web.ViewModels.Balance;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentMS.UI.Web.Controllers
{
    public class BalanceProviderController : Controller
    {
        private BalanceApiClient BalanceApi { get; }
        private StripeCardApiClient StripeApi { get; }

        public BalanceProviderController(BalanceApiClient balanceApi, StripeCardApiClient stripeApi)
        {
            BalanceApi = balanceApi;
            StripeApi = stripeApi;
        }

        
        public async Task<IActionResult> AddBalanceProvider()
        {
            var viewModel = new BalanceProviderViewModel();

            await InitReceiverList(viewModel);
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBalanceProvider(BalanceProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await InitReceiverList(viewModel);
                return View(viewModel);
            }

            if (viewModel.IsStripeCardIncomeEnabled && !viewModel.HasStipeReceiver)
            {
                var stripeReceivingWay = new AddStripeReceiverViewModel
                {
                    PublicKey = viewModel.StripePublicKey,
                    SecretKey = viewModel.StripeSecretKey,
                    ReceiverOrganizationId = viewModel.ProviderOrganizationId,
                    ReceiverOrganizationName = viewModel.ProviderOrganizationName
                };
                var recievingCreationResult = await StripeApi.AddReceivingWayAsync(stripeReceivingWay);
                if (!recievingCreationResult.IsSuccess)
                {
                    viewModel.ErrorMessage = recievingCreationResult.ErrorMessage;
                    await InitReceiverList(viewModel);
                    return View(viewModel);
                }
            }

            var callResult = await BalanceApi.AddProvider(viewModel);
            if (!callResult.IsSuccess)
            {
                viewModel.ErrorMessage = callResult.ErrorMessage;
                await InitReceiverList(viewModel);
                return View(viewModel);
            }

            await InitReceiverList(viewModel);
            viewModel.IsSuccess = true;
            return View(viewModel);
        }

        private async Task InitReceiverList(BalanceProviderViewModel viewModel)
        {
            var callResultWithOrganizations = await BalanceApi.GetPossibleProviderOrganizationsAsync();
            var receiverList = callResultWithOrganizations.Data;
            receiverList.Insert(0, new BalanceProviderOrganizationModel
            {
                OrganizationId = Guid.Empty,
                OrganizationName = "Add new",
                StripePublicKey = ""
            });


            viewModel.ReceiverList = receiverList;
            viewModel.SelectedReceiverId = receiverList.Count > 1
                ? receiverList[1].OrganizationId
                : receiverList[0].OrganizationId;
        }

        [HttpGet]
        public async Task<IActionResult> BalanceProvider(Guid receivingWayId)
        {
            var viewModel = new BalanceProviderViewModel
            {
                CurrencyList = EnumHelper.ToDictionary<CurrencyType>().Select(x => new SelectListItem
                {
                    Value = x.Key.ToLower(),
                    Text = x.Value
                }),
                Currency = CurrencyType.NOK.ToString(),
                ProviderOrganizationId = Guid.NewGuid(),
            };

            if (receivingWayId == Guid.Empty)
            {
                return View(viewModel);
            }
            
            var callResult = await BalanceApi.GetProvider(receivingWayId);

            var callResultWithOrganizations = await BalanceApi.GetPossibleProviderOrganizationsAsync();
            var receiverList = callResultWithOrganizations.Data;
            var receiver = receiverList.First(x => x.OrganizationId == receivingWayId);
            viewModel.ProviderOrganizationName = receiver.OrganizationName;
            viewModel.StripePublicKey = receiver.StripePublicKey;
            viewModel.HasStipeReceiver = !receiver.StripePublicKey.IsNullOrEmpty();
            viewModel.IsBalanceProvider = callResult.IsSuccess;

            if (!callResult.IsSuccess)
            {                
                viewModel.ProviderOrganizationId = receivingWayId;
                viewModel.ErrorMessage = callResult.ErrorMessage;
                return View(viewModel);
            }

            var model = callResult.Data;

            viewModel.ProviderOrganizationId = model.OrganizationId;
            viewModel.ProviderOrganizationName = model.OrganizationName;
            viewModel.IsWireTransferIncomeEnabled = model.IsWireTransferIncomeEnabled;
            viewModel.IsStripeCardIncomeEnabled = model.IsStripeCardIncomeEnabled;
            viewModel.Currency = model.Currency;
            viewModel.CreditLimit = model.CreditLimit;

            return View(viewModel);
        }        
    }
}
