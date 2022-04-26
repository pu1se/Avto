using System;

namespace Avto.BL
{
    public class ThirdPartyApiException : Exception
    {
        public ThirdPartyApiException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
