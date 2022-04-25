using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentMS.UI.Web.ViewModels
{
    public abstract class BaseViewModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
