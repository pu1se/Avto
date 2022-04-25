using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.Api.Controllers;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;

namespace PaymentMS.UI.Front.ViewModels
{
    public class CurrencyConfigListPageModel : BasePageModel
    {
        public List<ConfigViewModel> CurrencyConfigs { get; set; }
    }
}
