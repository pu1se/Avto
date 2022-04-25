using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaymentMS.BL.Services.Balance;
using PaymentMS.BL.Services.Balance.Models;

namespace PaymentMS.Api.Controllers
{
    [Route("balance")]
    public class BalanceController : BaseApiController
    {
        private BalanceService BalanceService { get; }
        public BalanceController(BalanceService balanceService)
        {
            BalanceService = balanceService;
        }

        [HttpGet]
        [Route("possible-provider-organizations")]
        public async Task<IActionResult> GetReceiverOrganizations()
        {
            return await HttpResponse(() => BalanceService.GetPossibleProviderOrganzations());
        }

        [HttpGet]
        [Route("providers")]
        public async Task<IActionResult> GetBalanceProviders()
        {
            return await HttpResponse(() => BalanceService.GetProviders());
        }

        [HttpGet]
        [Route("providers/{organizationId:guid}")]
        public async Task<IActionResult> GetBalanceProvider(Guid organizationId)
        {
            return await HttpResponse(() => BalanceService.GetProvider(organizationId));
        }

        [HttpPut]
        [Route("providers")]
        public async Task<IActionResult> AddEditProvider([FromBody]AddEditBalanceProviderCommand command)
        {
            return await HttpResponse(() => BalanceService.AddEditProvider(command));
        }

        [HttpGet]
        [Route("providers/{providerOrganizationId:guid}/clients")]
        public async Task<IActionResult> GetClients(Guid providerOrganizationId)
        {
            return await HttpResponse(() => BalanceService.GetClients(providerOrganizationId));
        }

        [HttpGet]
        [Route("providers/{providerOrganizationId:guid}/clients/{clientOrganizationId:guid}")]
        public async Task<IActionResult> GetBalanceClient(Guid providerOrganizationId, Guid clientOrganizationId)
        {
            return await HttpResponse(() => BalanceService.GetClient(providerOrganizationId, clientOrganizationId));
        }

        [HttpPost]
        [Route("providers/{providerOrganizationId:guid}/clients")]
        public async Task<IActionResult> AddClient(Guid providerOrganizationId, [FromBody]AddBalanceClientCommand model)
        {
            model.ProviderOrganizationId = providerOrganizationId;
            return await HttpResponse(() => BalanceService.AddClient(model));
        }

        [HttpPost]
        [Route("providers/{providerOrganizationId:guid}/clients/{clientOrganizationId:guid}/increase/wire-transfer")]
        public async Task<IActionResult> IncreaseBalanceByWireTransfer(
            Guid providerOrganizationId,
            Guid clientOrganizationId,
            [FromBody]IncreaseBalaceByWireTransferCommand command)
        {
            command.ProviderOrganizationId = providerOrganizationId;
            command.ClientOrganizationId = clientOrganizationId;
            return await HttpResponse(() => BalanceService.IncreaseByWireTransfer(command));
        }

        [HttpPost]
        [Route("providers/{providerOrganizationId:guid}/clients/{clientOrganizationId:guid}/increase/stripe-card")]
        public async Task<IActionResult> IncreaseBalanceByStripeCard(
            Guid providerOrganizationId,
            Guid clientOrganizationId,
            [FromBody]IncreaseBalanceByStripeCardCommand command)
        {
            command.ProviderOrganizationId = providerOrganizationId;
            command.ClientOrganizationId = clientOrganizationId;
            return await HttpResponse(() => BalanceService.IncreaseByStripeCard(command));
        }

        [HttpPost]
        [Route("providers/{providerOrganizationId:guid}/clients/{clientOrganizationId:guid}/payments")]
        public async Task<IActionResult> SendPayment(
            Guid providerOrganizationId,
            Guid clientOrganizationId,
            [FromBody] SendBalancePaymentCommand command)
        {
            command.ProviderOrganizationId = providerOrganizationId;
            command.ClientOrganizationId = clientOrganizationId;
            return await HttpResponse(() => BalanceService.SendPayment(command));
        }

        [HttpGet]
        [Route("providers/{providerOrganizationId:guid}/clients/{clientOrganizationId:guid}/payments")]
        public async Task<IActionResult> GetPaymentHistory(Guid providerOrganizationId, Guid clientOrganizationId)
        {
            return await HttpResponse(() => BalanceService.GetPayments(providerOrganizationId, clientOrganizationId));
        }
    }
}
