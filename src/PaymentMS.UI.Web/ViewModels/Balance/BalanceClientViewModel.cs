using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class BalanceClientViewModel : BaseViewModel
    {
        public Guid ProviderOrganizationId { get; set; }

        public Guid ClientOrganizationId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public decimal CreditLimit { get; set; }
    }
}
