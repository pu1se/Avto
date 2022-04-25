using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL;
using PaymentMS.DAL.Enums;
using PaymentMS.UI.Web.ViewModels;

namespace PaymentMS.UI.Web.Controllers
{
    public class CardsController : Controller
    {
        private StripeCardApiClient StripeCardApi { get; }

        public CardsController(StripeCardApiClient stripeCardApi)
        {
            StripeCardApi = stripeCardApi;
        }

        [HttpGet]
        public async Task<IActionResult> AddCard()
        {
            var callResult = await StripeCardApi.GetAllReceivingWaysAsync();
            var viewModel = new AddStripeCardViewModel
            {
                ReceiverList = callResult.Data,
                ErrorMessage = callResult.ErrorMessage
            };

            if (callResult.IsSuccess && callResult.Data.Any())
            {
                viewModel.SelectedReceiverId = callResult.Data.First().ReceiverOrganizationId;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCard(AddStripeCardViewModel viewModel)
        {
            var model = new AddEditSendingWayCommand
            {
                Token = viewModel.Token,
                SenderOrganizationId = TestData.PaymentSenderOrganizationId,
                SenderEmail = "pavel_pontus@tut.by",
                SenderName = "pavel pontus",
                SenderOrganizationName = "customre 1",
                ReceiverOrganizationId = viewModel.SelectedReceiverId,                
            };
            var callResult = await StripeCardApi.AddSendingWayAsync(model);
            viewModel.ReceiverList = StripeCardApi.GetAllReceivingWaysAsync().GetAwaiter().GetResult().Data;
            if (!callResult.IsSuccess)
            {
                viewModel.ErrorMessage = callResult.ErrorMessage;
                return View(viewModel);
            }

            viewModel.IsSuccess = true;
            return View(viewModel);
        }



        [HttpGet]
        public async Task<IActionResult> GetCurrentCard(Guid receiverOrganizationId)
        {
            var card = await StripeCardApi.GetDefaultSendingWayByOrgAsync(TestData.PaymentSenderOrganizationId, receiverOrganizationId);
            return PartialView("CurrentCard", card.Data);
        }



        [HttpGet]
        public IActionResult Pay()
        {
            var viewModel = new PayWithStripeCard
            {
                ReceiverList = StripeCardApi.GetAllReceivingWaysAsync().GetAwaiter().GetResult().Data,
                PaymentList = StripeCardApi.GetPaymentsAsync(
                    TestData.PaymentSenderOrganizationId
                ).GetAwaiter().GetResult().Data,
                CurrencyList = EnumHelper.ToDictionary<CurrencyType>().Select(x=>new SelectListItem
                {
                    Value = x.Key.ToLower(),
                    Text = x.Value
                })
            };

            if (viewModel.ReceiverList.Any())
            {
                viewModel.SelectedReceivingWayId = viewModel.ReceiverList.First().ReceiverOrganizationId;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PayWithStripeCard viewModel)
        {
            var model = new SendPaymentCommand
            {
                SenderOrganizationId = TestData.PaymentSenderOrganizationId,
                ReceiverOrganizationId = viewModel.SelectedReceivingWayId,
                PaymentAmount = viewModel.PaymentAmount,
                PaymentCurrency = viewModel.PaymentCurrency
            };
           
            var callResult = await StripeCardApi.SendPaymentAsync(model);

            viewModel.ReceiverList = StripeCardApi.GetAllReceivingWaysAsync().GetAwaiter().GetResult().Data;
            viewModel.PaymentList = StripeCardApi.GetPaymentsAsync(TestData.PaymentSenderOrganizationId).GetAwaiter().GetResult().Data;
            viewModel.CurrencyList = EnumHelper.ToDictionary<CurrencyType>().Select(x => new SelectListItem
            {
                Value = x.Key.ToLower(),
                Text = x.Value
            });
            if (!callResult.IsSuccess)
            {
                viewModel.ErrorMessage = callResult.ErrorMessage;
                return View(viewModel);
            }

            
            viewModel.IsSuccess = true;
            return View(viewModel);
        }


    }
}