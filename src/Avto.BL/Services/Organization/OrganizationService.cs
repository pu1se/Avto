﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Organization.Handlers.Commands.AddOrganization;
using Avto.BL.Services.Organization.Handlers.Queries.Organization;
using Avto.DAL.Entities;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Organization
{    
    public class OrganizationService : BaseService
    {
        public OrganizationService(
            Storage storage, 
            IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallResult> AddOrganization(AddOrganizationCommand command)
        {
            return GetHandler<AddOrganizationCommandHandler>().HandleAsync(command);
        }

        public Task<CallDataResult<GetOrganizationQueryResponse>> GetOrganization(Guid organizationId)
        {
            var query = new GetOrganizationQuery{OrganizationId = organizationId};
            return GetHandler<GetOrganizationQueryHandler>().HandleAsync(query);
        }        
    }
}