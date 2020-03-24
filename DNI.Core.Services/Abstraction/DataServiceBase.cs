namespace DNI.Core.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Services;

#pragma warning disable CA1012
    public abstract class DataServiceBase<TEntity> : IDataService<TEntity>
        where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> defaultQueryExpression;

        public DataServiceBase(
            IRepository<TEntity> repository,
            bool enableTracking = true,
            Expression<Func<TEntity, bool>> defaultQueryExpression = default)
        {
            Repository = repository;
            EnableTracking = enableTracking;
            this.defaultQueryExpression = defaultQueryExpression;
        }

        public IRepository<TEntity> Repository { get; }

        public bool EnableTracking { get; }

        public IQueryable<TEntity> DefaultQuery => Repository.Query(defaultQueryExpression, EnableTracking);
    }
#pragma warning restore CA1012
}
