namespace DNI.Core.Contracts
{
    using Microsoft.EntityFrameworkCore;

    public interface IImplementedRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        TDbContext DbContext { get; }

        DbSet<TEntity> DbSet { get; }
    }
}
