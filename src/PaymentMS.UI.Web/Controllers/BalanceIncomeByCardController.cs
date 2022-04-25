using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.DAL.Enums;
using PaymentMS.UI.Web.ApiRequests;
using PaymentMS.UI.Web.ViewModels.Balance;

namespace PaymentMS.UI.Web.Controllers
{
    public class BalanceIncomeByCardController : Controller
    {
        private BalanceApiClient BalanceApi { get; }
        private StripeCardApiClient StripeApi { get; }

        public BalanceIncomeByCardController(BalanceApiClient balanceApi, StripeCardApiClient stripeApi)
        {
            BalanceApi = balanceApi;
            StripeApi = stripeApi;
        }

        [HttpGet]
        public async Task<IActionResult> IncreaseByCard()
        {
            var viewModel = new IncreaseByCardViewModel();

            var callResult = await BalanceApi.GetPossibleProviderOrganizationsAsync();
            var receiverList = callResult.Data;
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
        public async Task<IActionResult> IncreaseByCard(IncreaseByCardViewModel viewModel)
        {
            var callResult = await BalanceApi.GetPossibleProviderOrganizationsAsync();
            var receiverList = callResult.Data;
            viewModel.ReceiverList = receiverList;


            var model = new IncreaseBalanceByStripeCardCommand
            {
                ClientOrganizationId = viewModel.SelectedClientId,
                ProviderOrganizationId = viewModel.SelectedProviderId,
                AddAmount = viewModel.AddAmount
            };
            var increaseResult = await BalanceApi.IncreaseByCard(model);
            if (!increaseResult.IsSuccess)
            {
                viewModel.ErrorMessage = increaseResult.ErrorMessage;
                return View(viewModel);
            }

            viewModel.IsSuccess = true;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BalanceClientList(Guid providerId)
        {
            var viewModel = new BalanceClientListViewModel();

            var getClientListResult = await BalanceApi.GetClientList(providerId);
            if (!getClientListResult.IsSuccess)
            {
                viewModel.ErrorMessage = getClientListResult.ErrorMessage;
                return View(viewModel);
            }

            var clientList = getClientListResult.Data;
            viewModel.ClientList = clientList;
            if (clientList.Any())
            {
                viewModel.SelectedClientId = clientList.First().ClientOrganizationId;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Increase(Guid clientId, Guid providerId)
        {
            var viewModel = new IncreaseViewModel();
            viewModel.CurrencyList = EnumHelper.ToDictionary<CurrencyType>().Select(x => new SelectListItem
            {
                Value = x.Key.ToLower(),
                Text = x.Value
            });

            var getClientResult = await BalanceApi.GetClient(providerId, clientId);
            if (!getClientResult.IsSuccess)
            {
                viewModel.ErrorMessage = getClientResult.ErrorMessage;
                return View(viewModel);
            }
            var client = getClientResult.Data;
            viewModel.CurrentAmount = client.Amount;
            viewModel.Currency = client.Currency;


            var cardResult = await StripeApi.GetDefaultSendingWayByOrgAsync(clientId, providerId);
            if (!cardResult.IsSuccess)
            {
                return View(viewModel);
            }

            viewModel.CurrentCard = cardResult.Data;

            return View(viewModel);
        }
    }
}
