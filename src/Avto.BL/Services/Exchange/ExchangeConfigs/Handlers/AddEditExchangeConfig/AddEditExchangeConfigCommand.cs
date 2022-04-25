using System;
using System.ComponentModel.DataAnnotations;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.AddEditExchangeConfig
{
    public class AddEditExchangeConfigCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }

        [Required]
        public CurrencyType? FromCurrency { get; set; }

        [Required]
        public CurrencyType? ToCurrency { get; set; }

        [Required]
        public ExchangeProviderType? RateSourceProvider { get; set; }

        public decimal? CustomRate { get; set; }

        [Range(0, 100)]
        public decimal SurchargeAsPercent { get; set; }
    }
}