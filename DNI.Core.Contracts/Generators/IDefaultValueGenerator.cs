using System;
using System.Linq.Expressions;

namespace DNI.Core.Contracts.Generators
{
    /// <summary>
    /// Represents a generator that supplies default values for entitie properties and members
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDefaultValueGenerator<TEntity>
    {
        /// <summary>
        /// Adds a default value for a member or property
        /// </summary>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="selectProperty"></param>
        /// <param name="createInstance"></param>
        /// <returns></returns>
        IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<object> createInstance);

        /// <summary>
        /// Adds a default value for a member or property using a specified factory method with injectable services
        /// </summary>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="selectProperty"></param>
        /// <param name="createInstance"></param>
        /// <returns></returns>
        IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<IServiceProvider, object> createInstance);

        /// <summary>
        /// Retrieves a default value for a specified member or property of T
        /// </summary>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="selectProperty"></param>
        /// <returns></returns>
        TSelector GetDefaultValue<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty);

       /// <summary>
       /// Retrieves a default value for a specified member or property
       /// </summary>
       /// <param name="propertyName"></param>
       /// <param name="propertyType"></param>
       /// <returns></returns>       
        object GetDefaultValue(string propertyName, Type propertyType);
    }
}
