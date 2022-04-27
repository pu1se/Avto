using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Exchange.GetExchangeRates
{
    public class GetSpecificExchangeRatesInfoQuery : Query
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public int PeriodInDays { get; set; }
    }
}
