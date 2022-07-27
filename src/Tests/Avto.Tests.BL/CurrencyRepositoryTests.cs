using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Avto.DAL.Entities;
using Avto.Tests.BL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avto.Tests.BL
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
            var createdCurrencyEntry = Storage.Currencies.Add(creatingCurrency);
            await Storage.SaveChangesAsync();
            var createdCurrency = createdCurrencyEntry.Entity;
            Assert.IsTrue(createdCurrency.Code.IsNullOrEmpty() == false);
            Assert.IsTrue(createdCurrency.Name.IsNullOrEmpty() == false);
            Assert.IsTrue(creatingCurrency.Code == createdCurrency.Code);
            Assert.IsTrue(creatingCurrency.Name == createdCurrency.Name);

            var currencyList = await Storage.Currencies
                    .Where(e => e.Name == creatingCurrency.Name)
                    .ToListAsync();

            Assert.IsTrue(currencyList != null);
            Assert.IsTrue(currencyList.Count > 0);
            Assert.IsTrue(currencyList.First().Name.IsNullOrEmpty() == false);

            var currencyFromDB = await Storage.Currencies.FirstOrDefaultAsync(organization => organization.Code == createdCurrency.Code);


            Assert.IsTrue(currencyFromDB.Code.IsNullOrEmpty() == false);
            Assert.IsTrue(currencyFromDB.Name.IsNullOrEmpty() == false);

            Assert.IsTrue(currencyFromDB.Code == createdCurrency.Code);
            Assert.IsTrue(createdCurrency.Name == currencyFromDB.Name);
            Assert.IsTrue(createdCurrency.CreatedDateUtc == currencyFromDB.CreatedDateUtc);
            Assert.IsTrue(createdCurrency.LastUpdatedDateUtc == currencyFromDB.LastUpdatedDateUtc);


            Storage.Currencies.Remove(createdCurrency);
            await Storage.SaveChangesAsync();

            currencyFromDB = await Storage.Currencies.FirstOrDefaultAsync(currency => currency.Code == createdCurrency.Code);

            Assert.IsTrue(currencyFromDB == null);
        }
    }
}
