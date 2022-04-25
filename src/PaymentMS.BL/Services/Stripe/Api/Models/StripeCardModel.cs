using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Stripe.Models
{
    public class StripeCardModel
    {
        public string CardId { get; set; }
        public string CustomerId { get; set; }
        public string Last4CardDigits { get;set; }
        public string CardBrand { get;set; }
        public string CardCountryCode { get; set; }
        public string CardType { get;set; }
        public long ExpireMonth { get; set; }
        public long ExpireYear { get; set; }
    }

    public static class MappingHelper{

        public static StripeCardModel ToStripeCard(this string json)
        {
            if (json.IsNullOrEmpty())
                return new StripeCardModel();

            return JsonConvert.DeserializeObject<StripeCardModel>(json);
        }

    }
}
