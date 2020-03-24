namespace DNI.Core.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using DNI.Core.Contracts.Enumerations;
    using DNI.Core.Contracts.Services;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS0809
    public abstract class DbContextBase : DbContext
    {
        private readonly bool useSingularTableNames;
        private readonly bool useModifierFlagAttributes;
        private readonly bool useDefaultValueAttributes;

        protected DbContextBase(
            DbContextOptions dbContextOptions,
            bool useSingularTableNames = true,
            bool useModifierFlagAttributes = true,
            bool useDefaultValueAttributes = true)
            : base(dbContextOptions)
        {
            this.useSingularTableNames = useSingularTableNames;
            this.useModifierFlagAttributes = useModifierFlagAttributes;
            this.useDefaultValueAttributes = useDefaultValueAttributes;
        }

        protected IModifierFlagPropertyService ModifierFlagPropertyService => this.GetService<IModifierFlagPropertyService>();

        protected IDefaultValueSetterService DefaultValueSetterService => this.GetService<IDefaultValueSetterService>();

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
            if (useModifierFlagAttributes)
            {
                ModifierFlagPropertyService
                    .SetModifierFlagValues(entity, ModifierFlag.Created);
            }

            if (useDefaultValueAttributes)
            {
                DefaultValueSetterService.SetDefaultValues(entity);
            }

            return base.Add(entity);
        }

        protected virtual EntityEntry<TEntity> UpdateEntity<TEntity>(TEntity entity)
                        where TEntity : class
        {
            if (useModifierFlagAttributes)
            {
                ModifierFlagPropertyService.
                    SetModifierFlagValues(entity, ModifierFlag.Modified);
            }

            if (useDefaultValueAttributes)
            {
                DefaultValueSetterService.SetDefaultValues(entity);
            }

            return base.Update(entity);
        }

        protected virtual void ModelCreating(ModelBuilder modelBuilder)
        {
            SetTableNamesToSingular(modelBuilder.Model.GetEntityTypes(), useSingularTableNames);
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
            {
                foreach (var entityType in entityTypes)
                {
                    SetTableName(entityType);
                }
            }
        }

        private void SetTableName(IMutableEntityType mutableEntityType)
        {
            mutableEntityType.SetTableName(mutableEntityType.GetTableName().Singularize());
        }
    }
#pragma warning restore CS0809
}
