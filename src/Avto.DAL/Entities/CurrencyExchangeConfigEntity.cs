using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.DAL.Entities
{
    [Table("CurrencyExchangeConfigs")]
    public class CurrencyExchangeConfigEntity : IEntityWithTrackedDates
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

        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get; set; }

        public ExchangeProviderType ExchangeProvider { get; set; }

        public decimal Surcharge { get; set; }

        public decimal? CustomRate { get; set; }


        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }
    }
}
