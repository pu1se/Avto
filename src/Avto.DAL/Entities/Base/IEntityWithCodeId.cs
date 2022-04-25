using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.DAL.Entities
{
    public interface IEntityWithCodeId : IEntityWithTrackedDates
    {
        string Code { get; set; }
    }
}
