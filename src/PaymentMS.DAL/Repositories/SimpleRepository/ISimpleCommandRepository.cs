using System.Threading.Tasks;
using PaymentMS.DAL.Entities;

namespace PaymentMS.DAL.Repositories.RepositoryWithNoId
{
    public interface ISimpleCommandRepository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class, IEntityWithTrackedDates
    {
        void Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Update(TEntity entity);
    }
}