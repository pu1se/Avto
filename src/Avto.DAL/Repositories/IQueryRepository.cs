using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL.Entities;

namespace Avto.DAL.Repositories
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        IQueryRepository<TEntity> Where(Expression<Func<TEntity, bool>> filterExpression);
        IQueryRepository<TEntity> Include(Expression<Func<TEntity, object>> includeExpression);
        Task<List<TEntity>> ToListAsync();
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null);
        IQueryRepository<TEntity> OrderBy(Expression<Func<TEntity, object>> orderExpression);
        IQueryRepository<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderExpression);

        IQueryRepository<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selectExpression)
            where TResult : class;
    }
}
