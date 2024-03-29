﻿using System.Collections.Generic;

namespace Avto.BL
{
    public class EmptyCommand : Command
    {
        public static EmptyCommand Value => new EmptyCommand();
        public override Dictionary<string, List<string>> GetValidationErrors()
        {
            return new Dictionary<string, List<string>>();
        }
    }
}
