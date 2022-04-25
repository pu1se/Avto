using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL;
using PaymentMS.UI.Web.ViewModels;

namespace PaymentMS.UI.Web.Controllers
{
    public class ReceiverController : Controller
    {
        private StripeCardApiClient StripeCardApi { get; }

        public ReceiverController(StripeCardApiClient stripeCardApi)
        {
            StripeCardApi = stripeCardApi;
        }

        public IActionResult AddReceiver()
        {
            var viewModel = new AddStripeReceiverViewModel
            {
                ReceiverOrganizationId = Guid.NewGuid(),
                ReceiverList = StripeCardApi.GetAllReceivingWaysAsync().GetAwaiter().GetResult().Data
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReceiver(AddStripeReceiverViewModel viewModel)
        {            
            if (!ModelState.IsValid)
            {                
                viewModel.ReceiverList = StripeCardApi.GetAllReceivingWaysAsync().GetAwaiter().GetResult().Data;
                return View(viewModel);
            }

            var callResult = await StripeCardApi.AddReceivingWayAsync(viewModel);
            viewModel.ReceiverList = StripeCardApi.GetAllReceivingWaysAsync().GetAwaiter().GetResult().Data;
            if (!callResult.IsSuccess)
            {
                viewModel.ErrorMessage = callResult.ErrorMessage;
                return View(viewModel);
            }

            ModelState.Clear();

            viewModel.ReceiverOrganizationId = Guid.NewGuid();
            viewModel.IsSuccess = true;

            return View(viewModel);
        }
    }
}
