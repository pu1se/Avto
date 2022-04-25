using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.DAL.Entities
{
    [Table("CurrencyExchangeRates")]
    public class CurrencyExchangeRateEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        public string FromCurrencyCode { get; set; }
        [ForeignKey("FromCurrencyCode")]
        public virtual CurrencyEntity FromCurrencyEntity { get; set; }

        public string ToCurrencyCode { get; set; }
        [ForeignKey("ToCurrencyCode")]
        public virtual CurrencyEntity ToCurrencyEntity { get; set; }

        public decimal Rate { get; set; }

        public DateTime ExchangeDate { get; set; }

        public ExchangeProviderType ExchangeProvider { get; set; }

        public Guid? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }

        public decimal OpenDayRate { get; set; }
        public decimal MinDayRate { get; set; }
        public decimal MaxDayRate { get; set; }
        public bool HasExtraInfoForRate { get; set; }
    }
}