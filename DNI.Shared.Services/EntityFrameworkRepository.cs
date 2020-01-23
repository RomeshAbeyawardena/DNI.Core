using DNI.Shared.Contracts;
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
    public class EntityFrameworkRepository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public EntityFrameworkRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
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
            var keyProperties = GetKeyProperties();
            if(keyProperties.All(keyProperty => keyProperty.GetValue(entity) == default))
                _dbSet.Add(entity);
            else
                _dbSet.Update(entity);
               
            if(saveChanges)
                await _dbContext.SaveChangesAsync();
            
            return entity;
        }

        private IEnumerable<PropertyInfo> GetKeyProperties()
        {
            var entityType = typeof(TEntity);

            return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttribute<KeyAttribute>() != null);
        }
    }
}
