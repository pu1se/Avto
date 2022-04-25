using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class SendBalancePaymentViewModel : BaseViewModel
    {
        public List<BalanceProviderOrganizationModel> ReceiverList { get; set; }
        public Guid SelectedProviderId { get; set; }
        public Guid SelectedClientId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
