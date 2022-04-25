using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.DAL.Enums;
using PaymentMS.UI.Web.ApiRequests;
using PaymentMS.UI.Web.ViewModels.Balance;

namespace PaymentMS.UI.Web.Controllers
{
    public class HistoryController : Controller
    {
        private BalanceApiClient BalanceApi { get; }

        public HistoryController(BalanceApiClient balanceApi)
        {
            BalanceApi = balanceApi;
        }

        [HttpGet]
        public async Task<IActionResult> BalancePaymentHistory()
        {
            var viewModel = new SendBalancePaymentViewModel();

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
        public async Task<IActionResult> HistoryTable(Guid clientId, Guid providerId)
        {
            var viewModel = new HistoryTableViewModel();

            var getPaymentsResult = await BalanceApi.GetPaymentHistory(providerId, clientId);
            if (!getPaymentsResult.IsSuccess)
            {
                viewModel.ErrorMessage = getPaymentsResult.ErrorMessage;
                return View(viewModel);
            }

            var payments = getPaymentsResult.Data;
            viewModel.Payments = payments;

            return View(viewModel);
        }
    }
}
