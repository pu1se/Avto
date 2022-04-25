using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class BalanceClientListViewModel : BaseViewModel
    {
        public Guid ProviderOrganizationId { get; set; }

        public Guid SelectedClientId { get; set; }

        public List<BalanceClientModel> ClientList { get; set; } = new List<BalanceClientModel>();
    }
}
