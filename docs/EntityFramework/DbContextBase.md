# DbContextBase

Extends DbContext functionality to support the following new features:

- Singularise table names used in SQL queries 
generated internally.
- IModifier flag properties (Created and Modified flags on Add and update)
- Default value properties.

## Usage
    using DNI.Core.Services.Abstraction;
    using Microsoft.EntityFrameworkCore;
    public class TestDbContext : DbContextBase
    {
        public TestDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions, useSingularTableNames: true, 
                useModifierFlagAttributes: true, 
                useDefaultValueAttributes: true)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }

### Base Parameters

- DbContextOptions
  - The default DbContextOptions object passed to the base DbContext
- UseSingularTableNames.
  - Flag to determine whether the table named used in SQL queries 
generated internally should be singularised (using Humanizer)
- UseModifierFlagAttributes.
  - Flag to determine whether the Modifier flag logic should run during
save operations.
- UseDefaultValueAttributes
  - Flag to determine whether the Default value logic should run during
save operations. 