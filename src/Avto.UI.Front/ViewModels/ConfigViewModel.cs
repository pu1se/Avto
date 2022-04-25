using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.DAL.Enums;

namespace Avto.UI.Front.ViewModels
{
    public class ConfigViewModel
    {
        public Guid OrganizationId { get; set; }
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public string RateSourceProvider { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal SurchargeAsPercent { get; set; }
        public decimal RateWithSurcharge  => (1 + SurchargeAsPercent / 100) * ExchangeRate;
        public decimal? CustomRate { get; set; }
    }
}
