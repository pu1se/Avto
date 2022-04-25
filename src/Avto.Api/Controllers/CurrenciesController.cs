using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Avto.BL.Services.Exchange.Currency;

namespace Avto.Api.Controllers
{
    [Route("currencies")]
    public class CurrenciesController : BaseApiController
    {
        private CurrencyService CurrencyService { get; }

        public CurrenciesController(CurrencyService currencyService)
        {
            CurrencyService = currencyService;
        }

        /// <summary>
        /// Get available currencies, that can be used for currency exchange.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAvailableCurrencies()
        {
            return await HttpResponse(() => CurrencyService.GetAvailableCurrencies());
        }
    }
}
