using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.DAL.Entities
{
    public interface IEntityWithCodeId : IEntityWithTrackedDates
    {
        string Code { get; set; }
    }
}
