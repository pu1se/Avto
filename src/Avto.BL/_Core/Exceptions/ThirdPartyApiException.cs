using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL
{
    public class ThirdPartyApiException : Exception
    {
        public ThirdPartyApiException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
