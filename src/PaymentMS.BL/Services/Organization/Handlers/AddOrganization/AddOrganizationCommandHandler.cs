using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Organization.Handlers.Commands.AddOrganization
{
    public class AddOrganizationCommandHandler 
        : CommandHandler<AddOrganizationCommand, CallResult>
    {
        public AddOrganizationCommandHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallResult> HandleCommandAsync(AddOrganizationCommand command)
        {
            var isOrganizationExists = await Storage.Organizations
                .ExistsAsync(e => e.Id == command.OrganizationId);
            if (isOrganizationExists)
            {
                return SuccessResult();
            }

            await Storage.Organizations.AddAsync(command.ToEntity());

            return SuccessResult();
        }
    }
}
