using System;
using System.Collections.Generic;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL
{
    public static class Extensions
    {
        public static bool IsCustom(this ExchangeProviderType exchangeProvider)
        {
            return exchangeProvider == ExchangeProviderType.Custom;
        }

        public static bool IsCustom(this ExchangeProviderType? exchangeProvider)
        {
            if (exchangeProvider == null)
            {
                return false;
            }

            return exchangeProvider == ExchangeProviderType.Custom;
        }

        public static bool IsCustomProvider(this string exchangeProvider)
        {
            if (exchangeProvider.IsNullOrEmpty())
            {
                return false;
            }

            return exchangeProvider == ExchangeProviderType.Custom.ToString();
        }
    }
}
