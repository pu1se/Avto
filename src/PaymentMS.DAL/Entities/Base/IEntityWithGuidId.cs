using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.DAL.Entities
{
    public interface IEntityWithGuidId : IEntityWithTrackedDates
    {
        Guid Id { get; set; }        
    }
}
