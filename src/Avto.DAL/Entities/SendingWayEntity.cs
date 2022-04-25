using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.DAL.Entities
{
    [Table("PaymentSendingWays")]
    public class SendingWayEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(4096)]
        public string Configuration { get; set; }

        public bool IsDefault { get; set; }
        
        public DateTime CreatedDateUtc { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }



        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get;set; }

        public Guid ReceivingWayId { get; set; }
        [ForeignKey("ReceivingWayId")]
        public virtual ReceivingWayEntity ReceivingWay { get; set; }

        public virtual ICollection<PaymentEntity> Payments { get; set; }
    }
}
