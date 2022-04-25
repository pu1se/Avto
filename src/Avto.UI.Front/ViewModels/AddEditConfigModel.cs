using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avto.UI.Front.ViewModels
{
    public class AddEditConfigModel
    {
        public Guid OrganizationId { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public string RateSourceProvider { get; set; }

        public decimal? CustomRate { get; set; }

        public decimal SurchargeAsPercent { get; set; }

        public decimal CurrentRate { get; set; }

        public decimal CurrentRateWithSurcharge => (1 + SurchargeAsPercent / 100)
                                                   * CurrentRate;
    }
}
