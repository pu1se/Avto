using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders
{
    public class GetProvidersQuery : Query
    {
        public CurrencyType? FilterByCurrency { get; set; }
    }
}
