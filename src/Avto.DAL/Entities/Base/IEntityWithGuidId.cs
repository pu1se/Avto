using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.DAL.Entities
{
    public interface IEntityWithGuidId : IEntityWithTrackedDates
    {
        Guid Id { get; set; }        
    }
}
