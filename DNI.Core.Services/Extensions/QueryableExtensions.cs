using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IAsyncQueryResultTransformer<T> For<T>(this IQueryable<T> queryable, IRepository<T> repository)
            where T : class
        {
            return repository.For(queryable);
        }
    }
}
