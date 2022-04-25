using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.DAL.Entities;

namespace PaymentMS.DAL.Repositories
{
    public interface IGuidIdCommandRepository<TEntity> : IQueryRepository<TEntity> 
        where TEntity : class, IEntityWithGuidId
    {
        Task<TEntity> AddAsync(TEntity entity);
        void Add(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(TEntity entity);
        void Delete(TEntity entity);
    }
}
