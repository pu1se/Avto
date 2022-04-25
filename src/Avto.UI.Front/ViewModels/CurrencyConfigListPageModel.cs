using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.Api.Controllers;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;

namespace Avto.UI.Front.ViewModels
{
    public class CurrencyConfigListPageModel : BasePageModel
    {
        public List<ConfigViewModel> CurrencyConfigs { get; set; }
    }
}
