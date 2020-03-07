using Microsoft.EntityFrameworkCore;

namespace DNI.Core.Contracts
{
    public interface IImplementedRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        
    }
}
