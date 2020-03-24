namespace DNI.Core.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Query;

    public interface IQueryResultTransformer<T>
    {
        IQueryable<T> AsNoTracking();

        IPagerResult<T> AsPager();

        IIncludableQueryable<T, TProperty> Include<TProperty>(
            Expression<Func<T, TProperty>> navigationPropertyPath = default,
            string propertyName = default);
    }
}
