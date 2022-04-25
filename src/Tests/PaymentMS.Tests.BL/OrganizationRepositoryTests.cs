using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.DAL;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class OrganizationRepositoryTests : BaseServiceTests<Storage>
    {
        [TestMethod]
        public async Task SuccessfullyCreateGetFilterDeleteOrganization()
        {
            var newOrganization = await Storage.Organizations.AddAsync(new OrganizationEntity()
            {
                Name = "Temp Test organization",
            });

            Assert.IsTrue(newOrganization.Id != Guid.Empty);
            Assert.IsTrue(newOrganization.Name.IsNullOrEmpty() == false);
            Assert.IsTrue(newOrganization.CreatedDateUtc.Date == DateTime.UtcNow.Date);
            Assert.IsTrue(newOrganization.LastUpdatedDateUtc.Date == DateTime.UtcNow.Date);

            var organizationList = await 
                Storage.Organizations
                    .Where(organization => organization.Name.Contains("Test organization"))
                    .ToListAsync();

            Assert.IsTrue(organizationList != null);
            Assert.IsTrue(organizationList.Count > 0);
            Assert.IsTrue(organizationList.First().Name.IsNullOrEmpty() == false);

            var organizationFromDB = await Storage.Organizations.GetAsync(organization => organization.Id == newOrganization.Id);


            Assert.IsTrue(organizationFromDB.Id != Guid.Empty);
            Assert.IsTrue(organizationFromDB.Name.IsNullOrEmpty() == false);

            Assert.IsTrue(organizationFromDB.Id == newOrganization.Id);
            Assert.IsTrue(newOrganization.Id == organizationFromDB.Id);
            Assert.IsTrue(newOrganization.Name == organizationFromDB.Name);
            Assert.IsTrue(newOrganization.CreatedDateUtc  == organizationFromDB.CreatedDateUtc);
            Assert.IsTrue(newOrganization.LastUpdatedDateUtc  == organizationFromDB.LastUpdatedDateUtc);


            await Storage.Organizations.DeleteAsync(newOrganization);

            organizationFromDB = await Storage.Organizations.GetAsync(organization => organization.Id == newOrganization.Id);

            Assert.IsTrue(organizationFromDB == null);
        }


        [TestMethod]
        public async Task SuccessUpdateOrganization()
        {
            var organizationForUpdate = await Storage.Organizations.GetAsync(organization => organization.Id == TestData.PaymentReceiverOrganizationId);

            Assert.IsTrue(organizationForUpdate != null);
            Assert.IsTrue(organizationForUpdate.Name.IsNullOrEmpty() == false);
            
            var organizationNameBackup = organizationForUpdate.Name;

            organizationForUpdate.Name += " some test value";
            Storage.Organizations.Update(organizationForUpdate);
            await Storage.SaveChangesAsync();

            var updatedOrganization = await Storage.Organizations.GetAsync(organization => organization.Id == organizationForUpdate.Id);

            Assert.IsTrue(updatedOrganization.Name == organizationForUpdate.Name);
            Assert.IsTrue(updatedOrganization.LastUpdatedDateUtc.Date == DateTime.UtcNow.Date);

            organizationForUpdate.Name = organizationNameBackup;
            Storage.Organizations.Update(organizationForUpdate);
            await Storage.SaveChangesAsync();
        }

        [TestMethod]
        public async Task SuccessTransactionRollback()
        {
            var oldPaymentStatus = PaymentStatusType.Fail;
            var oldAmount = (decimal)0;
            var oldCurrency = string.Empty;
            var oldPaymentId = Guid.Empty;
            using (var transaction = await Storage.BeginTransactionAsync())
            {
                try
                {
                    var payment = await Storage.Payments.GetAsync();
                    oldPaymentStatus = payment.Status;
                    oldAmount = payment.PaymentAmount;
                    oldCurrency = payment.PaymentCurrency;
                    oldPaymentId = payment.Id;


                    payment.Status = PaymentStatusType.Unknown;
                    payment.PaymentAmount = 0;
                    payment.PaymentCurrency = "";
                    await Storage.SaveChangesAsync();
                    
                    throw new Exception();
                }
                catch (Exception)
                {
                }
            }

            CleanStorageCache();
            var checkPayment = await Storage.Payments.GetAsync(e => e.Id == oldPaymentId);
            Assert.IsTrue(checkPayment.Status == oldPaymentStatus);
            Assert.IsTrue(checkPayment.PaymentAmount == oldAmount);
            Assert.IsTrue(checkPayment.PaymentCurrency == oldCurrency);
        }

        [TestMethod]
        public async Task SuccessTransactionCommit()
        {
            var newOrganization = new OrganizationEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test transaction"
            };
            using (var transaction = await Storage.BeginTransactionAsync())
            {
                await Storage.Organizations.AddAsync(newOrganization);
                transaction.Commit();
            }
            
            var savedOrganization = await Storage.Organizations.GetAsync(e => e.Id == newOrganization.Id);
            Assert.IsTrue(savedOrganization.Id == newOrganization.Id);
            Assert.IsTrue(savedOrganization.Name == newOrganization.Name);
            Assert.IsTrue(savedOrganization.CreatedDateUtc == newOrganization.CreatedDateUtc);
            Assert.IsTrue(savedOrganization.LastUpdatedDateUtc == newOrganization.LastUpdatedDateUtc);

            await Storage.Organizations.DeleteAsync(newOrganization.Id);
        }
    }
}
