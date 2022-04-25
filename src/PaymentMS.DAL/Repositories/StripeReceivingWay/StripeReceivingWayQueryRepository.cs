using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentMS.DAL.CloudServices;
using PaymentMS.DAL.Entities;

namespace PaymentMS.DAL.Repositories
{
    public class StripeReceivingWayQueryRepository : QueryRepository<ReceivingWayEntity>
    {
        private IKeyVaultStorage KeyVaultStorage { get; }

        public StripeReceivingWayQueryRepository(DataContext context, IKeyVaultStorage keyVaultStorage, bool useNoTracking = false) : base(context, useNoTracking)
        {
            KeyVaultStorage = keyVaultStorage;
        }

        public override async Task<ReceivingWayEntity> GetAsync(Expression<Func<ReceivingWayEntity, bool>> filter = null)
        {
            var entity = await base.GetAsync(filter);
            if (entity == null)
                return null;

            var jsonConfiguration = await KeyVaultStorage.GetSecretByNameAsync(entity.Id.ToKey());
            entity.StripePrivateConfig = JsonConvert.DeserializeObject<StripeConfigForKeyVault>(jsonConfiguration)
                                         ?? new StripeConfigForKeyVault();
            return entity;
        }

        public override async Task<List<ReceivingWayEntity>> ToListAsync()
        {
            var list = await base.ToListAsync();
            var taskList = new List<Task>();

            foreach (var entity in list)
            {
                var task = Task.Run(async () =>
                {
                    var jsonConfiguration = await KeyVaultStorage.GetSecretByNameAsync(entity.Id.ToKey());
                    entity.StripePrivateConfig = JsonConvert.DeserializeObject<StripeConfigForKeyVault>(jsonConfiguration)
                                                 ?? new StripeConfigForKeyVault();
                });
                taskList.Add(task);
            }

            await Task.WhenAll(taskList);

            return list;
        }  
    }
}
