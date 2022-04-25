using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Avto.BL.Services.Exchange.CalculatedExchangeRates;

namespace Avto.Api.Controllers
{
    [Route("exchange/calculate")]
    public class CalculatedExchangeRatesController : BaseApiController
    {
        private CalculatedExchangeRatesService CalculatedExchangeRatesService { get; }

        public CalculatedExchangeRatesController(CalculatedExchangeRatesService calculatedExchangeRatesService)
        {
            CalculatedExchangeRatesService = calculatedExchangeRatesService;
        }

        /// <summary>
        /// Calculate exchange rates and save them to cache table.
        /// </summary>
        [HttpPost]
        [Route("")]
        public Task<IActionResult> CalculateAllTodayExchangeRates()
        {
            return this.HttpResponse(() => CalculatedExchangeRatesService.RefreshAndCalculateAllTodayExchangeRates());
        }

        /// <summary>
        /// Get common exchange rates.
        /// </summary>
        [HttpGet]
        [Route("rates/today")]
        public Task<IActionResult> GetCalculatedCommonExchangeRatesForToday()
        {
            return this.HttpResponse(() => CalculatedExchangeRatesService.GetCalculatedCommonExchangeRates(DateTime.UtcNow));
        }

        /// <summary>
        /// Get exchange rates configured by organization.
        /// </summary>
        [HttpGet]
        [Route("organization/{organizationId}/rates/today")]
        public Task<IActionResult> GetCalculatedConfiguredByOrganizationExchangeRatesForToday([FromRoute] Guid organizationId)
        {
            return this.HttpResponse(() => CalculatedExchangeRatesService.GetCalculatedConfiguredByOrganizationExchangeRates(organizationId, DateTime.UtcNow));
        }
    }
}
