using System;
using System.Linq.Expressions;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs
{
    public class OrganizationExchangeConfigResponse
    {
        public Guid OrganizationId { get; set; }
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public string RateSourceProvider { get; set; }
        public decimal SurchargeAsPercent { get; set; }
        public decimal? CustomRate { get; set; }


        public static Expression<Func<CurrencyExchangeConfigEntity, OrganizationExchangeConfigResponse>> Map()
        {
            return e => new OrganizationExchangeConfigResponse
            {
                FromCurrency = e.FromCurrencyCode.AsEnum<CurrencyType>(),
                ToCurrency = e.ToCurrencyCode.AsEnum<CurrencyType>(),
                RateSourceProvider = e.ExchangeProvider.ToString(),
                OrganizationId = e.OrganizationId,
                CustomRate = e.CustomRate,
                SurchargeAsPercent = e.Surcharge
            };
        }
    }
}
