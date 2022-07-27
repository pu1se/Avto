using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.System.SendSystemWarningEmail
{
    public class SendSystemWarningEmailCommand : Command
    {
        public string Message { get; set; }
    }
}
