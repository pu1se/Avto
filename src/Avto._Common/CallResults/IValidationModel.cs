﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Avto
{
    public interface IValidationModel
    {
        Dictionary<string, List<string>> GetValidationErrors();
    }
}
