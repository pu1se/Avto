using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates
{
    public class GetTodayRateFromProviderQuery : Query
    {
        [Required]
        public ExchangeProviderType? Provider { get; set; }

        [Required]
        public CurrencyType? FromCurrency { get; set; }

        [Required]
        public CurrencyType? ToCurrency { get; set; }

        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }
    }
}
