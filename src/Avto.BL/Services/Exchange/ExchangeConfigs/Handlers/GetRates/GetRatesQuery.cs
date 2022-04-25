using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization
{
    public class GetRatesQuery : Query
    {
        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }
    }
}
