using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Converter;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization
{
    public class GetRatesQueryHandler 
        : QueryHandler<GetRatesQuery, CallListDataResult<RateResponse>>
    {
        public GetRatesQueryHandler(
            Storage storage, 
            LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<RateResponse>> HandleCommandAsync(GetRatesQuery query)
        {
            var organizationId = query.OrganizationId;
            var rateList = new List<RateResponse>();
            var currencyConfigList = await Storage.CurrencyExchangeConfis
                .Where(e => e.OrganizationId == organizationId)
                .ToListAsync();
            if (!currencyConfigList.Any())
            {
                return SuccessListResult(rateList);
            }

            await FillNotCustomRates(currencyConfigList, rateList, organizationId);

            await FillCustomRates(currencyConfigList, rateList, organizationId);

            if (!rateList.Any())
            {
                return SuccessListResult(rateList);
            }

            MakeCartesianProductFromCustomAndNotCustomRates(currencyConfigList, rateList, organizationId);

            foreach (var rate in rateList)
            {
                rate.ExchangeRate = rate.ExchangeRate.ToRoundedRate();
                rate.ExchangeRateWithSurcharge = rate.ExchangeRateWithSurcharge.ToRoundedRate();
            }

            return SuccessListResult(rateList);
        }

        
        private async Task FillNotCustomRates(List<CurrencyExchangeConfigEntity> currencyConfigList, List<RateResponse> rateList,
            Guid organizationId)
        {
            var groupedByProviderConfigs = currencyConfigList
                .Where(x => x.ExchangeProvider != ExchangeProviderType.Custom)
                .GroupBy(x => x.ExchangeProvider);
            foreach (var configsGroupedByProvider in groupedByProviderConfigs)
            {
                var provider = configsGroupedByProvider.Key;

                var firstRateEntityWithDate = await Storage.CurrencyExchangeRates
                    .Where(e => e.ExchangeProvider == provider)
                    .OrderByDescending(e => e.ExchangeDate)
                    .GetAsync();
                if (firstRateEntityWithDate == null)
                {
                    continue;
                }
                var exchangeDate = firstRateEntityWithDate.ExchangeDate;

                var fromCurrencyList = configsGroupedByProvider.Select(x => x.FromCurrency).ToList();
                var toCurrencyList = configsGroupedByProvider.Select(x => x.ToCurrency).ToList();
                var unionCurrencyList = fromCurrencyList.Union(toCurrencyList).ToList();
                var unionCurrencyListAsString = unionCurrencyList.Select(x => x.ToString()).ToList();
                var exchangeRateEntities = await Storage.CurrencyExchangeRates
                    .Where(
                        e =>
                            e.ExchangeProvider == provider &&
                            e.ExchangeDate == exchangeDate &&
                            unionCurrencyListAsString.Contains(e.FromCurrencyCode) &&
                            unionCurrencyListAsString.Contains(e.ToCurrencyCode)
                    ).ToListAsync();
                if (!exchangeRateEntities.Any())
                {
                    continue;
                }

                foreach (var fromCurrency in unionCurrencyList)
                {
                    foreach (var toCurrency in unionCurrencyList)
                    {
                        if (rateList.Any(x => x.FromCurrency == fromCurrency && x.ToCurrency == toCurrency))
                        {
                            continue;
                        }

                        var rate = exchangeRateEntities.FirstOrDefault(
                            x => 
                            x.FromCurrencyCode == fromCurrency.ToString() && 
                            x.ToCurrencyCode == toCurrency.ToString());
                        if (rate == null)
                        {
                            continue;
                        }
                        var surcharge = GetSurchargeAsPercent(currencyConfigList, rate.FromCurrencyCode.AsEnum<CurrencyType>(), rate.ToCurrencyCode.AsEnum<CurrencyType>());
                        var rateWithSurcharge = GetRateWithSurcharge(rate.Rate, surcharge);
                        rateList.Add(new RateResponse
                        {
                            FromCurrency = rate.FromCurrencyCode.AsEnum<CurrencyType>(),
                            ToCurrency = rate.ToCurrencyCode.AsEnum<CurrencyType>(),
                            ExchangeRate = rate.Rate,
                            ExchangeRateWithSurcharge = rateWithSurcharge,
                            ExchangeDate = exchangeDate,
                            ProvidedByOrganizationId = organizationId
                        });
                    }
                }
            }
        }

        private async Task FillCustomRates(
            List<CurrencyExchangeConfigEntity> currencyConfigList, 
            List<RateResponse> rateList, 
            Guid organizationId)
        {
            var customConfigs = currencyConfigList
                .Where(x => x.ExchangeProvider.IsCustom())
                .ToList();

            if (!customConfigs.Any())
            {
                return;
            }

            var firstRateEntityWithDate = await Storage.CurrencyExchangeRates
                .Where(
                    e => 
                    e.ExchangeProvider == ExchangeProviderType.Custom
                    &&
                    e.OrganizationId == organizationId
                )
                .OrderByDescending(e => e.ExchangeDate)
                .GetAsync();
            if (firstRateEntityWithDate == null)
            {
                return;
            }
            var exchangeDate = firstRateEntityWithDate.ExchangeDate;

            var fromCurrencyList = customConfigs.Select(x => x.FromCurrency).ToList();
            var toCurrencyList = customConfigs.Select(x => x.ToCurrency).ToList();
            var unionCurrencyList = fromCurrencyList.Union(toCurrencyList).ToList();
            var unionCurrencyListAsString = unionCurrencyList.Select(x => x.ToString()).ToList();
            var exchangeRateEntities = await Storage.CurrencyExchangeRates
                .Where(
                    e =>
                        e.ExchangeProvider == ExchangeProviderType.Custom
                        &&
                        e.ExchangeDate == exchangeDate &&
                        unionCurrencyListAsString.Contains(e.FromCurrencyCode) &&
                        unionCurrencyListAsString.Contains(e.ToCurrencyCode) &&
                        e.OrganizationId == organizationId
                ).ToListAsync();
            if (!exchangeRateEntities.Any())
            {
                return;
            }


            foreach (var fromCurrency in unionCurrencyList)
            {
                foreach (var toCurrency in unionCurrencyList)
                {
                    if (rateList.Any(x => x.FromCurrency == fromCurrency && x.ToCurrency == toCurrency))
                    {
                        continue;
                    }

                    var rate = exchangeRateEntities.FirstOrDefault(
                        x => 
                        x.FromCurrencyCode == fromCurrency.ToString() && 
                        x.ToCurrencyCode == toCurrency.ToString());
                    if (rate == null)
                    {
                        continue;
                    }
                    var surcharge = GetSurchargeAsPercent(currencyConfigList, rate.FromCurrencyCode.AsEnum<CurrencyType>(), rate.ToCurrencyCode.AsEnum<CurrencyType>());
                    var rateWithSurcharge = GetRateWithSurcharge(rate.Rate, surcharge);
                    rateList.Add(new RateResponse
                    {
                        FromCurrency = rate.FromCurrencyCode.AsEnum<CurrencyType>(),
                        ToCurrency = rate.ToCurrencyCode.AsEnum<CurrencyType>(),
                        ExchangeRate = rate.Rate,
                        ExchangeRateWithSurcharge = rateWithSurcharge,
                        ExchangeDate = exchangeDate,
                        ProvidedByOrganizationId = organizationId
                    });
                }
            }
        }


        private decimal GetSurchargeAsPercent(
            List<CurrencyExchangeConfigEntity> currencyConfigList,
            CurrencyType fromCurrency,
            CurrencyType toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return 0;
            }

            var config = currencyConfigList
                .Where(x => x.ToCurrencyCode.AsEnum<CurrencyType>() == fromCurrency ||
                            x.ToCurrencyCode.AsEnum<CurrencyType>() == toCurrency)
                .OrderByDescending(x => x.Surcharge)
                .First();
            return config.Surcharge;
        }

        private decimal GetRateWithSurcharge(decimal rate, decimal surcharge)
        {
            return (1 + surcharge / 100) * rate;
        }

        private void MakeCartesianProductFromCustomAndNotCustomRates(List<CurrencyExchangeConfigEntity> currencyConfigList, List<RateResponse> rateList,
            Guid organizationId)
        {
            var fromCurrencyList = currencyConfigList.Select(x => x.FromCurrency);
            var toCurrencyList = currencyConfigList.Select(x => x.ToCurrency);
            var unionCurrencyList = fromCurrencyList.Union(toCurrencyList).Distinct().ToList();
            var baseCurrency = currencyConfigList.First().FromCurrency;
            var converterInput = rateList.Where(x => x.FromCurrency == baseCurrency).Select(x =>
                new ExchangeConverterInput
                {
                    FromCurrency = x.FromCurrency,
                    ToCurrency = x.ToCurrency,
                    Rate = x.ExchangeRate,
                    ExchangeDate = x.ExchangeDate
                }).ToList();


            var exchangeCurrencyConverter = new ExchangeCurrencyConverter(baseCurrency, converterInput);
            foreach (var fromCurrency in unionCurrencyList)
            {
                foreach (var toCurrency in unionCurrencyList)
                {
                    if (rateList.Any(x => x.FromCurrency == fromCurrency && x.ToCurrency == toCurrency))
                    {
                        continue;
                    }

                    var rate = exchangeCurrencyConverter.From(fromCurrency).To(toCurrency);
                    var surcharge = GetSurchargeAsPercent(currencyConfigList, rate.FromCurrency, rate.ToCurrency);
                    var rateWithSurcharge = GetRateWithSurcharge(rate.Rate, surcharge);
                    rateList.Add(new RateResponse
                    {
                        FromCurrency = rate.FromCurrency,
                        ToCurrency = rate.ToCurrency,
                        ExchangeRate = rate.Rate,
                        ExchangeRateWithSurcharge = rateWithSurcharge,
                        ExchangeDate = rate.ExchangeDate,
                        ProvidedByOrganizationId = organizationId
                    });
                }
            }
        }
    }
}
