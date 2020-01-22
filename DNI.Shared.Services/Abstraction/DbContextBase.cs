using DNI.Shared.Contracts;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Abstraction
{
    public abstract class DbContextBase : DbContext
    {
        private readonly bool _useSingularTableNames;

        protected DbContextBase(DbContextOptions dbContextOptions, bool useSingularTableNames = true)
            : base(dbContextOptions)
        {
            _useSingularTableNames = useSingularTableNames;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetTableNamesToSingular(modelBuilder.Model.GetEntityTypes(), _useSingularTableNames);
            base.OnModelCreating(modelBuilder);
        }

        private void SetTableNamesToSingular(IEnumerable<IMutableEntityType> entityTypes, bool useSingularTableNames)
        {
            if (useSingularTableNames)
                foreach (var entityType in entityTypes)
                    SetTableName(entityType);
        }

        private void SetTableName(IMutableEntityType mutableEntityType)
        {
            mutableEntityType.SetTableName(mutableEntityType.GetTableName().Singularize());
        }
    }
}
