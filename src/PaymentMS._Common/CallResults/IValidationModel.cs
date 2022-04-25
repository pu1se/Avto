using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS
{
    public interface IValidationModel
    {
        Dictionary<string, List<string>> GetValidationErrors();
    }
}
