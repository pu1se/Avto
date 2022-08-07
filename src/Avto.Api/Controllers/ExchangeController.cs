using System.Reflection;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange;
using Avto.BL.Services.Exchange.GetExchangeRates;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetSpecificExchangeRatesInfo([FromBody]GetSpecificExchangeRatesInfoQuery infoQuery)
        {
            return await HttpResponse(() => ExchangeService.GetSpecificExchangeRatesInfo(infoQuery));
        }

        [HttpGet]
        [Route("currencies")]
        public async Task<IActionResult> GetAvailableCurrencies()
        {
            return await HttpResponse(() => ExchangeService.GetAvailableCurrencies());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("version")]
        public IActionResult GetVersion()
        {
            var version = typeof(Startup)
                .GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            return Ok(version);
        }
    }
}
