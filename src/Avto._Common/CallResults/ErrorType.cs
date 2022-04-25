using System;
using System.Collections.Generic;
using System.Text;

namespace Avto
{
    public enum ErrorType
    {
        NotError = 0,
        ValidationError400,
        NotFoundError404,
        ModelWasNotInitialized400,
        UnexpectedError500,
        ThirdPartyApiError417
    }
}
