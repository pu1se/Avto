﻿using System;
using System.ComponentModel.DataAnnotations;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.DeleteExchangeConfig
{
    public class DeleteExchangeConfigCommand : Command
    {
        [Required]
        public CurrencyType? FromCurrency { get; set; }

        [Required]
        public CurrencyType? ToCurrency { get; set; }
        
        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }
    }
}