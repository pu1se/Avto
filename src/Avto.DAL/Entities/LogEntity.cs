using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Avto.DAL.Entities
{
    [Table("_Logs")]
    public class LogEntity : IEntityWithGuidId
    {
        public Guid Id { get; set; }

        [MaxLength(8192)]
        public string Logs { get; set; }

        [MaxLength(1024)]
        public string PathToAction { get; set; }

        [MaxLength(32)]
        public string HttpMethod { get; set; }

        public int ResponseCode { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }

        public long ExecutionTimeInMillSec { get; set; }
    }
}
