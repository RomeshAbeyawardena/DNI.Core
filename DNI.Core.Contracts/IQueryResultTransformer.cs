using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DNI.Core.Contracts
{
    public interface IQueryResultTransformer<T>
    {
        IQueryable<T> AsNoTracking();
        IPagerResult<T> AsPager();
        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath = default, 
            string propertyName = default);
    }
}
