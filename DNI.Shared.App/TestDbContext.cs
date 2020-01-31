using DNI.Shared.App.Domains;
using DNI.Shared.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DNI.Shared.App
{
    public class TestDbContext : DbContextBase
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions, true, true)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
