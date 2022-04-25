using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL;
using PaymentMS.UI.Web.ApiRequests;
using PaymentMS.UI.Web.ViewModels;
using PaymentMS.UI.Web.ViewModels.Balance;

namespace PaymentMS.UI.Web.Controllers
{
    public class BalanceClientController : Controller
    {
        private BalanceApiClient BalanceApi { get; }

        public BalanceClientController(BalanceApiClient balanceApi)
        {
            BalanceApi = balanceApi;
        }

        [HttpGet]
        public async Task<IActionResult> AddBalanceClient()
        {
            var viewModel = new AddBalanceClientViewModel();

            var callResult = await BalanceApi.GetProviders();
            var receiverList = callResult.Data.Select(x => new BalanceProviderOrganizationModel
            {
                OrganizationId = x.OrganizationId,
                OrganizationName = x.OrganizationName
            }).ToList();
            viewModel.ReceiverList = receiverList;
            if (receiverList.Any())
            {
                viewModel.SelectedProviderId = receiverList.First().OrganizationId;
            }


            if (!callResult.IsSuccess)
            {
                viewModel.ErrorMessage = callResult.ErrorMessage;
                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBalanceClient(AddBalanceClientViewModel viewModel)
        {
            var callResult = await BalanceApi.GetProviders();
            var receiverList = callResult.Data.Select(x => new BalanceProviderOrganizationModel
            {
                OrganizationId = x.OrganizationId,
                OrganizationName = x.OrganizationName
            }).ToList();
            viewModel.ReceiverList = receiverList;

            var createClientResult = await BalanceApi.AddClient(new AddBalanceClientCommand
            {
                ProviderOrganizationId = viewModel.SelectedProviderId,
                ClientOrganizationId = TestData.PaymentSenderOrganizationId
            });

            if (!createClientResult.IsSuccess)
            {
                viewModel.ErrorMessage = createClientResult.ErrorMessage;
                return View(viewModel);
            }

            viewModel.IsSuccess = true;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BalanceClient(Guid providerId)
        {
            var viewModel = new BalanceClientViewModel();

            var clientResult = await BalanceApi.GetClient(providerId, TestData.PaymentSenderOrganizationId);
            if (!clientResult.IsSuccess)
            {
                viewModel.ErrorMessage = clientResult.ErrorMessage;
                return View(viewModel);
            }

            var client = clientResult.Data;

            viewModel.Amount = client.Amount;
            viewModel.ClientOrganizationId = client.ClientOrganizationId;
            viewModel.ProviderOrganizationId = client.ProviderOrganizationId;
            viewModel.CreditLimit = client.CreditLimit;
            viewModel.Currency = client.Currency;

            return View(viewModel);
        }
    }
}
