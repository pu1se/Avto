using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.BL._Core.ApiRequests;
using Avto.BL.Services.Exchange.Converter;
using Avto.BL.Services.Exchange.ExternalApis.ApiModels;
using Avto.DAL.Enums;
using Newtonsoft.Json;
using RestSharp;

namespace Avto.BL.Services.Exchange.ExternalApis
{
    public class CurrencyLayerApiProvider : BaseExternalApiProvider
    {
        private AppSettings Settings { get; }

        public CurrencyLayerApiProvider(AppSettings appSettings)
        {
            Settings = appSettings;
        }

        public async Task<List<CurrencyLayerRateApiModel>> GetLatestTodayExchangeRateListAsync()
        {
            var client = new RestClient(new Uri("http://apilayer.net/api/live?source=EUR&access_key="
                                                +Settings.CurrencyLayerApiKey, UriKind.Absolute));

            var response = await client.ExecuteAsync(new RestRequest());
            var callResult = ApiRequestErrorHandler.HandleResponse(response);
            if (!callResult.IsSuccess)
            {
                throw new ThirdPartyApiException(callResult.ErrorMessage);
            }

            var exchangeDate = DateTime.UtcNow.Date;
            var rateList = new List<CurrencyLayerRateApiModel>();
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
                    ratesFromCurrencyLayer.Add(currency, decimal.Parse(value, NumberStyles.Float));
                }
            }

            foreach (var currency in GetSupportedCurrencies())
            {
                if (!ratesFromCurrencyLayer.ContainsKey(currency))
                {
                    continue;
                }

                var rate = ratesFromCurrencyLayer[currency];
                rateList.Add(new CurrencyLayerRateApiModel
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

        private CurrencyType GetBaseCurrency()
        {
            return CurrencyType.EUR;
        }

        public List<CurrencyType> GetSupportedCurrencies()
        {
            return EnumHelper.ToList<CurrencyType>();
        }

        private List<CurrencyLayerRateApiModel> FilterRateList(List<CurrencyLayerRateApiModel> rateList)
        {
            var supportedCurrency = GetSupportedCurrencies();
            rateList = rateList.Where(x => supportedCurrency.Contains(x.FromCurrency) && supportedCurrency.Contains(x.ToCurrency)).ToList();
            return rateList;
        }

        private List<CurrencyLayerRateApiModel> GetCartesianProductForProvider(
            List<CurrencyLayerRateApiModel> rateApiModels
        )
        {
            if (!rateApiModels.Any())
            {
                return new List<CurrencyLayerRateApiModel>();
            }

            var baseCurrency = rateApiModels.First().FromCurrency;
            var inputRateList = rateApiModels.Select( 
                x =>
                    new ExchangeConverterInput
                    {
                        FromCurrency = x.FromCurrency,
                        ToCurrency = x.ToCurrency,
                        Rate = x.Rate,
                        ExchangeDate = x.ExchangeDate,
                    }
            ).ToList();

            var exchangeDate = rateApiModels.First().ExchangeDate;

            var exchangeCurrencyConverter = new ExchangeCurrencyConverter(baseCurrency, inputRateList);
            var rateList = new List<CurrencyLayerRateApiModel>();
            var currencyList = EnumHelper.ToList<CurrencyType>();
            foreach (var fromCurrency in currencyList)
            {
                foreach (var toCurrency in currencyList)
                {
                    var rate = exchangeCurrencyConverter.From(fromCurrency).To(toCurrency);
                    rateList.Add(new CurrencyLayerRateApiModel
                    {
                        FromCurrency = rate.FromCurrency,
                        ToCurrency = rate.ToCurrency,
                        Rate = rate.Rate,
                        ExchangeDate = exchangeDate
                    });
                }
            }

            return rateList;
        }
    }
}
