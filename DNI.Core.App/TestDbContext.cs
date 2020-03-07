using DNI.Core.App.Domains;
using DNI.Core.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DNI.Core.App
{
    public class TestDbContext : DbContextBase
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions, useSingularTableNames: true, useModifierFlagAttributes: true, useDefaultValueAttributes: true)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
