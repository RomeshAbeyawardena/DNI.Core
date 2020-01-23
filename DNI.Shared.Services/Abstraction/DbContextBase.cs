using DNI.Shared.Contracts;
using DNI.Shared.Domains.Enumerations;
using DNI.Shared.Services.Attributes;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Abstraction
{
    public abstract class DbContextBase : DbContext
    {
        private readonly bool _useSingularTableNames;
        private readonly bool _useModifierFlagAttributes;

        protected DbContextBase(DbContextOptions dbContextOptions, bool useSingularTableNames = true, bool useModifierFlagAttributes = true)
            : base(dbContextOptions)
        {
            _useSingularTableNames = useSingularTableNames;
            _useModifierFlagAttributes = useModifierFlagAttributes;
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if(!_useModifierFlagAttributes)
                return base.Add(entity);

            var modifierAttributeProperties = GetModifierAttributeProperties<TEntity>();

            var createdModifierFlagAttributes = modifierAttributeProperties
                .Where(a => a.GetCustomAttribute<ModifierAttribute>()?.ModifierFlag == ModifierFlag.Created);

            SetModifierFlagValues(createdModifierFlagAttributes, entity, DateTime.Now);

            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if(!_useModifierFlagAttributes)
                return base.Update(entity);

            var modifierAttributeProperties = GetModifierAttributeProperties<TEntity>();

            var createdModifierFlagAttributes = modifierAttributeProperties
                .Where(a => a.GetCustomAttribute<ModifierAttribute>()?.ModifierFlag == ModifierFlag.Modified);

            SetModifierFlagValues(createdModifierFlagAttributes, entity, DateTime.Now);


            return base.Update(entity);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetTableNamesToSingular(modelBuilder.Model.GetEntityTypes(), _useSingularTableNames);
            base.OnModelCreating(modelBuilder);
        }

        private IEnumerable<PropertyInfo> GetModifierAttributeProperties<TEntity>()
        {
            var entityType = typeof(TEntity);
            
            return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttribute<ModifierAttribute>() != null);

        }

        private void SetTableNamesToSingular(IEnumerable<IMutableEntityType> entityTypes, bool useSingularTableNames)
        {
            if (useSingularTableNames)
                foreach (var entityType in entityTypes)
                    SetTableName(entityType);
        }

        private void SetModifierFlagValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity entity, object value)
        {
            foreach(var property in properties)
            {
                if(property.PropertyType == typeof(DateTime) && value is DateTimeOffset dateTimeOffset)
                     value = dateTimeOffset.DateTime;

                if(property.PropertyType == typeof(DateTimeOffset) && value is DateTime dateTimeValue)
                     value = new DateTimeOffset(dateTimeValue);

                property.SetValue(entity, value);
            }
        }

        private void SetTableName(IMutableEntityType mutableEntityType)
        {
            mutableEntityType.SetTableName(mutableEntityType.GetTableName().Singularize());
        }
    }
}
