using System.Threading.Tasks;
using Avto.BL.Services.Exchange;
using Avto.BL.Services.Exchange.GetExchangeRates;
using Microsoft.AspNetCore.Mvc;

namespace Avto.Api.Controllers
{
    [Route("exchange")]
    public class ExchangeController : BaseApiController
    {
        private ExchangeService ExchangeService { get; }

        public ExchangeController(ExchangeService exchangeService)
        {
            ExchangeService = exchangeService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> GetCurrencyChart([FromBody]GetExchangeRatesQuery query)
        {
            return await HttpResponse(() => ExchangeService.GetExchangeRates(query));
        }
    }
}
