using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Options;
using DNI.Shared.Services.Extensions;
using DNI.Shared.Services.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultEntityFrameworkRepository<TDbContext, TEntity> : IImplementedRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly IEnumerable<PropertyInfo> _keyProperties;
        private TDbContext DbContext { get; }
        private readonly DbSet<TEntity> _dbSet;


        public DefaultEntityFrameworkRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
            _keyProperties = GetKeyProperties();
        }

        public async Task<TEntity> Find(CancellationToken cancellationToken = default, params object[] keys)
        {
            return await _dbSet.FindAsync(keys, cancellationToken);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression = null, bool enableTracking = true)
        {
            if(whereExpression == null)
                return _dbSet;

            var query = _dbSet.Where(whereExpression);
            
            if(!enableTracking)
                return query.AsNoTracking();

            return query;
        }

        public async Task<TEntity> SaveChanges(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
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

        public IPagerResult<TEntity> GetPager(IQueryable<TEntity> query)
        {
            return DefaultPagerResult
                .Create(query);
        }

        public IQueryable<TEntity> AsNoTracking(IQueryable<TEntity> query)
        {
            return query.AsNoTracking();
        }
    }
}
