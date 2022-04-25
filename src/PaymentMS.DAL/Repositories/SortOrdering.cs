using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PaymentMS.DAL.Repositories
{
    public enum SortOrderType
    {
        Asc,
        Desc
    }

    public class SortOrdering<TEntity>
    {
        public SortOrderType SortOrder { get; }
        public Expression<Func<TEntity, object>> ByExpression { get; }


        public SortOrdering(SortOrderType sortOrdering, Expression<Func<TEntity, object>> byExpression)
        {
            SortOrder = sortOrdering;
            ByExpression = byExpression;
        }
    }
}
