using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.DAL;
using PaymentMS.DAL.CloudServices;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class KeyVaultStorageTests : BaseServiceTests<IKeyVaultStorage>
    {
        [TestMethod]
        public async Task SuccessCreateReadUpdate()
        {
            var random = new Random().Next(1, 1000);
            var key = "unit-test-key" + random;
            var value = "unit-test-value" + random;
            await Service.AddOrUpdateSecretAsync(key, value);

            var valueGotByName = await Service.GetSecretByNameAsync(key);
            Assert.IsTrue(valueGotByName == value);

            var newValue = value + "-" + random;
            await Service.AddOrUpdateSecretAsync(key, newValue);

            valueGotByName = await Service.GetSecretByNameAsync(key);
            Assert.IsTrue(valueGotByName == newValue);

            await Service.DeleteSecretAsync(key);
        }
    }
}
