using System;
using System.Collections.Generic;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders
{
    public class GetProvidersQuery : Query
    {
        public CurrencyType? FilterByCurrency { get; set; }
    }
}
