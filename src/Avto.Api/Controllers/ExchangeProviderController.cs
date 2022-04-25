using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Avto.BL.Services.Exchange.Currency;
using Avto.BL.Services.Exchange.ExchangeProviders;
using Avto.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates;
using Avto.DAL.Enums;

namespace Avto.Api.Controllers
{
    [Route("exchange/providers")]
    public class ExchangeProviderController : BaseApiController
    {
        private ExchangeProviderService ProviderService { get; }
        private CurrencyService CurrencyService { get; }

        public ExchangeProviderController(ExchangeProviderService providerService, CurrencyService currencyService)
        {
            ProviderService = providerService;
            CurrencyService = currencyService;
        }

        /// <summary>
        /// Get available currencies, that can be used for currency exchange.
        /// </summary>
        [AllowAnonymous]
        [Route("currencies")]
        [HttpGet]
        // todo: should be removed in next version.
        [Obsolete("should be removed in next version.")]
        public async Task<IActionResult> GetAvailableCurrencies()
        {
            return await HttpResponse(() => CurrencyService.GetAvailableCurrencies());
        }

        /// <summary>
        /// Get exchange rate source providers, that can be used for currency exchange.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProviders()
        {
            return await HttpResponse(() => ProviderService.GetExchangeProviders());
        }

        /// <summary>
        /// Get exchange rate source providers, that can be used for currency exchange filtered by supported currency.
        /// </summary>
        [HttpGet]
        [Route("{currency}")]
        public async Task<IActionResult> GetProvidersWhichSupportCurrency(CurrencyType currency)
        {
            return await HttpResponse(() => ProviderService.GetExchangeProviders(currency));
        }

        /// <summary>
        /// Get exchange rate from source provider.
        /// </summary>
        [HttpPost]
        [Route("rate")]
        public async Task<IActionResult> GetRateFromProvider(
            [FromBody]GetTodayRateFromProviderQuery query
        )
        {
            return await HttpResponse(() => ProviderService.GetTodayRateFromProvider(query));
        }

        /// <summary>
        /// Get exchange rate from ECB provider plus standard surcharge from config file.
        /// </summary>
        [HttpGet]
        [Route("ECB/rates-plus-surcharge")]
        // todo: should be removed in next version.
        [Obsolete("should be removed in next version.")]
        public async Task<IActionResult> GetRateListFromEcbProviderWithSurcharge()
        {
            return await HttpResponse(() => ProviderService.GetTodayCommonExchangeRatesFromEcbWithSurcharge());
        }

        /// <summary>
        /// Refresh rates from specific provider.
        /// </summary>
        [HttpPost]
        [Route("{provider}/rates/refresh")]
        public async Task<IActionResult> RefreshRates([FromRoute]ExchangeProviderType provider)
        {
            return await HttpResponse(() => ProviderService.RefreshTodayRatesForProvider(provider));
        }
    }
}
