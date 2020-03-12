using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using DNI.Core.Contracts.Services;
using DNI.Core.Contracts.Enumerations;
using System;

namespace DNI.Core.Services.Abstraction
{
    #pragma warning disable CS0809
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


        [Obsolete("Override AddEntity instead to enable Table singularing functionality")]
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return AddEntity(entity);
        }
        [Obsolete("Override UpdateEntity instead to enable Table singularing functionality")]
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            return UpdateEntity(entity);
        }

        protected virtual EntityEntry<TEntity> AddEntity<TEntity>(TEntity entity)
            where TEntity : class
        {
            if(_useModifierFlagAttributes)
                ModifierFlagPropertyService
                    .SetModifierFlagValues(entity, ModifierFlag.Created);

            if(_useDefaultValueAttributes)
                DefaultValueSetterService.SetDefaultValues(entity);

            return base.Add(entity);
        }

        protected virtual EntityEntry<TEntity> UpdateEntity<TEntity>(TEntity entity)
                        where TEntity : class
        {
            if(_useModifierFlagAttributes)
                ModifierFlagPropertyService.
                    SetModifierFlagValues(entity, ModifierFlag.Modified);

            if(_useDefaultValueAttributes)
                DefaultValueSetterService.SetDefaultValues(entity);

            return base.Update(entity);   
        }

        protected virtual void ModelCreating(ModelBuilder modelBuilder)
        {
            SetTableNamesToSingular(modelBuilder.Model.GetEntityTypes(), _useSingularTableNames);
        }

        [Obsolete("Override ModelCreating instead to enable Table singularing functionality")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreating(modelBuilder);
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
    #pragma warning restore CS0809
}
