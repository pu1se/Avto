using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentMS.DAL.Entities;

namespace PaymentMS.DAL.Repositories.RepositoryWithNoId
{
    public class SimpleCommandRepository<TEntity> : QueryRepository<TEntity>, ISimpleCommandRepository<TEntity> 
        where TEntity : class, IEntityWithTrackedDates
    {
        protected DbSet<TEntity> DbSet { get; }
        protected DataContext DataContext { get; }

        public SimpleCommandRepository(DataContext context, bool useNoTracking = false) : base(context, useNoTracking)
        {
            DbSet = context.Set<TEntity>();
            DataContext = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await DataContext.SaveChangesAsync();
            return entity;
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await DataContext.SaveChangesAsync();
        }
    }
}
