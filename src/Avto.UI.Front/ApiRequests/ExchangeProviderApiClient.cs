﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.Api.Controllers;
using Avto.BL.Services.Balance.Models;
using Avto.BL.Services.Currency.ResponseModels;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders;
using Avto.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates;
using Avto.DAL;
using Avto.DAL.Enums;
using Avto.UI.Front.ViewModels;

namespace Avto.UI.Front.ApiRequests
{
    public class ExchangeProviderApiClient : BaseApiClient
    {
        public ExchangeProviderApiClient(UiAppSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<AvailableCurrencyResponse>>> GetAvailableCurrenciesAsync()
        {
            return Api.GetAsync<List<AvailableCurrencyResponse>>("exchange/providers/currencies");
        }

        public Task<ApiCallDataResult<List<ExchangeProvidersResponse>>> GetProvidersAsync()
        {
            return Api.GetAsync<List<ExchangeProvidersResponse>>(
                $"exchange/providers"
            );
        }

        public async Task<ApiCallDataResult<TodayProviderExchangeRateResponse>> GetRateFromProviderAsync(GetTodayRateFromProviderQuery query)
        {
            var result = await Api.PostAsync<TodayProviderExchangeRateResponse>(
                $"exchange/providers/rate",
                query
            );
            return result;
        }

        public Task<ApiCallResult> RefreshRatesForAllProvidersAsync()
        {
            return Api.PostAsync(
                $"exchange/providers/rates/refresh"
            );
        }

        public Task<ApiCallResult> RefreshRatesForProviderAsync(ExchangeProviderType provider)
        {
            return Api.PostAsync(
                $"exchange/providers/{provider}/rates/refresh"
            );
        }
    }
}