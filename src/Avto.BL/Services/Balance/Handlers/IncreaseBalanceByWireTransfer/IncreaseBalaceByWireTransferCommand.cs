using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Avto.BL.Services.Balance.Models
{
    public class IncreaseBalaceByWireTransferCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid ProviderOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid ClientOrganizationId { get; set; }

        [Range(1, Double.MaxValue)]
        public decimal AddAmount { get; set; }
    }
}
