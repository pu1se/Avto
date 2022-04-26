using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.DAL.Entities
{
    [Table("CurrencyExchangeRates")]
    public class ExchangeRateEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        public string FromCurrencyCode { get; set; }
        [ForeignKey("FromCurrencyCode")]
        public virtual CurrencyEntity FromCurrencyEntity { get; set; }

        public string ToCurrencyCode { get; set; }
        [ForeignKey("ToCurrencyCode")]
        public virtual CurrencyEntity ToCurrencyEntity { get; set; }

        // Close day rate.
        public decimal Rate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ExchangeDate { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }

        public decimal OpenDayRate { get; set; }
        public decimal MinDayRate { get; set; }
        public decimal MaxDayRate { get; set; }
    }
}