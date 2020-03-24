namespace DNI.Core.Contracts.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDataService<TEntity>
        where TEntity : class
    {
        IRepository<TEntity> Repository { get; }

        IQueryable<TEntity> DefaultQuery { get; }

        bool EnableTracking { get; }
    }
}
