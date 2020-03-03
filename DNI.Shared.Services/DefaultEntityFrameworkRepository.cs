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
using System.Transactions;

namespace DNI.Shared.Services
{
    internal sealed class DefaultEntityFrameworkRepository<TDbContext, TEntity> : IImplementedRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly IEnumerable<PropertyInfo> _keyProperties;
        private TDbContext DbContext { get; }

        public Action<TEntity> ConfigureSoftDeletion { get; set; }

        private readonly DbSet<TEntity> _dbSet;

        public IQueryable<TQueryEntity> FromQuery<TQueryEntity>(string query, params object[] parameters)
            where TQueryEntity : class
        {
            return DbContext
                .Set<TQueryEntity>()
                .FromSqlRaw(query, parameters);
        }

        public DefaultEntityFrameworkRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
            _keyProperties = GetKeyProperties();
        }

        public async Task<TEntity> Find(bool enableTracking = true, CancellationToken cancellationToken = default, params object[] keys)
        {
            var foundEntry = await _dbSet.FindAsync(keys, cancellationToken);

            if(enableTracking)
                return foundEntry;

            var entry = DbContext.Entry(foundEntry);
            entry.State = EntityState.Detached;
            
            return foundEntry;
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

        public async Task<TEntity> SaveChanges(TEntity entity, bool saveChanges = true, 
            bool detachAfterSave = true, CancellationToken cancellationToken = default)
        {
            if(_keyProperties.All(keyProperty => IsValueDefault(keyProperty, entity) ))
                _dbSet.Add(entity);
            else
                _dbSet.Update(entity);
               
            if(saveChanges)
                await Commit();

            if(detachAfterSave)
                DbContext.Entry(entity).State = EntityState.Detached;

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

        public async Task<int> Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            
            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> Delete(TEntity entity, bool softDelete = true, CancellationToken cancellationToken = default)
        {
            if(softDelete && ConfigureSoftDeletion == null)
                throw new ArgumentNullException(nameof(ConfigureSoftDeletion));
                
            if(!softDelete)
                return await Delete(entity, cancellationToken);
            else
            { 
                ConfigureSoftDeletion(entity);
                await SaveChanges(entity, true, cancellationToken: cancellationToken);
                return 1;
            }
        }

        public async Task<int> Delete(CancellationToken cancellationToken = default, params object[] keys)
        {
            var foundEntity = await Find(true, cancellationToken, keys);
            return await Delete(foundEntity, cancellationToken);
        }

        public async Task<int> Delete(bool softDelete = true, CancellationToken cancellationToken = default, params object[] keys)
        {
            var foundEntity = await Find(true, cancellationToken, keys);
            return await Delete(foundEntity, softDelete, cancellationToken);
        }

        public async Task<int> Commit()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
