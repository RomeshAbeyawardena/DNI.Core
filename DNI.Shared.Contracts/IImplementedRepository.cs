using Microsoft.EntityFrameworkCore;

namespace DNI.Shared.Contracts
{
    public interface IImplementedRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        
    }
}
