using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class HistoryTableViewModel : BaseViewModel
    {
        public List<BalancePaymentModel> Payments { get; set; } = new List<BalancePaymentModel>();
    }
}
