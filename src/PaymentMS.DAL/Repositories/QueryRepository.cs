using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using PaymentMS.DAL.Entities;

namespace PaymentMS.DAL.Repositories
{    
    public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
    {
        private List<Expression<Func<TEntity, object>>> IncludeList { get; } = new List<Expression<Func<TEntity, object>>>();
        private List<Expression<Func<TEntity, bool>>> FilterList { get; } = new List<Expression<Func<TEntity, bool>>>();
        private List<SortOrdering<TEntity>> OrderingList { get; } = new List<SortOrdering<TEntity>>();
        private IQueryable<TEntity> Query { get; set; }
        private bool UseNoTracking { get; }
        

        public QueryRepository(DataContext context, bool useNoTracking)
        {
            Query = context.Set<TEntity>().AsQueryable();
            //todo: start UseAsNoTracking from constructor
            UseNoTracking = false;
        }

        public QueryRepository(IQueryable<TEntity> query)
        {
            Query = query;
        }
                

        public virtual IQueryRepository<TEntity> Where(Expression<Func<TEntity, bool>> filterExpression)
        {
            if (filterExpression != null)
                FilterList.Add(filterExpression);
            return this;
        }

        public virtual IQueryRepository<TEntity> Include(Expression<Func<TEntity, object>> includeExpression)
        {
            if (includeExpression != null)
                IncludeList.Add(includeExpression);
            return this;
        }

        public virtual IQueryRepository<TEntity> OrderBy(Expression<Func<TEntity, object>> orderExpression)
        {
            if (orderExpression != null)
                OrderingList.Add(new SortOrdering<TEntity>(SortOrderType.Asc, orderExpression));
            return this;
        }

        public virtual IQueryRepository<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderExpression)
        {
            if (orderExpression != null)
                OrderingList.Add(new SortOrdering<TEntity>(SortOrderType.Desc, orderExpression));
            return this;
        }

        public virtual IQueryRepository<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selectExpression) where TResult : class
        {
            var query = PrepareQuery().Select(selectExpression);
            IQueryRepository<TResult> repository = new QueryRepository<TResult>(query);
            return repository;
        }

        public IQueryable<TEntity> PrepareQuery()
        {
            var query = Query;

            foreach (var filter in FilterList)
            {
                query = query.Where(filter);
            }
            
            foreach (var include in IncludeList)
            {
                query = query.Include(include);
            }

            if (OrderingList.Count > 0)
            {
                IOrderedQueryable<TEntity> orderQuery;
                var firstOrder = OrderingList.First();
                if (firstOrder.SortOrder == SortOrderType.Asc)
                {
                    orderQuery = query.OrderBy(firstOrder.ByExpression);
                }
                else
                {
                    orderQuery = query.OrderByDescending(firstOrder.ByExpression);
                }

                foreach (var order in OrderingList.Skip(1))
                {
                    if (order.SortOrder == SortOrderType.Asc)
                    {
                        orderQuery = orderQuery.ThenBy(order.ByExpression);
                    }
                    else
                    {
                        orderQuery = orderQuery.ThenByDescending(order.ByExpression);
                    }
                }

                query = orderQuery;
            }            

            return query;
        }

        public virtual Task<List<TEntity>> ToListAsync()
        {
            return PrepareQuery().ToListAsync();
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            Where(filter);
            var query = PrepareQuery();
            if (UseNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.AnyAsync();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                Where(filter);
            }
            
            var query = PrepareQuery();
            if (UseNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefaultAsync();
        }
    }
}
