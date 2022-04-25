using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL._Core.Logger
{
    public class LogHttpInfo
    {
        public string PathToAction { get; set; }
        public int ResponseCode { get; set; }
        public string HttpMethod { get; set; }
        public string HttpResponseAsJson { get; set; }
        public long ExecutionTimeInMilliSec { get; set; }
    }
}
