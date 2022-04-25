using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PaymentMS.DAL.Entities
{
    [Table("Organizations")]
    public class OrganizationEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(512)]
        public string Name { get; set; }
        
        public DateTime CreatedDateUtc { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }



        public virtual ICollection<SendingWayEntity> SendingWays { get;set; } = new List<SendingWayEntity>();

        public virtual ICollection<ReceivingWayEntity> ReceivingWays { get; set; } = new List<ReceivingWayEntity>();
        
        public virtual ICollection<BalanceProviderEntity> BalanceProviders { get; set; } = new List<BalanceProviderEntity>();

        public virtual ICollection<BalanceClientEntity> BalanceClients { get; set; } = new List<BalanceClientEntity>();

        public virtual ICollection<CalculatedCurrencyExchangeRateEntity> CalculatedCurrencyExchangeRates { get; set; } = new List<CalculatedCurrencyExchangeRateEntity>();
    }
}
