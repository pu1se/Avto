using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.Parser._Core.CallResults
{
    public enum ErrorType
    {
        NotError = 0,
        ValidationError400,
        UnauthorizedError401,
        AccessDenied403,
        NotFoundError404,
        WrongHttpMethodError405,
        ModelWasNotInitialized400,
        UnexpectedError500,
        ThirdPartyApiError417,
        CanNotFindParameters415,
        ApiIsNotAvailable,
        CacheIsNotAvailable,
    }
}
