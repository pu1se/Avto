using System;
using System.Collections.Generic;
using System.Text;

namespace Avto
{
    public interface IRequestModelWithValidation
    {
        Dictionary<string, List<string>> GetValidationErrors();
    }
}
