using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.Api.ApiModels;
using Avto.BL.Services.Exchange.ExchangeConfigs.Converter;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.Api.CurrencyLayer
{
    public abstract class BaseProviderApi
    {
        public abstract Task<List<RateApiModel>> GetLatestTodayExchangeRateListAsync();

        //todo: remove this after release of currency exchange v2
        public abstract CurrencyType GetBaseCurrency();
        public abstract List<CurrencyType> GetSupportedCurrencies();

        public bool IsCurrencySupported(CurrencyType currency)
        {
            return GetSupportedCurrencies().Contains(currency);
        }

        protected List<RateApiModel> FilterRateList(List<RateApiModel> rateList)
        {
            var supportedCurrency = GetSupportedCurrencies();
            rateList = rateList.Where(x => supportedCurrency.Contains(x.ToCurrency)).ToList();
            return rateList;
        }

        protected void AddBaseCurrency(List<RateApiModel> rateList, DateTime exchangeDate)
        {
            if (!rateList.Any(
                item =>
                    item.FromCurrency == GetBaseCurrency() && 
                    item.ToCurrency == GetBaseCurrency()))
            {
                rateList.Add(new RateApiModel
                {
                    FromCurrency = GetBaseCurrency(),
                    ToCurrency = GetBaseCurrency(),
                    ExchangeDate = exchangeDate,
                    Rate = 1
                });
            }
        }

        protected virtual List<RateApiModel> GetCartesianProductForProvider(
            List<RateApiModel> rateApiModels
        )
        {
            if (!rateApiModels.Any())
            {
                return new List<RateApiModel>();
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
            var organizationId = rateApiModels.First().OrganizationId;

            var exchangeCurrencyConverter = new ExchangeCurrencyConverter(baseCurrency, inputRateList);
            var rateList = new List<RateApiModel>();
            var currencyList = EnumHelper.ToList<CurrencyType>();
            foreach (var fromCurrency in currencyList)
            {
                foreach (var toCurrency in currencyList)
                {
                    var rate = exchangeCurrencyConverter.From(fromCurrency).To(toCurrency);
                    rateList.Add(new RateApiModel
                    {
                        FromCurrency = rate.FromCurrency,
                        ToCurrency = rate.ToCurrency,
                        Rate = rate.Rate,
                        OrganizationId = organizationId,
                        ExchangeDate = exchangeDate
                    });
                }
            }

            return rateList;
        }
    }
}