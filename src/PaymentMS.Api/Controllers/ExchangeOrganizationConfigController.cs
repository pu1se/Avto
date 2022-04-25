using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.AddEditExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.DeleteExchangeConfig;

namespace PaymentMS.Api.Controllers
{
    [Route("exchange")]
    public class ExchangeOrganizationConfigController : BaseApiController
    {
        public ExchangeConfigService ConfigService { get; }
        public ExchangeOrganizationConfigController(ExchangeConfigService configService)
        {
            ConfigService = configService;
        }

        /// <summary>
        /// Get exchange configs of organization.
        /// </summary>
        [HttpGet]
        [Route("organizations/{organizationId:guid}/configs")]
        public async Task<IActionResult> GetCurrencyConfigsByOrganization([FromRoute]Guid organizationId)
        {
            return await HttpResponse(() => ConfigService.GetOrganizationExchangeConfigs(organizationId));
        }

        /// <summary>
        /// Add or edit exchange configs of organization.
        /// </summary>
        [HttpPut]
        [Route("organizations/{organizationId:guid}/configs")]
        public async Task<IActionResult> SaveCurrencyConfig([FromBody]AddEditExchangeConfigCommand command)
        {
            return await HttpResponse(() => ConfigService.AddEditOrganizationExchangeConfig(command));
        }

        /// <summary>
        /// delete exchange configs of organization.
        /// </summary>
        [HttpDelete]
        [Route("organizations/{organizationId:guid}/configs")]
        public async Task<IActionResult> DeleteCurrencyConfig([FromBody]DeleteExchangeConfigCommand command)
        {
            return await HttpResponse(() => ConfigService.DeleteOrganizationExchangeConfig(command));
        }

        /// <summary>
        /// Get exchange rates based on exchange configs of organization.
        /// </summary>
        [HttpGet]
        [Route("organizations/{organizationId:guid}/rates")]
        public async Task<IActionResult> GetLatestExchangeRatesByOrganization([FromRoute]Guid organizationId)
        {
            return await HttpResponse(() => ConfigService.GetTodayRatesConfiguredByOrganization(organizationId));
        }
    }
}
