using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PaymentMS.DAL.Entities
{
    public class StripeConfigForKeyVault
    {
        public string PublicKey { get; set; }

        public string SecretKey { get; set; }
    }
}
