using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Avto.DAL.CloudServices;
using Avto.DAL.Entities;

namespace Avto.DAL.Repositories
{    
    public class StripeReceivingWayCommandRepository : StripeReceivingWayQueryRepository, IGuidIdCommandRepository<ReceivingWayEntity>
    {
        private IKeyVaultStorage KeyVaultStorage { get; }
        private IGuidIdCommandRepository<ReceivingWayEntity> Repository { get; }

        public StripeReceivingWayCommandRepository(DataContext context, IKeyVaultStorage keyVaultStorage, bool isQuery) : base(context, keyVaultStorage, isQuery)
        {
            KeyVaultStorage = keyVaultStorage;
            Repository = new GuidIdCommandRepository<ReceivingWayEntity>(context, isQuery);
        }
        
        public void Add(ReceivingWayEntity entity)
        {
            throw new ArgumentException("Use AddAsync method instead of Add");
        }

        public async Task<ReceivingWayEntity> AddAsync(ReceivingWayEntity entity)
        {
            await KeyVaultStorage.AddOrUpdateSecretAsync(entity.Id.ToKey(), entity.StripePrivateConfig.ToJson());
            return await Repository.AddAsync(entity);
        }

        public void Update(ReceivingWayEntity entity)
        {
            Repository.Update(entity);
        }

        public Task DeleteAsync(Guid id)
        {
            return Repository.DeleteAsync(id);
        }

        public Task DeleteAsync(ReceivingWayEntity entity)
        {
            return Repository.DeleteAsync(entity);
        }

        public void Delete(ReceivingWayEntity entity)
        {
            Repository.Delete(entity);
        }
    }

    internal static class ExtensionMethods
    {
        public static string ToKey(this Guid id)
        {
            return "payment-receiving-way-" + id + "-configuration";
        }
    }
}
