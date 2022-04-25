using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Avto.DAL.Entities;

namespace Avto.DAL.Repositories
{    
    public class GuidIdCommandRepository<TEntity> : QueryRepository<TEntity>, IGuidIdCommandRepository<TEntity> 
        where TEntity : class, IEntityWithGuidId
    {
        protected DbSet<TEntity> DbSet { get; }
        protected DataContext DataContext { get; }

        public GuidIdCommandRepository(DataContext context, bool useNoTracking = false) : base(context, useNoTracking)
        {
            DbSet = context.Set<TEntity>();
            DataContext = context;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            DbSet.Add(entity);
            await DataContext.SaveChangesAsync();
            return entity;
        }

        public virtual void Add(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(x => x.Id == id);
            if (entity == null)
                return;

            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await DataContext.SaveChangesAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
