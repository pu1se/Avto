using Avto.BL.Services.Exchange.ExchangePrediction;
using Avto.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo;

namespace Avto.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Avto.BL.Services.Exchange.ExchangeProviders;
    using Avto.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates;
    using Avto.DAL.Enums;


    [Route("exchange/report")]
    public class ExchangeReportController : BaseApiController
    {
        private ExchangeReportService ReportService { get; }

        public ExchangeReportController(ExchangeReportService reportService)
        {
            ReportService = reportService;
        }
        
        [AllowAnonymous]
        [Route("")] // from/{fromCurrency}/to/{toCurrency}/period/{periodInDays}
        [HttpPost]
        public async Task<IActionResult> GetCurrencyChart([FromBody]GetExchangeReportItemQuery query)
        {
            return await HttpResponse(() => ReportService.GetCurrencyExchangeInfo(query));
        }
    }
}
