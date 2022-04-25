using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Avto.BL.Services.Balance.Models
{
    public class SendBalancePaymentCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid ProviderOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid ClientOrganizationId { get; set; }

        [Range(1, Double.MaxValue)]
        public decimal PaymentAmount { get; set; }

        public Guid ExternalId { get; set; }

        public Dictionary<string, dynamic> ExternalMetadata { get; set; } = new Dictionary<string, dynamic>();

        [MaxLength(2048)]
        public string Description { get; set; }
    }
}
