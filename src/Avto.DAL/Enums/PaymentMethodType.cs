using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Avto.DAL.Enums
{
    public enum PaymentMethodType
    {
        Unknown = 0,

        [Description("Payed by Card")]
        StripeCard = 1,

        [Description("Payed by Balance")]
        Balance,

        [Description("Increased by Wire Transfer")]
        WireTransferToBalance,

        [Description("Increased by Card")]
        StripeCardToBalance
    }
}
