using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Avto.BL.Services.Organization;
using Avto.BL.Services.Organization.Handlers.Commands.AddOrganization;
using Avto.Tests.BL.Base;

namespace Avto.Tests.BL
{
    [TestClass]
    public class OrganizationServiceTests : BaseServiceTests<OrganizationService>
    {
        [TestMethod]
        public async Task SuccessCreateOrganization()
        {
            var creatingOrganization = new AddOrganizationCommand
            {
                OrganizationId = Guid.NewGuid(),
                OrganizationName = "unit test organization"
            };

            var createResult = await Service.AddOrganization(creatingOrganization);
            Assert.IsTrue(createResult.IsSuccess);
            Assert.IsTrue(createResult.ErrorType == ErrorType.NotError);

            var getOrganizationResult = await Service.GetOrganization(creatingOrganization.OrganizationId);
            Assert.IsTrue(getOrganizationResult.IsSuccess);
            Assert.IsTrue(getOrganizationResult.Data != null);

            var createdOrganization = getOrganizationResult.Data;
            Assert.IsTrue(createdOrganization.OrganizationId == creatingOrganization.OrganizationId);
            Assert.IsTrue(createdOrganization.OrganizationName == creatingOrganization.OrganizationName);

            await Storage.Organizations.DeleteAsync(creatingOrganization.OrganizationId);
        }

        public void FailCreateOrganization()
        {

        }
    }
}
