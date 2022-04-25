using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avto.UI.Front.ViewModels
{
    public class BasePageModel
    {
        public string ErrorMessage { get; set; }
        public string LogMessage { get; set; }
        public bool IsActionSuccess { get; set; }
        public bool IsReady { get; set; }
    }
}
