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
using Microsoft.EntityFrameworkCore.Infrastructure;
using DNI.Shared.Contracts.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace DNI.Shared.Services.Abstraction
{
    public abstract class DbContextBase : DbContext
    {
        private readonly bool _useSingularTableNames;
        private readonly bool _useModifierFlagAttributes;
        
        protected DbContextBase(DbContextOptions dbContextOptions, 
            bool useSingularTableNames = true, 
            bool useModifierFlagAttributes = true)
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

            var defaultValueProperties = GetDefaultValueProperties<TEntity>();
            SetDefaultValues(defaultValueProperties, entity);

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

            var defaultValueProperties = GetDefaultValueProperties<TEntity>();
            SetDefaultValues(defaultValueProperties, entity);

            return base.Update(entity);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetTableNamesToSingular(modelBuilder.Model.GetEntityTypes(), _useSingularTableNames);
            base.OnModelCreating(modelBuilder);
        }

        private IEnumerable<PropertyInfo> GetDefaultValueProperties<TEntity>()
        {
            var entityType = typeof(TEntity);
            return GetCustomAttributeProperties<DefaultValueAttribute>(entityType);
        }

        private IEnumerable<PropertyInfo> GetModifierAttributeProperties<TEntity>()
        {
            var entityType = typeof(TEntity);
            return GetCustomAttributeProperties<ModifierAttribute>(entityType);

        }

        private IEnumerable<PropertyInfo> GetCustomAttributeProperties<TAttribute>(Type entityType)
            where TAttribute : Attribute
        {
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
            var isDateTime = false;
            var isDateTimeOffset = false;
            DateTimeOffset dateTimeOffset = default;
            DateTime dateTimeValue = default;

            if(value is DateTime _dateTimeValue)
            {
                isDateTime = true;
                dateTimeValue = _dateTimeValue;
            }

            if(value is DateTimeOffset _dateTimeOffset)
            {
                isDateTimeOffset = true;
                dateTimeOffset = _dateTimeOffset;
            }

            if(!isDateTime && !isDateTimeOffset)
                throw new InvalidOperationException();

            foreach(var property in properties)
            {
                if(property.PropertyType == typeof(DateTime) && isDateTimeOffset)
                     value = dateTimeOffset.DateTime;

                if(property.PropertyType == typeof(DateTimeOffset) && isDateTime)
                     value = new DateTimeOffset(dateTimeValue);

                property.SetValue(entity, value);
            }
        }

        private void SetDefaultValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity value)
        {    
            var service = this.GetInfrastructure().GetRequiredService<IDefaultValueGenerator<TEntity>>();

            if(service == null)
                return;

            foreach (var property in properties)
                property.SetValue(value, service.GetDefaultValue(property.Name, property.PropertyType));
            
        }

        private void SetTableName(IMutableEntityType mutableEntityType)
        {
            mutableEntityType.SetTableName(mutableEntityType.GetTableName().Singularize());
        }
    }
}
