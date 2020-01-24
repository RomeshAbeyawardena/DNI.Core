using DNI.Shared.App.Domains;
using DNI.Shared.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
