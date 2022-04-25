using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PaymentMS.DAL.Entities
{
    [Table("BalanceProviders")]
    public class BalanceProviderEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        public decimal CreditLimit { get; set; }

        [MaxLength(8)]
        [Required]
        public string Currency { get; set; }

        public bool IsWireTransferIncomeEnabled { get; set; }

        public bool IsStripeCardIncomeEnabled { get; set; }

        [Required]
        public DateTime CreatedDateUtc { get; set; }

        [Required]
        public DateTime LastUpdatedDateUtc { get; set; }
        


        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get; set; }

        public virtual ICollection<ReceivingWayEntity> ReceivingWays { get; set; } = new List<ReceivingWayEntity>();
    }
}
