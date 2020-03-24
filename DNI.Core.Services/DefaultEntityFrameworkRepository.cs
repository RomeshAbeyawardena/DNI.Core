namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive.Subjects;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Domains.States;
    using DNI.Core.Services.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    internal sealed class DefaultEntityFrameworkRepository<TDbContext, TEntity> : IImplementedRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly IEnumerable<PropertyInfo> keyProperties;
        private readonly ISubject<RepositoryState> repositoryStateSubject;

        public DefaultEntityFrameworkRepository(TDbContext dbContext, ISubject<RepositoryState> repositoryStateSubject)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
            keyProperties = GetKeyProperties();
            this.repositoryStateSubject = repositoryStateSubject;
        }

        public Action<TEntity> ConfigureSoftDeletion { get; set; }

        public DbSet<TEntity> DbSet { get; }

        public TDbContext DbContext { get; }

        public IAsyncQueryResultTransformer<TEntity> For(IQueryable<TEntity> query)
            => DefaultAsyncQueryResultTransformer.Create(query);

        public IAsyncQueryResultTransformer<T> For<T>(IQueryable<T> query)
            where T : class
            => DefaultAsyncQueryResultTransformer.Create(query);

        public IQueryable<TQueryEntity> FromQuery<TQueryEntity>(string query, params object[] parameters)
            where TQueryEntity : class
        {
            return DbContext
                .Set<TQueryEntity>()
                .FromSqlRaw(query, parameters);
        }

        public async Task<TEntity> Find(bool enableTracking = true, CancellationToken cancellationToken = default, params object[] keys)
        {
            var foundEntry = await DbSet.FindAsync(keys, cancellationToken);

            if (foundEntry == null)
            {
                return default;
            }

            if (enableTracking)
            {
                return foundEntry;
            }

            var entry = DbContext.Entry(foundEntry);
            entry.State = EntityState.Detached;

            return foundEntry;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression = null, bool enableTracking = true)
        {
            if (whereExpression == null)
            {
                return DbSet;
            }

            var query = DbSet.Where(whereExpression);

            if (!enableTracking)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        public async Task<TEntity> SaveChanges(
            TEntity entity,
            bool saveChanges = true,
            bool detachAfterSave = true,
            CancellationToken cancellationToken = default)
        {
            EntityEntry<TEntity> entry;

            if (keyProperties.All(keyProperty => IsValueDefault(keyProperty, entity)))
            {
                entry = DbSet.Add(entity);
            }
            else
            {
                entry = DbSet.Update(entity);
            }

            if (saveChanges)
            {
                await Commit(true, cancellationToken);
            }

            repositoryStateSubject.OnNext(new RepositoryState
            {
                State = entry.State,
                Type = typeof(TEntity),
                Value = entity,
            });

            if (detachAfterSave)
            {
                DbContext.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public async Task<int> Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = DbSet.Remove(entity);

            repositoryStateSubject.OnNext(new RepositoryState
            {
                State = entityEntry.State,
                Type = typeof(TEntity),
                Value = entity,
            });

            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> Delete(TEntity entity, bool softDelete = true, CancellationToken cancellationToken = default)
        {
            if (softDelete && ConfigureSoftDeletion == null)
            {
                throw new ArgumentNullException(nameof(ConfigureSoftDeletion));
            }

            if (!softDelete)
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

        public async Task<int> Commit(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            return await DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public IAsyncQueryResultTransformer<TEntity> For(Expression<Func<TEntity, bool>> whereExpression)
        {
            return For(Query(whereExpression));
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

            // use internal reflection as fallback.
            if (dbContextEntityType == null)
            {
                return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(property => property.GetCustomAttribute<KeyAttribute>() != null);
            }

            var key = dbContextEntityType.FindPrimaryKey();

            return key.Properties.Select(property => property.PropertyInfo);
        }
    }
}
