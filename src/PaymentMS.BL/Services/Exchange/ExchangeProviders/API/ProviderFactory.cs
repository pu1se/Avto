using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.BL.Services.Exchange.Api.CurrencyLayer;
using PaymentMS.DAL.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.API
{
    public class ProviderFactory
    {
        private IServiceProvider DependencyContainer { get; }

        public ProviderFactory(IServiceProvider dependencyContainer)
        {
            DependencyContainer = dependencyContainer;
        }

        public BaseProviderApi GetProvider(ExchangeProviderType providerType)
        {
            switch (providerType)
            {
                case ExchangeProviderType.ECB:
                    return DependencyContainer.GetRequiredService<EcbProviderApi>();
                case ExchangeProviderType.CurrencyLayer:
                    return DependencyContainer.GetRequiredService <CurrencyLayerProviderApi>();
                case ExchangeProviderType.Custom:
                    return DependencyContainer.GetRequiredService<CustomProviderApi>();
            }
            throw new NotImplementedException("Requested provider wasn't implemented.");
        }

        public List<ExchangeProviderType> GetSupportedProviders()
        {
            return EnumHelper.ToList<ExchangeProviderType>();
        }
    }
}
