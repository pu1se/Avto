using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Avto.BL.Services.Organization;
using Avto.BL.Services.Organization.Handlers.Commands.AddOrganization;

namespace Avto.Api.Controllers
{
    public class OrganizationController : BaseApiController
    {
        private OrganizationService OrganizationService { get; }

        public OrganizationController(OrganizationService organizationService)
        {
            OrganizationService = organizationService;
        }


        [Route("organizations/{organizationId:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetOrganization(Guid organizationId)
        {
            return await HttpResponse(() => OrganizationService.GetOrganization(organizationId));
        }

        [Route("organizations")]
        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody]AddOrganizationCommand command)
        {
            return await HttpResponse(() => OrganizationService.AddOrganization(command));
        }        
    }
}
