using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.DAL.Entities
{
    [Table("Currencies")]
    public class CurrencyEntity : IEntityWithCodeId
    {
        [Key]
        [Required]
        [MaxLength(8)]
        public string Code { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime LastUpdatedDateUtc { get; set; }

        public CurrencyType AsEnum()
        {
            return EnumHelper.Parse<CurrencyType>(Code);
        }
    }
}
