using DNI.Core.Contracts;
using DNI.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstraction
{
    public abstract class DataServiceBase<TEntity> : IDataService<TEntity>
        where TEntity : class
    {        
        private readonly Expression<Func<TEntity, bool>> _defaultQueryExpression;

        public DataServiceBase(IRepository<TEntity> repository, 
            bool enableTracking = true,
            Expression<Func<TEntity, bool>> defaultQueryExpression = default)
        {
            Repository = repository;
            EnableTracking = enableTracking;
            _defaultQueryExpression = defaultQueryExpression;
        }

        public IRepository<TEntity> Repository { get; }

        public bool EnableTracking { get; }

        public IQueryable<TEntity> DefaultQuery => Repository.Query(_defaultQueryExpression, EnableTracking);
    }
}
