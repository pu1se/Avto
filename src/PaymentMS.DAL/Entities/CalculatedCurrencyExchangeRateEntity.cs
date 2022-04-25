using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PaymentMS.DAL.Enums;

namespace PaymentMS.DAL.Entities
{
    [Table("CalculatedCurrencyExchangeRates")]
    public class CalculatedCurrencyExchangeRateEntity : IEntityWithGuidId
    {
        public string FromCurrencyCode { get; set; }
        [ForeignKey("FromCurrencyCode")]
        public virtual CurrencyEntity FromCurrencyEntity { get; set; }
        [NotMapped]
        public CurrencyType FromCurrency
        {
            get => EnumHelper.Parse<CurrencyType>(FromCurrencyCode);
            set => FromCurrencyCode = value.ToString();
        }


        public string ToCurrencyCode { get; set; }
        [ForeignKey("ToCurrencyCode")]
        public virtual CurrencyEntity ToCurrencyEntity { get; set; }
        [NotMapped]
        public CurrencyType ToCurrency
        {
            get => EnumHelper.Parse<CurrencyType>(ToCurrencyCode);
            set => ToCurrencyCode = value.ToString();
        }

        public Guid? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get; set; }

        public decimal ExchangeRate { get; set; }

        [Column(TypeName="Date")]
        public DateTime ExchangeDate { get; set; }


        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }

        [Key]
        public Guid Id { get; set; }
    }
}
