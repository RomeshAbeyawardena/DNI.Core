using System;
using System.Linq.Expressions;

namespace DNI.Shared.Contracts.Generators
{
    public interface IDefaultValueGenerator<TEntity>
    {
        IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<object> createInstance);

        IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<IServiceProvider, object> createInstance);

        TSelector GetDefaultValue<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty);
        object GetDefaultValue(string propertyName, Type propertyType);
    }
}
