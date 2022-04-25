using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;
using PaymentMS.DAL.Enums;

namespace PaymentMS.DAL.Entities
{
    [Table("PaymentReceivingWays")]
    public class ReceivingWayEntity : IEntityWithGuidId
    {
        [Key]
        public Guid Id { get; set; }

        public PaymentMethodType PaymentMethod { get; set; } = PaymentMethodType.StripeCard;

        //implement later: when we will have more then one payment provider,
        //return factory that will convert configuration 
        //to specific configuration
        public string Configuration { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }
        
        [NotMapped]
        public StripeConfigForKeyVault StripePrivateConfig { get; set; } = new StripeConfigForKeyVault();

        private StripeConfigForDB _stripePublicConfig;
        [NotMapped]
        public StripeConfigForDB StripePublicConfig
        {
            get
            {
                if (_stripePublicConfig == null)
                {
                    _stripePublicConfig = new StripeConfigForDB();
                    if (!Configuration.IsNullOrEmpty() 
                        && 
                        PaymentMethod == PaymentMethodType.StripeCard)
                    {
                        _stripePublicConfig = JsonConvert.DeserializeObject<StripeConfigForDB>(Configuration);
                    }
                }
                return _stripePublicConfig;
            }
            set
            {
                _stripePublicConfig = value;
                Configuration = _stripePublicConfig.ToJson();
            }
        }        



        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual OrganizationEntity Organization { get;set; }

        public virtual ICollection<SendingWayEntity> SendingWays { get; set; }

        public Guid? BalanceProviderId { get; set; }
        [ForeignKey("BalanceProviderId")]
        public virtual BalanceProviderEntity BalanceProvider { get; set; }
    }
}
