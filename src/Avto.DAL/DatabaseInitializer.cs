using System.Linq;
using Avto.DAL.Entities;
using Avto.DAL.Enums;

namespace Avto.DAL
{
    public static class DatabaseInitializer
    {        
        public static void SeedWithTestData(Storage storage)
        {
            var isExists = storage.Currencies.Any();

            if (!isExists)
            {
                foreach (var currency in EnumHelper.ToList<CurrencyType>())
                {
                    storage.Currencies.Add(new CurrencyEntity
                    {
                        Code = currency.ToString(),
                        Name = EnumHelper.GetDescription(currency),
                    });   
                }
            }

            storage.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
