using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using PaymentMS.BL._Core.ApiRequests;
using PaymentMS.BL.Services.Exchange.Api.ApiModels;
using PaymentMS.BL.Services.Exchange.Api.CurrencyLayer;
using PaymentMS.DAL.Enums;
using RestSharp;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.API
{
    public class EcbProviderApi : BaseProviderApi
    {
        public override async Task<List<RateApiModel>> GetLatestTodayExchangeRateListAsync()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", UriKind.Absolute)
            };

            var response = await client.ExecuteAsync(new RestRequest());
            var callResult = ApiRequestErrorHandler.HandleResponse(response);
            if (!callResult.IsSuccess)
            {
                throw new ThirdPartyApiException(callResult.ErrorMessage);
            }

            var exchangeDate = DateTime.UtcNow.Date;

            var rateList = new List<RateApiModel>();
            var xmlDocument = XDocument.Parse(response.Content);
            var xmlCurrencyRateList = xmlDocument.Descendants()
                .Where(
                    xElement => 
                        xElement.Name.LocalName == "Cube" &&
                        xElement.HasAttributes &&
                        xElement.IsEmpty
                );
            foreach (var xmlRate in xmlCurrencyRateList)
            {
                var currencyCode = xmlRate.Attribute("currency")?.Value;
                var rateAsString = xmlRate.Attribute("rate")?.Value;
                var currency = EnumHelper.TryParse<CurrencyType>(currencyCode);

                if (currencyCode.IsNullOrEmpty() 
                    || rateAsString.IsNullOrEmpty()
                    || !decimal.TryParse(rateAsString, out var rate)
                    || currency == null)
                {
                    continue;
                }

                rateList.Add(new RateApiModel
                {
                    FromCurrency = GetBaseCurrency(),
                    ToCurrency = currency.Value,
                    Rate = rate,
                    ExchangeDate = exchangeDate
                });
            }

            rateList = FilterRateList(rateList);
            rateList = GetCartesianProductForProvider(rateList);
            return rateList;
        }

        public async Task<List<RateApiModel>> GetAllHistoricalExchangeRateListAsync()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml", UriKind.Absolute)
            };

            var response = await client.ExecuteAsync(new RestRequest());
            var callResult = ApiRequestErrorHandler.HandleResponse(response);
            if (!callResult.IsSuccess)
            {
                throw new ThirdPartyApiException(callResult.ErrorMessage);
            }

            var rateList = new List<RateApiModel>();
            var xmlDocument = XDocument.Parse(response.Content);
            var xmlPerDays = xmlDocument.Descendants()
                .Where(
                    xElement => 
                        xElement.Name.LocalName == "Cube" &&
                        xElement.HasAttributes &&
                        xElement.Attribute("time") != null
                );
            var fromDate = new DateTime(2010, 01, 01);

            foreach (var xmlPerDay in xmlPerDays)
            {
                var exchangeDateAsString = xmlPerDay.Attribute("time")?.Value;
                if (exchangeDateAsString.IsNullOrEmpty())
                {
                    continue;
                }

                if (!DateTime.TryParse(exchangeDateAsString, out var exchangeDate))
                {
                    continue;
                }

                if (exchangeDate < fromDate)
                {
                    continue;
                }

                foreach (var xmlElement in xmlPerDay.Descendants())
                {
                    var currencyCode = xmlElement.Attribute("currency")?.Value;
                    var rateAsString = xmlElement.Attribute("rate")?.Value;
                    var currency = EnumHelper.TryParse<CurrencyType>(currencyCode);

                    if (currencyCode.IsNullOrEmpty() 
                        || rateAsString.IsNullOrEmpty()
                        || !decimal.TryParse(rateAsString, out decimal rate)
                        || currency == null)
                    {
                        continue;
                    }

                    rateList.Add(new RateApiModel
                    {
                        FromCurrency = GetBaseCurrency(),
                        ToCurrency = currency.Value,
                        Rate = rate,
                        ExchangeDate = exchangeDate
                    });
                }

                AddBaseCurrency(rateList, exchangeDate);
            }

            rateList = FilterRateList(rateList);
            rateList = GetCartesianProductForProvider(rateList);
            return rateList;
        }

        public override CurrencyType GetBaseCurrency()
        {
            return CurrencyType.EUR;
        }

        public override List<CurrencyType> GetSupportedCurrencies()
        {
            return EnumHelper.ToList<CurrencyType>();
        }
    }
}
