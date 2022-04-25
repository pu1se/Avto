using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedConfiguredByOrganizationExchangeRates
{
    public class GetCalculatedConfiguredByOrganizationExchangeRatesQuery : Query
    {
        [NotEmptyValueRequired]
        public DateTime ExchangeDate { get; set; }

        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }
    }
}
