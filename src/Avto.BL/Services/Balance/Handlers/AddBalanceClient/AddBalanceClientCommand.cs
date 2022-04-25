using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Balance.Models
{
    public class AddBalanceClientCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid ProviderOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid ClientOrganizationId { get; set; }
    }
}
