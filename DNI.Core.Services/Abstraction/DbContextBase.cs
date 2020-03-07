using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using DNI.Core.Contracts.Services;
using DNI.Core.Contracts.Enumerations;

namespace DNI.Core.Services.Abstraction
{
    public abstract class DbContextBase : DbContext
    {
        private readonly bool _useSingularTableNames;
        private readonly bool _useModifierFlagAttributes;
        private readonly bool _useDefaultValueAttributes;

        protected IModifierFlagPropertyService ModifierFlagPropertyService => this.GetService<IModifierFlagPropertyService>();
        protected IDefaultValueSetterService DefaultValueSetterService => this.GetService<IDefaultValueSetterService>();

        protected DbContextBase(DbContextOptions dbContextOptions, 
            bool useSingularTableNames = true, 
            bool useModifierFlagAttributes = true,
            bool useDefaultValueAttributes = true)
            : base(dbContextOptions)
        {
            _useSingularTableNames = useSingularTableNames;
            _useModifierFlagAttributes = useModifierFlagAttributes;
            _useDefaultValueAttributes = useDefaultValueAttributes;
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if(_useModifierFlagAttributes)
                ModifierFlagPropertyService
                    .SetModifierFlagValues(entity, ModifierFlag.Created);

            if(_useDefaultValueAttributes)
                DefaultValueSetterService.SetDefaultValues(entity);

            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if(_useModifierFlagAttributes)
                ModifierFlagPropertyService.
                    SetModifierFlagValues(entity, ModifierFlag.Modified);

            if(_useDefaultValueAttributes)
                DefaultValueSetterService.SetDefaultValues(entity);

            return base.Update(entity);       
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
