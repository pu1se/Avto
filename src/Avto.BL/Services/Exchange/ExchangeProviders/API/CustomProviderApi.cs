using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.Api.ApiModels;
using Avto.BL.Services.Exchange.Api.CurrencyLayer;
using Avto.BL.Services.Exchange.ExchangeConfigs.Converter;
using Avto.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.ExchangeProviders.API
{
    public class CustomProviderApi : BaseProviderApi
    {
        private Storage Storage { get; }
        public CustomProviderApi(Storage storage)
        {
            Storage = storage;
        }

        public override async Task<List<RateApiModel>> GetLatestTodayExchangeRateListAsync()
        {
            var rateList = new List<RateApiModel>();
            var currentDate = DateTime.UtcNow.Date;
            var customRateConfigList = await Storage.CurrencyExchangeConfis
                .Where(e => e.CustomRate != null)
                .ToListAsync();

            foreach (var organizationExchangeConfigs in customRateConfigList.GroupBy(x => x.OrganizationId))
            {
                var organizationId = organizationExchangeConfigs.Key;
                var baseCurrency = customRateConfigList.First(x => x.OrganizationId == organizationId).FromCurrency;
                var customRateListForOrganization = new List<RateApiModel>();
                foreach (var config in organizationExchangeConfigs)
                {
                    if (config.CustomRate == null)
                    {
                        continue;
                    }

                    customRateListForOrganization.Add(new RateApiModel
                    {
                        FromCurrency = config.FromCurrency,
                        ToCurrency = config.ToCurrency,
                        ExchangeDate = currentDate,
                        Rate = config.CustomRate.Value,
                        OrganizationId = config.OrganizationId
                    });   
                }

                customRateListForOrganization = GetCartesianProductForProvider(baseCurrency, customRateListForOrganization);
                rateList.AddRange(customRateListForOrganization);
            }

            return rateList;
        }

        public override CurrencyType GetBaseCurrency()
        {
            throw new System.NotImplementedException($"{nameof(GetBaseCurrency)} will be made protected later. Don't use it.");
        }

        public override List<CurrencyType> GetSupportedCurrencies()
        {
            return EnumHelper.ToList<CurrencyType>();
        }

        private List<RateApiModel> GetCartesianProductForProvider(CurrencyType baseCurrency,
            List<RateApiModel> rateApiModels)
        {
            var exchangeDate = rateApiModels.First().ExchangeDate;
            var organizationId = rateApiModels.First().OrganizationId;
            var fromCurrencyList = rateApiModels.Select(x => x.FromCurrency).ToList();
            var toCurrencyList = rateApiModels.Select(x => x.ToCurrency).ToList();
            var exchangeRateEntitiesForCurrentDate = rateApiModels
                .Where(
                    x =>
                    fromCurrencyList.Contains(x.FromCurrency) &&
                    toCurrencyList.Contains(x.ToCurrency)
                ).ToList();
            if (!exchangeRateEntitiesForCurrentDate.Any())
            {
                return rateApiModels;
            }


            var converterInput = exchangeRateEntitiesForCurrentDate.Select(e => new ExchangeConverterInput
            {
                FromCurrency = e.FromCurrency,
                ToCurrency = e.ToCurrency,
                Rate = e.Rate,
                ExchangeDate = e.ExchangeDate,
            }).ToList();
            var exchangeCurrencyConverter = new ExchangeCurrencyConverter(baseCurrency, converterInput);
            var unionCurrencyList = fromCurrencyList.Union(toCurrencyList).ToList();
            var rateList = new List<RateApiModel>();
            foreach (var fromCurrency in unionCurrencyList)
            {
                foreach (var toCurrency in unionCurrencyList)
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
