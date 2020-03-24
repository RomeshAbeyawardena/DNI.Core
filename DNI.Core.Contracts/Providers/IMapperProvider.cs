namespace DNI.Core.Contracts
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a mapper that encapsulates AutoMapper.
    /// </summary>
    public interface IMapperProvider
    {
        /// <summary>
        /// Maps TSource to a new instance of TDestination using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        TDestination Map<TSource, TDestination>(TSource source);

        /// <summary>
        /// Maps an array of TSource to a new array instance of TDestination using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
