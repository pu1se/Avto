using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentMS.BL._Core.ApiRequests;
using PaymentMS.BL.Services.Exchange.Api.ApiModels;
using PaymentMS.BL.Services.Exchange.Api.CurrencyLayer;
using PaymentMS.DAL.Enums;
using RestSharp;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.API
{
    public class CurrencyLayerProviderApi : BaseProviderApi
    {
        private AppSettings Settings { get; }

        public CurrencyLayerProviderApi(AppSettings appSettings)
        {
            Settings = appSettings;
        }

        public override async Task<List<RateApiModel>> GetLatestTodayExchangeRateListAsync()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("http://apilayer.net/api/live?source=EUR&access_key="
                                  +Settings.CurrencyLayerApiKey, UriKind.Absolute)
            };

            var response = await client.ExecuteAsync(new RestRequest());
            var callResult = ApiRequestErrorHandler.HandleResponse(response);
            if (!callResult.IsSuccess)
            {
                throw new ThirdPartyApiException(callResult.ErrorMessage);
            }

            var exchangeDate = DateTime.UtcNow.Date;
            var rateList = new List<RateApiModel>();
            var dynamicResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var quotes = dynamicResponse.quotes;
            var ratesFromCurrencyLayer = new Dictionary<CurrencyType, decimal>();
            foreach (var quote in quotes)
            {
                var name = quote.Name.ToString();
                name = name.Replace(GetBaseCurrency().ToString(), "");
                var value = quote.Value.ToString();
                var currency = EnumHelper.TryParse<CurrencyType>(name);
                if (currency != null)
                {
                    ratesFromCurrencyLayer.Add(currency, decimal.Parse(value));
                }
            }

            foreach (var currency in GetSupportedCurrencies())
            {
                if (!ratesFromCurrencyLayer.ContainsKey(currency))
                {
                    continue;
                }

                var rate = ratesFromCurrencyLayer[currency];
                rateList.Add(new RateApiModel
                {
                    FromCurrency = GetBaseCurrency(),
                    ToCurrency = currency,
                    Rate = rate,
                    ExchangeDate = exchangeDate,
                });
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
