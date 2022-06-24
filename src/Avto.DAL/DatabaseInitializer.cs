using System.Linq;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Avto.DAL
{
    public static class DatabaseInitializer
    {        
        public static void SeedWithTestData(Storage storage)
        {
            foreach (var currencyItem in EnumHelper.ToList<CurrencyType>())
            {
                var isExists = storage.Currencies.Any(e => e.Code == currencyItem.ToString());

                if (!isExists)
                {
                    storage.Currencies.Add(new CurrencyEntity
                    {
                        Code = currencyItem.ToString(),
                        Name = EnumHelper.GetDescription(currencyItem),
                    });
                }
            }

            storage.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
