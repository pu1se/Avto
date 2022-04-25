using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentMS.UI.Web.Models
{
    public class AddCardModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }
    }
}
