using DNI.Shared.Contracts;
using DNI.Shared.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class EntityFrameworkRepository<TDbContext, TEntity> : IImplementedRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly IEnumerable<PropertyInfo> _keyProperties;
        private TDbContext DbContext { get; }
        private readonly DbSet<TEntity> _dbSet;
        public EntityFrameworkRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
            _keyProperties = GetKeyProperties();
        }

        public async Task<TEntity> Find(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression = null)
        {
            if(whereExpression == null)
                return _dbSet;

            return _dbSet.Where(whereExpression);
        }

        public async Task<TEntity> SaveChanges(TEntity entity, bool saveChanges = true)
        {
            if(_keyProperties.All(keyProperty => IsValueDefault(keyProperty, entity) ))
                _dbSet.Add(entity);
            else
                _dbSet.Update(entity);
               
            if(saveChanges)
                await DbContext.SaveChangesAsync();
            
            return entity;
        }

        private bool IsValueDefault(PropertyInfo propertyInfo, TEntity entity)
        {
            var value = propertyInfo.GetValue(entity);
            var defaultValue = propertyInfo.GetDefaultValue();
            return value.Equals(defaultValue);
        }

        private IEnumerable<PropertyInfo> GetKeyProperties()
        {
            var entityType = typeof(TEntity);
            var dbContextEntityType = DbContext.Model
                .GetEntityTypes()
                .SingleOrDefault(entity => entity.ClrType == entityType);
            
            if(dbContextEntityType == null) //use internal reflection as fallback.
                return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(property => property.GetCustomAttribute<KeyAttribute>() != null);

            var key = dbContextEntityType.FindPrimaryKey();

            return key.Properties.Select(property => property.PropertyInfo);
        }
    }
}
