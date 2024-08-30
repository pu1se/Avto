using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.Parser._Core.CallResults
{
    public interface IRequestModelWithValidation
    {
        Dictionary<string, List<string>> GetValidationErrors();
    }
}
