using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Repositories;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class CurrencyRepositoryTests : BaseServiceTests<Storage>
    {
        [TestMethod]
        public async Task SuccessfullyCreateGetFilterDeleteCurrency()
        {
            var creatingCurrency = new CurrencyEntity()
            {
                Code = "UnitTest",
                Name = "Currency Unit Test",
            };
            var createdCurrency = await Storage.Currencies.AddAsync(creatingCurrency);

            Assert.IsTrue(createdCurrency.Code.IsNullOrEmpty() == false);
            Assert.IsTrue(createdCurrency.Name.IsNullOrEmpty() == false);
            Assert.IsTrue(createdCurrency.CreatedDateUtc.Date == DateTime.UtcNow.Date);
            Assert.IsTrue(createdCurrency.LastUpdatedDateUtc.Date == DateTime.UtcNow.Date);
            Assert.IsTrue(creatingCurrency.Code == createdCurrency.Code);
            Assert.IsTrue(creatingCurrency.Name == createdCurrency.Name);

            var currencyList = await Storage.Currencies
                    .Where(e => e.Name == creatingCurrency.Name)
                    .ToListAsync();

            Assert.IsTrue(currencyList != null);
            Assert.IsTrue(currencyList.Count > 0);
            Assert.IsTrue(currencyList.First().Name.IsNullOrEmpty() == false);

            var currencyFromDB = await Storage.Currencies.GetAsync(organization => organization.Code == createdCurrency.Code);


            Assert.IsTrue(currencyFromDB.Code.IsNullOrEmpty() == false);
            Assert.IsTrue(currencyFromDB.Name.IsNullOrEmpty() == false);

            Assert.IsTrue(currencyFromDB.Code == createdCurrency.Code);
            Assert.IsTrue(createdCurrency.Name == currencyFromDB.Name);
            Assert.IsTrue(createdCurrency.CreatedDateUtc == currencyFromDB.CreatedDateUtc);
            Assert.IsTrue(createdCurrency.LastUpdatedDateUtc == currencyFromDB.LastUpdatedDateUtc);


            await Storage.Currencies.DeleteAsync(createdCurrency);

            currencyFromDB = await Storage.Currencies.GetAsync(organization => organization.Code == createdCurrency.Code);

            Assert.IsTrue(currencyFromDB == null);
        }
    }
}
