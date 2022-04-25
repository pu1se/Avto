using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.DAL.Entities
{
    public interface IEntityWithTrackedDates
    {
        DateTime CreatedDateUtc { get; set; }
        DateTime LastUpdatedDateUtc { get; set; }
    }
}
