using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Avto.DAL.Entities
{
    [Table("BalanceClients")]
    public class BalanceClientEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CreatedDateUtc { get; set; }

        [Required]
        public DateTime LastUpdatedDateUtc { get; set; }



        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get; set; }

        public Guid BalanceProviderId { get; set; }
        [ForeignKey("BalanceProviderId")]
        public virtual BalanceProviderEntity BalanceProvider { get;set; }

        public virtual ICollection<PaymentEntity> Payments { get; set; }
    }
}
