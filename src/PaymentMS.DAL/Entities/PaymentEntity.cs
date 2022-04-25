using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PaymentMS.DAL.Enums;

namespace PaymentMS.DAL.Entities
{
    [Table("Payments")]
    public class PaymentEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        public decimal PaymentAmount { get; set; }

        [MaxLength(8)]
        public string PaymentCurrency { get; set; }

        public PaymentStatusType Status { get; set; }

        public PaymentMethodType PaymentMethod { get; set; }

        public Guid ExternalId { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [MaxLength(8192)]
        public string TransactionLog { get; set; }

        [MaxLength(2048)]
        public string ExternalMetadata { get; set; }
        
        public DateTime CreatedDateUtc { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }



        public Guid? SendingWayId { get; set; }
        [ForeignKey("SendingWayId")]
        public virtual SendingWayEntity SendingWay { get; set; }

        public Guid? BalanceClientId { get; set; }
        [ForeignKey("BalanceClientId")]
        public virtual BalanceClientEntity BalanceClient { get;set; }
    }
}
