using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.DAL.Entities
{
    public interface IEntityWithTrackedDates
    {
        DateTime CreatedDateUtc { get; set; }
        DateTime LastUpdatedDateUtc { get; set; }
    }
}
