using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Organization.Handlers.Commands.AddOrganization
{
    public class AddOrganizationCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid OrganizationId { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        public OrganizationEntity ToEntity()
        {
            return new OrganizationEntity
            {
                Id = OrganizationId,
                Name = OrganizationName
            };
        }
    }
}
